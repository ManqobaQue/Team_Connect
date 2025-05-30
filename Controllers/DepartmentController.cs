using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using ClosedXML.Excel;

namespace CompanyPhonebook.Controllers
{
   //[Authorize]
    public class DepartmentController(PhonebookContext context) : Controller
    {
        private readonly PhonebookContext _context = context;

        //Search method includes related users count
        public async Task<IActionResult> Index(string searchQuery, int page = 1)
        {
            int pageSize = 6;  // Define how many departments to show per page

            // Gets all departments and includes related users
            var departmentsQuery = _context.Departments.Include(d => d.Users).AsQueryable();

            // Apply search filter if there is a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                departmentsQuery = departmentsQuery.Where(d => d.Name.Contains(searchQuery) || d.PhoneExtension.Contains(searchQuery));
            }

            // Order departments by name
            departmentsQuery = departmentsQuery.OrderBy(d => d.Name);

            // Get the total number of departments (for pagination)
            var totalDepartments = await departmentsQuery.CountAsync();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalDepartments / pageSize);

            // Get the departments for the current page
            var departmentsForCurrentPage = await departmentsQuery
                .Skip((page - 1) * pageSize)  // Skip the previous pages
                .Take(pageSize)  // Take the required number of departments
                .ToListAsync();

            // Create the view model to pass to the view
            var viewModel = new DepartmentViewModel
            {
                Departments = departmentsForCurrentPage,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }




        //create Department controller
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        //Edit Department controller
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }



        //Delete Department controller
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.Include(d => d.Users).FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return NotFound();

            // Check if department has users before allowing deletion
            if (department.Users.Count != 0)
            {
                TempData["Error"] = "Cannot delete department with assigned users.";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.
                Include(d => d.Users).
                FirstOrDefaultAsync(d => d.Id == id);

            // Double-check for users before deletion
            if (department?.Users != null && department.Users.Count != 0)
            {
                TempData["Error"] = "Cannot delete department with assigned users.";
                return RedirectToAction(nameof(Index));
            }

            if (department != null)
            {
                try
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Department deleted successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error deleting department: " + ex.Message;
                }
            }

            return RedirectToAction(nameof(Index));
        }
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        //Export to CSV
        public ActionResult ExportToCSV()
        {
            // Step 1: Fetch the employee data from the database
            var employees = _context.Users.ToList(); // Example, adjust according to your data model

            // Step 2: Prepare the CSV content
            StringBuilder csvContent = new();
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
            var departments = _context.Departments.ToList(); // Example, adjust according to your data model

            // Step 2: Prepare the Excel content
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Departments");
                worksheet.Cell("A1").Value = "Department Name";
                worksheet.Cell("B1").Value = "Extension Number";

                int row = 2;
                foreach (var department in departments)
                {
                    worksheet.Cell("A" + row).Value = department.Name;
                    worksheet.Cell("B" + row).Value = department.PhoneExtension;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DepartmentDirectory.xlsx");
                }
            }
        }
    }
}
