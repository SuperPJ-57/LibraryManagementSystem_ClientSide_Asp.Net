namespace ClientSideLibraryManagementSystem.Models
{
    public class TransactionViewModel
    {
        public TransactionsEntity? Transaction { get; set; }
        public IEnumerable<TransactionsEntity>? Transactions { get; set; }
        public IEnumerable<TransactionDetails>? transactionDetails { get; set; }
        public BorrowData? BorrowData { get; set; }
    }
}
