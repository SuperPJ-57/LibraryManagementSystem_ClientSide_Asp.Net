namespace ClientSideLibraryManagementSystem.Models
{
    public class StudentsEntity
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; } = null;

        public string Department { get; set; } = null;
    }
}
