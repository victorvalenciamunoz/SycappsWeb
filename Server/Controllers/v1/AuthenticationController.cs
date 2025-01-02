using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SycappsWeb.Shared.Models.Un2Trek;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using SendGrid.Helpers.Mail;
using SendGrid;
using SycappsWeb.Server.Models;
using SycappsWeb.Shared.Entities;

namespace SycappsWeb.Server.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration config;
    private readonly IEventService eventService;

    public AuthenticationController(UserManager<ApplicationUser> userManager,
                                    IConfiguration config, IEventService eventService)
    {
        this.userManager = userManager;
        this.config = config;
        this.eventService = eventService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginRequest loginRequest)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(loginRequest.Email);
            if (existingUser != null)
            {
                if (await userManager.CheckPasswordAsync(existingUser, loginRequest.Password))
                {
                    return Ok(await SetupToken(loginRequest.Email));
                }
            }
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials",
                Detail = StringConstants.InvalidCredentialsErrorCode
            });
        }
        catch (Exception ex)
        {            
            Log.Logger.Fatal("General exception login user {@loginRequest} {@ex}", loginRequest, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
        }
    }

    [HttpGet("renew")]
    public async Task<ActionResult<string>> Renew()
    {
        var emailClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "userName");
        if (emailClaim != null)
        {
            return Ok(await SetupToken(emailClaim.Value));
        }
        else
        {
            return BadRequest("Could not renew token");
        }
    }

    

    private async Task<string> SetupToken(string userEmail)
    {
        var registeredUser = await userManager.FindByEmailAsync(userEmail);
        if (registeredUser != null)
        {
            IList<string> userRoles = await userManager.GetRolesAsync(registeredUser);
            string fullName = string.Empty;
            var fullNameClaim = await GetFullNameClaim(registeredUser);
            if (fullNameClaim != null)
            {
                fullName = fullNameClaim.Value;
            }
            return BuildToken(registeredUser, userRoles.ToList(), fullName);
        }

        return string.Empty;
    }
    private async Task<Claim> GetFullNameClaim(ApplicationUser identityUser)
    {
        var userClaims = await userManager.GetClaimsAsync(identityUser);

        var fullNameClaim = userClaims.FirstOrDefault(c => c.Type.Equals("FullName", StringComparison.InvariantCultureIgnoreCase));

        return fullNameClaim!;
    }
    private string BuildToken(ApplicationUser identityUser, List<string> userRoles, string fullUserName)
    {
        var claims = new List<Claim>()
        {
            new Claim("userRoles", string.Join(", ", userRoles).TrimEnd(',',' '))
        };
        claims.Add(new(JwtRegisteredClaimNames.Sub, identityUser.Id));
        claims.Add(new("userName", identityUser.Email!.ToString()));

        if (!string.IsNullOrEmpty(fullUserName))
        {
            claims.Add(new("fullName", fullUserName));
        }
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetValue<string>("Authentication:SecretKey")!));
        var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.Now;
        var expiration = now.AddMinutes(180);
        var token = new JwtSecurityToken(
                            issuer: config.GetValue<string>("Authentication:Issuer"),
                            audience: config.GetValue<string>("Authentication:Audience"),
                            claims: claims,
                            notBefore: now,
                            expires: expiration,
                            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
