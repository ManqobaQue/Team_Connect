using Microsoft.AspNetCore.Mvc.RazorPages;
using CompanyPhonebook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompanyPhonebook.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly PhonebookContext _context;

        public IndexModel(PhonebookContext context)
        {
            _context = context;
        }

        public DashboardViewModel DashboardData { get; set; }

        public async Task OnGetAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Users)
                .ToListAsync();

            var users = await _context.Users
                .Include(u => u.Department)
                .ToListAsync();

            DashboardData = new DashboardViewModel
            {
                TotalDepartments = departments.Count,
                TotalUsers = users.Count,
                Departments = departments,
                Users = users
            };
        }
    }
}
