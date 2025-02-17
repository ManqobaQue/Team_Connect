using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Models;
using System.Threading.Tasks;
using CompanyPhonebook.Data;

namespace CompanyPhonebook.Controllers
{
    public class DashboardController : Controller
    {
        private readonly PhonebookContext _context;

        public DashboardController(PhonebookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Users)
                .ToListAsync();

            var users = await _context.Users
                .Include(u => u.Department)
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalDepartments = departments.Count,
                TotalUsers = users.Count,
                Departments = departments,
                Users = users
            };

            return View(viewModel);
        }
    }
}
