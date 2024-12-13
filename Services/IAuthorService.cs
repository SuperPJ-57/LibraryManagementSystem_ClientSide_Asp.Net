

using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorsEntity>> GetAllAuthorsAsync(string token);
        Task<AuthorsEntity> GetAuthorByIdAsync(int authorId,string token);
        Task<bool> AddAuthorAsync(AuthorsEntity author,string token);
        Task<bool> UpdateAuthorAsync(AuthorsEntity author,string token);
        Task<bool> DeleteAuthorAsync(int authorId,string token);
    }
}
