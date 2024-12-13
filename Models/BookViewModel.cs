namespace ClientSideLibraryManagementSystem.Models
{
    public class BookViewModel
    {
        public BooksEntity Book { get; set; } = null;
        public IEnumerable<BooksEntity> Books { get; set; } = null;
        
        public AddBookModel AddBookModel { get; set; } = null;
    }
}
