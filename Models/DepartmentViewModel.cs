namespace CompanyPhonebook.Models
{
    public class DepartmentViewModel
    {
        public IEnumerable<Department> Departments { get; set; } = new List<Department>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchQuery { get; set; }  // For handling search query in pagination
    }
}

