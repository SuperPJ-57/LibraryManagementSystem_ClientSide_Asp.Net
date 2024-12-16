using ClientSideLibraryManagementSystem.Models;

namespace ClientSideLibraryManagementSystem.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardData> GetDashboardDataAsync(string username)
        {
            var response = await _httpClient.GetFromJsonAsync<DashboardData>($"https://localhost:7084/api/Dashboard/{username}");
            if(response == null)
            {
                throw new Exception("Failed to get dashboard data");
            }

            return response;

        }

        public async Task<IEnumerable<OverDueBorrowers>> GetOverdueBorrowersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OverDueBorrowers>>($"https://localhost:7084/api/Dashboard/");
            if (response == null)
            {
                throw new Exception("Failed to get dashboard data");
            }

            return response;

        }
    }
}
