using ClientSideLibraryManagementSystem.Models;
using System.Net.Http.Headers;

namespace ClientSideLibraryManagementSystem.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<bool> AddTransactionAsync(TransactionsEntity transaction,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(transaction.StudentId.ToString()), "StudentId");
            formData.Add(new StringContent(transaction.UserId.ToString()), "UserId");
            formData.Add(new StringContent(transaction.BookId.ToString()), "BookId");
            formData.Add(new StringContent(transaction.BarCode.ToString()), "BarCode");
            formData.Add(new StringContent(transaction.TransactionType), "TransactionType");
            formData.Add(new StringContent(transaction.Date.ToString()), "Date");
            var response = await _httpClient.PostAsync("https://localhost:7084/api/Transactions", formData);

            // Return whether the API request was successful
            return response.IsSuccessStatusCode;
        }

        public Task<bool> DeleteTransactionAsync(int transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TransactionDetails>> GetAllTransactionsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7084/api/Transactions");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<TransactionDetails>>();
        }

        public Task<TransactionDetails> GetTransactionByIdAsync(int transactionId)
        {
           
            var result = _httpClient.GetFromJsonAsync<TransactionDetails>($"https://localhost:7084/api/Transactions/{transactionId}");
            if(result == null)
            {
                throw new KeyNotFoundException("Transaction not foound");
            }
            return result;
        }

        public Task<TransactionsEntity> UpdateTransactionAsync(TransactionsEntity transaction)
        {
            throw new NotImplementedException();
        }
    }
}
