namespace ClientSideLibraryManagementSystem.Models
{
    public class BooksEntity
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
    }
}
