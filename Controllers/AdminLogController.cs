using CompanyPhonebook.Data;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPhonebook.Controllers
{
    public class AdminLogController : Controller
    {
        private readonly PhonebookContext _context;

        public AdminLogController(PhonebookContext context)
        {
            _context = context;
        }


        // display admin logs - need to check on adminlogs data -  2025/10/04
        public IActionResult Index()
        {
            var logs = _context.AdminLogs
                .OrderByDescending(l => l.TimeStamp)
                .ToList();

            return View(logs);
        }
    }
}
