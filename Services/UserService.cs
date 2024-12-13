using ClientSideLibraryManagementSystem.Models;
using System.Transactions;

namespace ClientSideLibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddUserAsync(UsersEntity user)
        {
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(user.Username), "Username");
            formData.Add(new StringContent(user.Email), "Email");
            formData.Add(new StringContent(user.Password), "Password");
            formData.Add(new StringContent(user.Role), "Role");
            var response = await _httpClient.PostAsync("https://localhost:7084/api/Users", formData);

            // Return whether the API request was successful
            return response.IsSuccessStatusCode;

        }

        public Task DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UsersEntity>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UsersEntity> GetUserByUsernameAsync(string username)
        {
            var user = await _httpClient.GetFromJsonAsync<UsersEntity>($"https://localhost:7084/api/Users/{username}");
            if (user == null)
            {
                throw new KeyNotFoundException("User not foound");
            }

            return user;
        }

        public Task UpdateUserAsync(UsersEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
