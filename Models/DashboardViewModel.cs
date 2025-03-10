using System.Collections.Generic;
using CompanyPhonebook.Models;

namespace CompanyPhonebook.Models
{
    public class DashboardViewModel
    {
        public int TotalDepartments { get; set; }
        public int TotalUsers { get; set; }
        public List<Department> Departments { get; set; } = [];
        public List<User> Users { get; set; } = [];
    }
}
