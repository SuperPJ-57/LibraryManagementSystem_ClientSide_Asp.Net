namespace ClientSideLibraryManagementSystem.Models
{
    public class AddTransactionModel
    {
        

        public int StudentId { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }
        public int BarCode { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        
    }
}
