using ClientSideLibraryManagementSystem.Models;
using System.Net.Http.Headers;

namespace ClientSideLibraryManagementSystem.Services
{
    public class BookInstanceService : IBookInstanceService
    {
        private readonly HttpClient _httpClient;

        public BookInstanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddBookInstanceAsync(BookInstanceEntity bookInstance, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(bookInstance.BookId.ToString()), "Bookid");
            formData.Add(new StringContent(bookInstance.BarCode.ToString()), "BarCode");
            var response = await _httpClient.PostAsync("https://localhost:7084/api/BookCopies", formData);

            // Return whether the API request was successful
            return response.IsSuccessStatusCode;
        }



        public async Task<IEnumerable<BookInstanceDetails>?> GetAllBookInstancesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BookInstanceDetails>>("https://localhost:7084/api/BookCopies");

            return response;
        }

        public async Task<bool> DeleteBookInstanceAsync(int barcode, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7084/api/BookCopies/{barcode}");
            return response.IsSuccessStatusCode;
        }

        public async Task<BookInstanceDetails?> GetBookInstanceByBarcodeAsync(int barcode, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var bookInstance = await _httpClient.GetFromJsonAsync<BookInstanceDetails>($"https://localhost:7084/api/BookCopies/{barcode}");
            if (bookInstance == null)
            {
                throw new KeyNotFoundException("User not foound");
            }

            return bookInstance;
        }

        public async Task<bool> UpdateBookInstanceAsync(BookInstanceEntity bookInstance, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            //formData.Add(new StringContent(bookInstance.barcode.ToString()), "barcode");
            formData.Add(new StringContent(bookInstance.BookId.ToString()), "BookId");

            formData.Add(new StringContent(bookInstance.IsAvailable.ToString()), "IsAvailable");



            var response = await _httpClient.PutAsync($"https://localhost:7084/api/BookCopies/{bookInstance.BarCode}", formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // _logger.LogError($"Error updating product: {response.StatusCode} - {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
