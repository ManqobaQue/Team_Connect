using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CompanyPhonebook.Models;
using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Data;


namespace CompanyPhonebook.Controllers
{

    //[Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PhonebookContext _context;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, PhonebookContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var users = await _context.Users.ToListAsync(); //fetching users from uiser table DB

            var model = new AssignRoleViewModel
            {
                Id = string.Empty, // or set a default value
                Role = string.Empty, // or set a default value
                Users = users, //Passing Users to the view
                Roles = roles

            };
            return View(model);
        }

        // POST: Handle role assignment
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors or check them to understand what's wrong
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = $"Invalid data entered: {string.Join(", ", errors)}";
                model.Users = await _context.Users.ToListAsync();
                model.Roles = await _roleManager.Roles.ToListAsync();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(AssignRole));
            }

            if (await _userManager.IsInRoleAsync(user, model.Role))
            {
                TempData["InfoMessage"] = $"User '{user.UserName}' already has the role '{model.Role}'.";
                return RedirectToAction(nameof(AssignRole));
             }


            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{model.Role}' assigned to user successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Error assigning role '{model.Role}' to user.";
            }

            return RedirectToAction(nameof(AssignRole));
        }
    }
}
    
