using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SycappsWeb.Client.Services;

public class CustomStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;

    public CustomStateProvider(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>("userData");
        ClaimsPrincipal claimsPrincipal;
        if (string.IsNullOrEmpty(token))
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            claimsPrincipal = new ClaimsPrincipal(identity);
        }
        else
        {
            claimsPrincipal = GetCurrentUserClaims(token);
        }

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await localStorage.SetItemAsync("userToken", token);
        var claimsPrincipal = GetCurrentUserClaims(token);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    public async Task MarkUserAsLoggedOut()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(identity);

        await localStorage.RemoveItemAsync("userToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var claimsIdentity = new ClaimsIdentity("apiauth");

        var handler = new JwtSecurityTokenHandler();
        var decodedValue = handler.ReadJwtToken(token);
        foreach (var keyValuePair in decodedValue.Payload)
        {
            if (keyValuePair.Key.Contains("unique_name"))
            {
                claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Email, keyValuePair.Value.ToString()));
            }
            if (keyValuePair.Key.Contains("sub"))
            {
                claimsIdentity.AddClaim(new Claim(type: ClaimTypes.NameIdentifier, keyValuePair.Value.ToString()));
            }
            if (keyValuePair.Key.Contains("fullName"))
            {
                claimsIdentity.AddClaim(new Claim(type: "FullName", keyValuePair.Value.ToString()));
            }
            if (keyValuePair.Key.Contains("userRoles"))
            {
                claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Role, keyValuePair.Value.ToString()));
            }
        }

        return claimsIdentity;
    }
    private ClaimsPrincipal GetCurrentUserClaims(string token)
    {
        var identity = GetClaimsIdentity(token);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return claimsPrincipal;
    }
}
