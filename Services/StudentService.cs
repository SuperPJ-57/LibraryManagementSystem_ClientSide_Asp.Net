using ClientSideLibraryManagementSystem.Models;
using Lms.Domain.Interfaces;
using System.Net.Http.Headers;

namespace ClientSideLibraryManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;
        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddStudentAsync(StudentsEntity student, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(student.Name), "Name");
            formData.Add(new StringContent(student.Email), "Email");
            formData.Add(new StringContent(student.ContactNumber), "ContactNumber");
            formData.Add(new StringContent(student.Department), "Department");

            var response = await _httpClient.PostAsync("https://localhost:7084/api/Students", formData);

            return response.IsSuccessStatusCode;
        }

       
       
        public async Task<IEnumerable<StudentsEntity>> GetAllStudentsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<StudentsEntity>>("https://localhost:7084/api/Students");

            return response;
        }



        public async Task<bool> DeleteStudentAsync(int studentId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7084/api/Students/{studentId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<StudentsEntity> GetStudentByIdAsync(int studentId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var student = await _httpClient.GetFromJsonAsync<StudentsEntity>($"https://localhost:7084/api/Students/{studentId}");
            if (student == null)
            {
                throw new KeyNotFoundException("User not foound");
            }

            return student;
        }

        public async Task<bool> UpdateStudentAsync(StudentsEntity student, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            //formData.Add(new StringContent(student.StudentId.ToString()), "StudentId");
            formData.Add(new StringContent(student.StudentId.ToString()), "StudentId");

            formData.Add(new StringContent(student.Name), "Name");
            formData.Add(new StringContent(student.Email), "Email");
            formData.Add(new StringContent(student.ContactNumber), "ContactNumber");
            formData.Add(new StringContent(student.Department), "Department");



            var response = await _httpClient.PutAsync($"https://localhost:7084/api/Students/{student.StudentId}", formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // _logger.LogError($"Error updating product: {response.StatusCode} - {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
