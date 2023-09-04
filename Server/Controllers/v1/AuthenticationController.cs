using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SycappsWeb.Shared.Models.Un2Trek;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SycappsWeb.Server.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;    
    private readonly IConfiguration config;

    public AuthenticationController(UserManager<IdentityUser> userManager,                                
                                    IConfiguration config)
    {
        this.userManager = userManager;        
        this.config = config;
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
            return Unauthorized(StringConstants.InvalidCredentialsErrorCode);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);  
        }
        
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Post(RegisterRequest registerRequest)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(identityUser, registerRequest.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(identityUser, "Un2TrekUser");
            await userManager.AddClaimAsync(identityUser, new Claim("fullName", $"{registerRequest.Name} {registerRequest.LastName}"));

            return await SetupToken(registerRequest.Email);
        }
        else
        {
            return BadRequest(result.Errors);
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

        var fullNameClaim = userClaims.Where(c => c.Type.Equals("FullName", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

        return fullNameClaim!;
    }
    private string BuildToken(IdentityUser identityUser, List<string> userRoles, string fullUserName)
    {
        var claims = new List<Claim>()
        {
            new Claim("userRoles", string.Join(", ", userRoles).TrimEnd(',',' '))
        };
        claims.Add(new(JwtRegisteredClaimNames.Sub, identityUser.Id));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, identityUser.Email.ToString()));

        if (!string.IsNullOrEmpty(fullUserName))
        {
            claims.Add(new("fullName", fullUserName));
        }
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetValue<string>("Authentication:SecretKey")!));
        var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //var expiration = DateTime.UtcNow.AddMinutes(120);
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
