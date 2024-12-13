using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(UserLogin user);
    }
}
