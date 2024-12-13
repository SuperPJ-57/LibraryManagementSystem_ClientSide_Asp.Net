using ClientSideLibraryManagementSystem.Services;

namespace ClientSideLibraryManagementSystem.Models
{
    public class ReturnBookModel
    {
        private readonly ITransactionService _transactionService;
        public ReturnBookModel(ITransactionService transaction)
        {
            _transactionService = transaction;
        }
        private int studentId;
        public int TransactionId { get; set; }
        public int StudentId { get { return studentId; } set {
              var result = _transactionService.GetTransactionByIdAsync(TransactionId);
            studentId = result.Result.StudentId;
        } }

        public int UserId { get; set; }

        public int BookId { get; set; }
        public int BarCode { get; set; }
        public string TransactionType { get; set; } = "Return";
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
