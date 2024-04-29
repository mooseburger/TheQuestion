using Algolia.Search.Clients;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TheQuestion.CAPTCHA;
using TheQuestion.Data;
using TheQuestion.Repositories;
using TheQuestion.Search;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

// CAPTCHA
builder.Services.Configure<CaptchaConfiguration>(builder.Configuration.GetSection("CAPTCHA"));
builder.Services.AddScoped<ICaptchaService, CaptchaService>();

// Search
builder.Services.Configure<SearchConfiguration>(builder.Configuration.GetSection("Search"));
builder.Services.AddSingleton<ISearchClient, SearchClient>(provider =>
{
    var config = provider.GetRequiredService<IOptions<SearchConfiguration>>().Value;
    return new SearchClient(config.AppId, config.AdminApiKey);
});

builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
    options.AccessDeniedPath = "/auth/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});



var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

app.MapControllerRoute(
    name: "answer",
    pattern: "answer/{id}",
    defaults: new { controller = "Answer", action = "Index" });

app.Run();
