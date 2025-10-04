using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using CompanyPhonebook.Filters;


var builder = WebApplication.CreateBuilder(args);

// A. Configure Entity Framework and the database context
builder.Services.AddDbContext<PhonebookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqloptins => sqloptins.EnableRetryOnFailure()

    )
);


// B. Configure Identity (users, roles, password rules, etc.)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
     .AddRoles<IdentityRole>()  // Enable role support
    .AddEntityFrameworkStores<PhonebookContext>(); //Use EF Core to store Identity data


// C. Add MVC controller and Razor Pages support
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AdminLoggingFilter>(); // ⬅️ Register the global filter
});
builder.Services.AddRazorPages(); // For Razor Pages


// D. Configure authentication cookie behavior
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Where to redirect when login is required
    options.AccessDeniedPath = "/Account/AccessDenied";// Where to redirect when access is denied
});

var app = builder.Build();


// E. Seed roles and admin user at application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<SeedData>>(); // Get logger
    try
    {
        await SeedData.Initialize(services, logger); // Pass logger to seed method
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// F. Configure error handling and HTTP settings
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");// Use custom error page
    app.UseHsts();
}


// G. Enable middleware components
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// H. Configure endpoints/routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route

app.MapRazorPages(); // Enable Razor Pages

// I. Run the application
app.Run(); // End of the application setup
