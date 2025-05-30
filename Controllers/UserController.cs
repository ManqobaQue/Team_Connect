using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using ClosedXML.Excel;

namespace CompanyPhonebook.Controllers
{
    //[Authorize]
    public class UserController(PhonebookContext context) : Controller
    {
        private readonly PhonebookContext _context = context;

        //List & search all Users
        //Allow searching by FirstName, LastName, or Email.
        public async Task<IActionResult> Index(string searchQuery, int page = 1)
        {
            int pageSize = 6; // Number of items per page

            // Fetch all users and include related departments for displaying department names
            var usersQuery = _context.Users.Include(u => u.Department).AsQueryable();

            // Apply search filter if there is a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(searchQuery) ||
                                                    u.LastName.Contains(searchQuery) ||
                                                    u.Email.Contains(searchQuery));
            }

            // Order users by last name for consistency
            usersQuery = usersQuery.OrderBy(u => u.LastName);

            // Get total number of users after applying search (needed for pagination)
            var totalUsers = await usersQuery.CountAsync();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            // Fetch the users for the current page
            var usersForCurrentPage = await usersQuery
                .Skip((page - 1) * pageSize)  // Skip previous pages
                .Take(pageSize)  // Take the required number of users
                .ToListAsync();

            // Prepare the ViewModel with pagination data
            var viewModel = new UserViewModel
            {
                Users = usersForCurrentPage,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }


        //Create User controller
        public IActionResult Create()
        {
           ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            //rEMOVAL OF EXCLAMATION MARK ALLOWS APPLICATION TO CREATE USERS WITHOUT VALIDATION.
            if (!ModelState.IsValid)
            {

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            return View(user);
        }

               


            //Edit User controller
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.
                Include(u => u.Department).
                FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            //Preparing the department dropdown list
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    throw;
                }
                //user update error
                catch (Exception ex)
                {
                    // Add user-friendly error handling
                    ModelState.AddModelError("", $"Error updating user: {ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }
            //Preparing the department dropdown list
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);

            return View(user);
        }

        //Delete user controller
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            //Include the department information
            var user = await _context.Users.
                Include(u => u.Department).
                FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        //Details Method for User
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Department)       
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        //Delete User controller
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //Export to CSV
        public ActionResult ExportToCSV()
        {
            // Step 1: Fetch the employee data from the database
            var employees = _context.Users.ToList(); // Example, adjust according to your data model

            // Step 2: Prepare the CSV content
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("FirstName,LastName,Email,Extension");

            foreach (var employee in employees)
            {
                csvContent.AppendLine($"{employee.FirstName},{employee.LastName},{employee.Email},{employee.ExtensionNumber}");
            }

            // Step 3: Return the CSV file for download
            byte[] fileBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            return File(fileBytes, "text/csv", "EmployeeDirectory.csv");
        }

        //Export to Excel
        public ActionResult ExportToExcel()
        {
            // Step 1: Fetch the employee data from the database
            var employees = _context.Users.ToList(); // Example, adjust according to your data model
            // Step 2: Prepare the Excel content
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                worksheet.Cell("A1").Value = "FirstName";
                worksheet.Cell("B1").Value = "LastName";
                worksheet.Cell("C1").Value = "Extension Number";
                //worksheet.Cell("D1").Value = "Email";
                int row = 2;
                foreach (var employee in employees)
                {
                    worksheet.Cell("A" + row).Value = employee.FirstName;
                    worksheet.Cell("B" + row).Value = employee.LastName;
                    worksheet.Cell("C" + row).Value = employee.ExtensionNumber;
                    //worksheet.Cell("D" + row).Value = employee.Email;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeDirectory.xlsx");
                }
            }
        }
    }
}
