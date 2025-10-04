using Microsoft.AspNetCore.Identity;

namespace CompanyPhonebook.Models;

public class ApplicationUser : IdentityUser
{
    public override string? UserName { get; set; } = string.Empty;
    public override string? Email { get; set; } = string.Empty;
}
