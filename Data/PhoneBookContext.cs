using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CompanyPhonebook.Data
{
    public class PhonebookContext(DbContextOptions<PhonebookContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
