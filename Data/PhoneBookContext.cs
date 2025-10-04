using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CompanyPhonebook.Data
{
   public class PhonebookContext : IdentityDbContext<ApplicationUser>
{
    public PhonebookContext(DbContextOptions<PhonebookContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    

          protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);



            // Seed Departments

            modelBuilder.Entity<Department>().HasData(

                new Department { Id = 1, Name = "Human Resources", PhoneExtension = "10001" },

                new Department { Id = 2, Name = "IT Department", PhoneExtension = "10002" },

                new Department { Id = 3, Name = "Finance", PhoneExtension = "10003" }

            );



        // Seed Phonebook Users

       modelBuilder.Entity<User>().HasData(

               new User

               {

                   Id = 1,

                   FirstName = "Alice",

                   LastName = "Smith",

                   Email = "alice.smith@company.com",

                   ExtensionNumber = "20001",

                   DepartmentId = 1

               },

               new User

               {

                   Id = 2,

                   FirstName = "Bob",

                   LastName = "Johnson",

                   Email = "bob.johnson@company.com",

                   ExtensionNumber = "20002",

                   DepartmentId = 2

               }

           );

        }

    }
}

