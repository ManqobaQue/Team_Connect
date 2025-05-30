using Microsoft.AspNetCore.Identity;

namespace CompanyPhonebook.Models;

public class ApplicationUser:IdentityUser
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //User status / date

}
