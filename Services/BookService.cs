using ClientSideLibraryManagementSystem.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ClientSideLibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<BooksEntity>> GetAllBooksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7084/api/Books");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<BooksEntity>>();
        }
        public async Task<bool> AddBookAsync(BooksEntity book,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(book.Title), "Title");
            formData.Add(new StringContent(book.Genre), "Genre");
            formData.Add(new StringContent(book.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(book.ISBN), "ISBN");
            var response = await _httpClient.PostAsync("https://localhost:7084/api/Books", formData);

            // Return whether the API request was successful
            return response.IsSuccessStatusCode;
        }

        public async Task<BooksEntity> GetBookByIdAsync(int bookId,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var book = await _httpClient.GetFromJsonAsync<BooksEntity>($"https://localhost:7084/api/Books/{bookId}");
            if (book == null)
            {
                throw new KeyNotFoundException("User not foound");
            }

            return book;
        }


        public async Task<bool> DeleteBookAsync(int bookId,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7084/api/Books/{bookId}");
            return response.IsSuccessStatusCode;
        }


        
        public async Task<bool> UpdateBookAsync(BooksEntity book,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            //formData.Add(new StringContent(author.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(book.Title), "Title");
            formData.Add(new StringContent(book.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(book.Genre), "Genre");
            formData.Add(new StringContent(book.ISBN), "ISBN");
            //formData.Add(new StringContent(book.Quantity.ToString()), "Quantity");


            var response = await _httpClient.PutAsync($"https://localhost:7084/api/Books/{book.BookId}", formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // _logger.LogError($"Error updating product: {response.StatusCode} - {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
