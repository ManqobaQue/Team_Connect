// Models/AssignRoleViewModel.cs
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CompanyPhonebook.Models
{
    public class AssignRoleViewModel
    {
        public required string UserId { get; set; } = string.Empty;
        public required string Role { get; set; } = string.Empty;
        public required List<IdentityRole> Roles { get; set; } = [];
        public required List<User> Users { get; set; } = []; //To store users

      
        // Add FullName property to make the display cleaner
        // public string FullName => $"{FirstName} {LastNameLastName}"; // Assuming you have FirstName and LastName in your model
    }
}

