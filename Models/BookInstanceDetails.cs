namespace ClientSideLibraryManagementSystem.Models
{
    public class BookInstanceDetails
    {
        public int BarCode { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public bool IsAvailable { get; set; }
    }
}
