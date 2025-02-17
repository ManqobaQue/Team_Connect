using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Models;

namespace CompanyPhonebook.Data
{
    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }

    }

}
