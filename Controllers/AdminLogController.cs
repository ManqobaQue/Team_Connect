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

        //public IActionResult Index()
        //{
        //    var logs = _context.AdminLogs
        //        .OrderByDescending(l => l.TimeStamp)
        //        .ToList();

        //    return View(logs);
        //}
    }
}
