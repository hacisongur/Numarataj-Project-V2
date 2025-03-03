using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Numarataj.Business.Mapping;
using Numarataj.DataAccess.Context;
using Numarataj.Entity.Entities;
using Numarataj.WebUI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

// Add DbContext with SQL Server configuration
builder.Services.AddDbContext<NumaratajDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

// Add Identity services for authentication and authorization
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<NumaratajDbContext>()
    .AddDefaultTokenProviders();

// Register the NoCacheHeaderFilter globally
builder.Services.AddScoped<NoCacheHeaderFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<NoCacheHeaderFilter>();
});

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(45);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

// Configure Application Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(90);
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    name: "logout",
    pattern: "Account/Logout",
    defaults: new { controller = "Account", action = "Logout" }
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");



// Seed the database with initial identity users
IdentitySeedData.IdentityTestUser(app);

app.Run();
