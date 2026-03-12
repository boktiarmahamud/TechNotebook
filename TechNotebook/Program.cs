using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechNotebook.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Post/AccessDenied";
	options.LoginPath = "/Auth/Login";
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	options.SlidingExpiration = true;
});

var app = builder.Build();


// Seed Admin Role and User
await SeedRolesAndAdminAsync(app);


// Configure middleware
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Post}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


// Seeding Method
static async Task SeedRolesAndAdminAsync(WebApplication app)
{
	using var scope = app.Services.CreateScope();

	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	string adminEmail = "admin@gmail.com";
	string adminPassword = "admin";

	// Create Admin role if it doesn't exist
	if (!await roleManager.RoleExistsAsync("Admin"))
	{
		await roleManager.CreateAsync(new IdentityRole("Admin"));
	}

	// Create Admin user if it doesn't exist
	var user = await userManager.FindByEmailAsync(adminEmail);

	if (user == null)
	{
		user = new IdentityUser
		{
			UserName = adminEmail,
			Email = adminEmail
		};

		await userManager.CreateAsync(user, adminPassword);
		await userManager.AddToRoleAsync(user, "Admin");
	}
}