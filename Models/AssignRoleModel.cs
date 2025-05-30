using Microsoft.AspNetCore.Identity;

namespace CompanyPhonebook.Models
{
    public class AssignRoleViewModel
    {
        public required string Id { get; set; } = string.Empty;
        public required string Role { get; set; } = string.Empty;
        public required List<IdentityRole> Roles { get; set; } = [];
        public required List<User> Users { get; set; } = []; //To store users

    }
}

