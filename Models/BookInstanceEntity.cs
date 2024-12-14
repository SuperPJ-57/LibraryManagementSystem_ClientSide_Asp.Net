namespace ClientSideLibraryManagementSystem.Models
{
    public class BookInstanceEntity
    {
        public int BookId { get; set; }
        public int BarCode { get; set; }
        public bool IsAvailable { get; set; }
    }
}
