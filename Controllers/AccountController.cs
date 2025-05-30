using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CompanyPhonebook.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CompanyPhonebook.Controllers
{
    public class AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger) : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                logger.LogInformation("Attempting to log in user: {Email}", model.Email);

                var result = await signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    logger.LogInformation("User {Email} logged in successfully.", model.Email);
                    return LocalRedirect(returnUrl ?? Url.Content("~/"));
                }
            
                if (result.IsLockedOut)
                {
                    logger.LogWarning("User {Email} account locked out.", model.Email);
                    ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
                    return View(model);
                }

                logger.LogWarning("Invalid login attempt for user {Email}.", model.Email);
                ModelState.AddModelError(string.Empty, "Incorrect login Credentials.");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            logger.LogWarning("Model state invalid for user {Email}.", model.Email);
            return View(model);
        } 

        
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}

