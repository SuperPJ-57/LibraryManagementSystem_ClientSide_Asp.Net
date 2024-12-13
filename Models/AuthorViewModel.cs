namespace ClientSideLibraryManagementSystem.Models
{
    public class AuthorViewModel
    {
        public AuthorsEntity Author { get; set; } = null;
        public AddAuthorModel AddAuthorModel { get; set; } = null;
        public IEnumerable<AuthorsEntity> Authors { get; set; } = null;
    }
}
