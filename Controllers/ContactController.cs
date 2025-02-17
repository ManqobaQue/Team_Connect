using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPhonebook.Controllers
{
    public class ContactController : Controller
    {
        private readonly PhonebookContext _context;

        public ContactController(PhonebookContext context)
        {
            _context = context;
        }

        //List & search contacts
        //Allows to search by Name or Phone Number
        public async Task<IActionResult> Index(string searchQuery)
        {
            var contacts = _context.Contacts.Include(c => c.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                contacts = contacts.Where(c => c.Name.Contains(searchQuery) || c.PhoneNumber.Contains(searchQuery));
            }

            return View(await contacts.ToListAsync());
        }

        //Create a new contact[GET]
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _context.Users.ToListAsync(), "Id", "FirstName");
            return View();
        }

        

        // Create Contact (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving contact: " + ex.Message);
                }
            }

            ViewData["UserId"] = new SelectList(await _context.Users.ToListAsync(), "Id", "FirstName", contact.UserId);
            return View(contact);
        }

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // First, verify that we received a valid ID
            if (id == null) return NotFound();

            // Fetch the contact, including the associated User information
            var contact = await _context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return NotFound();

            // Prepare the Users dropdown list for selection
            // We're using FirstName here, but you might want to show both first and last names
            ViewData["UserId"] = new SelectList(
                await _context.Users.ToListAsync(),
                "Id",
                "FirstName",
                contact.UserId);

            return View(contact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            // Ensure the ID in the URL matches the contact being edited
            if (id != contact.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if the contact still exists before throwing the error
                    if (!ContactExists(contact.Id))
                        return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    // Handle any other unexpected errors
                    ModelState.AddModelError("", "Error updating contact: " + ex.Message);
                }
            }

            // If we get here, something failed - redisplay the form
            ViewData["UserId"] = new SelectList(
                await _context.Users.ToListAsync(),
                "Id",
                "FirstName",
                contact.UserId);
            return View(contact);
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Include User information for the delete confirmation view
            var contact = await _context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return NotFound();

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                try
                {
                    _context.Contacts.Remove(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any deletion errors, such as foreign key constraints
                    ModelState.AddModelError("", "Error deleting contact: " + ex.Message);
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Fetch the contact with associated User information
            var contact = await _context.Contacts
                .Include(c => c.User)
                    .ThenInclude(u => u.Department) // Include Department information as well
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return NotFound();

            return View(contact);
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
