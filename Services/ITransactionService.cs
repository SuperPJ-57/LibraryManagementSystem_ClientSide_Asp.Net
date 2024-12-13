using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDetails>> GetAllTransactionsAsync(string token);
        Task<TransactionDetails> GetTransactionByIdAsync(int transactionId);
        Task<bool> AddTransactionAsync(TransactionsEntity transaction,string token);
        Task<TransactionsEntity> UpdateTransactionAsync(TransactionsEntity transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}
