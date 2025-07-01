using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Data.Helpers;
using SocialApp.Data.Models;
using SocialApp.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext with dependency injection
var connectionString = builder.Configuration.GetConnectionString("NewConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(connectionString));

// Register other services as needed, e.g., for authentication, logging, etc.
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IHashtagsService, HashtagsService>();
builder.Services.AddScoped<IStoriesService, StoriesService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IUsersService, UsersService>();

//Identity configuration (if needed)
builder.Services.AddIdentity<User, IdentityRole<int>>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Seed the database with initial data if needed
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

	await dbContext.Database.MigrateAsync();
	await DbInitializer.SeedAsync(dbContext);

	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
	await DbInitializer.SeedUsersAndRolesAsync(userManager, roleManager);

	if (dbContext.Database.EnsureCreated())
	{
		// Database was created, you can seed initial data here if needed
		// Example: dbContext.Users.Add(new User { Name = "Admin" });
		// dbContext.SaveChanges();
	}
	else
	{
		// Database already exists, you can perform migrations or other operations if needed
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
