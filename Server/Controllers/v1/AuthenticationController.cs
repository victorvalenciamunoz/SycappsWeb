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
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration config;
    private readonly IEventService eventService;

    public AuthenticationController(UserManager<IdentityUser> userManager,
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

    [HttpPost("register")]
    public async Task<ActionResult<string>> Post(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                Log.Logger.Error("Error validating user register data {@registerRequest} {@modelError.Errors}", registerRequest, modelError.Errors);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        var identityUser = new ApplicationUser
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            EmailConfirmed = false,
            ReceivePromotionalEmails = registerRequest.ReceivePromotionalEmails
        };
        var result = await userManager.CreateAsync(identityUser, registerRequest.Password);

        if (result.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = identityUser.Email }, Request.Scheme);

            var domainEvent = new UserRegisteredEvent(identityUser.Email, $"{registerRequest.Name} {registerRequest.LastName}", confirmationLink);
            await eventService.Publish(domainEvent);

            return Ok("Registration successful. Please check your email to confirm your account.");
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpGet("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Invalid email confirmation request.");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully.");
        }
        else
        {
            return BadRequest("Email confirmation failed.");
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
    private async Task<Claim> GetFullNameClaim(IdentityUser identityUser)
    {
        var userClaims = await userManager.GetClaimsAsync(identityUser);

        var fullNameClaim = userClaims.FirstOrDefault(c => c.Type.Equals("FullName", StringComparison.InvariantCultureIgnoreCase));

        return fullNameClaim!;
    }
    private string BuildToken(IdentityUser identityUser, List<string> userRoles, string fullUserName)
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
