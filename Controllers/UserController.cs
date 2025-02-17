using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CompanyPhonebook.Controllers
{
    public class UserController : Controller
    {
        private readonly PhonebookContext _context;

        public UserController(PhonebookContext context)
        {
            _context = context;
        }

        //List & search all Users
        //Allow searching by FirstName, LastName, or Email.
        public async Task<IActionResult> Index(String searchQuery)
        {
            var users = _context.Users.Include(u => u.Department).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                users = users.Where(u => u.FirstName.Contains(searchQuery) ||
                                         u.LastName.Contains(searchQuery) ||
                                         u.Email.Contains(searchQuery));
            }

            return View(await users.ToListAsync());
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
                    ModelState.AddModelError("", "Error updating user: " + ex.Message);
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
                .Include(u => u.Contacts)  // Include related contacts
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

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
    }
}
