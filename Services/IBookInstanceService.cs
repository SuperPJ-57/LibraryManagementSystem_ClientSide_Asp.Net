using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public interface IBookInstanceService
    {
        Task<bool> AddBookInstanceAsync(BookInstanceEntity bookInstance,string token);
        Task<IEnumerable<BookInstanceDetails>?> GetAllBookInstancesAsync(string token);
        Task<BookInstanceDetails> GetBookInstanceByBarcodeAsync(int barcode, string token);
        Task<bool> UpdateBookInstanceAsync(BookInstanceEntity bookInstance, string token);
        Task<bool> DeleteBookInstanceAsync(int barcode, string token);
    }
}
