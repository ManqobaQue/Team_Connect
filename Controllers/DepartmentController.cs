using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;

namespace CompanyPhonebook.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly PhonebookContext _context;

        public DepartmentController(PhonebookContext context)
        {
            _context = context;
        }

        //Search method includes related users count
        public async Task<IActionResult> Index(string searchQuery)
        {
            //Gets all departments and includes related users for counting
            var departments = _context.Departments.Include(d => d.Users).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                departments = departments.Where(d => d.Name.Contains(searchQuery) || d.PhoneExtension.Contains(searchQuery));
            }

            // Order departments by name for consistent display
            departments = departments.OrderBy(d => d.Name);

            return View(await departments.ToListAsync());
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
            if (department.Users.Any())
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
            if (department?.Users != null && department.Users.Any())
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
    }
}
