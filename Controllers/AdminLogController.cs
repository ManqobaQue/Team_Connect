using CompanyPhonebook.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

        //Export logs to CSV/Excel
        // Need to install the package "using System.Text;"
        // Install-Package CsvHelper
        // dotnet add package CsvHelper
        // Uodated Manqoba 2025/10/07
        public IActionResult ExportLogs() 
        {
            var logs = _context.AdminLogs.OrderByDescending(l => l.TimeStamp).ToList();
            // Logic to export logs to CSV/Excel would go here

            var csv = new StringBuilder();

            foreach (var log in logs)
            {
                csv.AppendLine($"{log.Id},{log.ActionName},{log.Id},{log.TimeStamp}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", "Adminlogs.csv");
        }
    }
}
