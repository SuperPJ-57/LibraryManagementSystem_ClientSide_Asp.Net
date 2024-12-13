using ClientSideLibraryManagementSystem.Models;

namespace Lms.Domain.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentsEntity>> GetAllStudentsAsync(string token);
        Task<StudentsEntity> GetStudentByIdAsync(int studentId,string token);
        Task<bool> AddStudentAsync(StudentsEntity student,string token);
        Task<bool> UpdateStudentAsync(StudentsEntity student,string token);
        Task<bool> DeleteStudentAsync(int studentId,string token);
    }
}
