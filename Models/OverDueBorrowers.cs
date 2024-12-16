namespace ClientSideLibraryManagementSystem.Models
{
    public class OverDueBorrowers
    {
        public int BorrowerId { get; set; }
        public int BorrowId { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerEmail { get; set; }
        public string DueDate { get; set; }
        public string BookTitle { get; set; }
    }
}
