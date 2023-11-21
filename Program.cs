using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheQuestion.Data;
using TheQuestion.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/login";
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { action = "Index" });

app.Run();
