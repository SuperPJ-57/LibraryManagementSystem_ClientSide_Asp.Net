namespace ClientSideLibraryManagementSystem.Models
{
    public class BookInstanceViewModel
    {
        public BookInstanceEntity? Book { get; set; } 
        //public IEnumerable<BookInstanceEntity>? Books { get; set; }
        public IEnumerable<BookInstanceDetails>? Books { get; set; }
        public BookInstanceDetails? BookInstanceDetail { get; set; }
    }
}
