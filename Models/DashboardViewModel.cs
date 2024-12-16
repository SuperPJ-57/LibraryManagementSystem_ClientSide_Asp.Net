namespace ClientSideLibraryManagementSystem.Models
{
    public class DashboardViewModel
    {
        public DashboardData? DashboardData { get; set; }
        public IEnumerable<OverDueBorrowers>? OverDueBorrowers { get; set; }
    }
}
