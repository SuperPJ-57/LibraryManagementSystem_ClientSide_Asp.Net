using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDetails>?> GetAllTransactionsAsync(string token);
        Task<TransactionDetails> GetTransactionByIdAsync(int transactionId,string token);
        Task<bool> AddTransactionAsync(TransactionsEntity transaction,string token);
        Task<bool> UpdateTransactionAsync(TransactionsEntity transaction,string token);
        Task<bool> DeleteTransactionAsync(int transactionId, string token);
    }
}
