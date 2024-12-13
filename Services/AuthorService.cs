using ClientSideLibraryManagementSystem.Models;
using System.Net.Http.Headers;
using System.Transactions;

namespace ClientSideLibraryManagementSystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient _httpClient;

        public AuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddAuthorAsync(AuthorsEntity author,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(author.Name), "Name");
            formData.Add(new StringContent(author.Bio), "Bio");
            var response = await _httpClient.PostAsync("https://localhost:7084/api/Authors", formData);

            // Return whether the API request was successful
            return response.IsSuccessStatusCode;
        }

        

        public async Task<IEnumerable<AuthorsEntity>> GetAllAuthorsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7084/api/Authors");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<AuthorsEntity>>();
        }

        public async Task<bool> DeleteAuthorAsync(int authorId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7084/api/Authors/{authorId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<AuthorsEntity> GetAuthorByIdAsync(int authorId,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var author = await _httpClient.GetFromJsonAsync<AuthorsEntity>($"https://localhost:7084/api/Authors/{authorId}");
            if (author == null)
            {
                throw new KeyNotFoundException("User not foound");
            }

            return author;
        }

        public async Task<bool> UpdateAuthorAsync(AuthorsEntity author,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            //formData.Add(new StringContent(author.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(author.Name), "Name");

            formData.Add(new StringContent(author.Bio), "Bio");

            

            var response = await _httpClient.PutAsync($"https://localhost:7084/api/Authors/{author.AuthorId}", formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // _logger.LogError($"Error updating product: {response.StatusCode} - {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
