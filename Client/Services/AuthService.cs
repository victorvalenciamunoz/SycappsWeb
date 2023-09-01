using Microsoft.AspNetCore.Components.Authorization;
using SycappsWeb.Shared.Models.Un2Trek;
using System.Net.Http.Json;

namespace SycappsWeb.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authStateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        this.httpClient = httpClient;
        this.authStateProvider = authStateProvider;
    }
    public async Task<bool> Login(LoginRequest loginRequest)
    {
        var result = await httpClient.PostAsJsonAsync<LoginRequest>("api/v1/authentication/login", loginRequest);
        if (result.IsSuccessStatusCode)
        {
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string token = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    await ((CustomStateProvider)authStateProvider).MarkUserAsAuthenticated(token);
                    return true;
                }
                else
                {
                    await ((CustomStateProvider)authStateProvider).MarkUserAsLoggedOut();
                }
            }
        }
        return false;
    }

    public async Task Logout()
    {
        await ((CustomStateProvider)authStateProvider).MarkUserAsLoggedOut();
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var result = await httpClient.PostAsJsonAsync("api/v1/authentication/register", registerRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
}
