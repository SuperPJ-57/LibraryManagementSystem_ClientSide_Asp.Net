using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BooksEntity>> GetAllBooksAsync(string token,string? query=null);
        Task<BooksEntity> GetBookByIdAsync(int bookId,string token);
        Task<bool> AddBookAsync(BooksEntity book,string token);
        Task<bool> UpdateBookAsync(BooksEntity book,string token);
        Task<bool> DeleteBookAsync(int bookId,string token);
    }
}
