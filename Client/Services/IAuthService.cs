using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Client.Services;

public interface IAuthService
{
    Task<bool> Login(LoginRequest loginRequest);
    Task Logout();
}