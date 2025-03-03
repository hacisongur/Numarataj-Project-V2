using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Numarataj.DataAccess.Context;
using Numarataj.Entity.Entities;

public class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "Admin_123";

    public static async Task IdentityTestUser(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<NumaratajDbContext>();
        if (context.Database.GetAppliedMigrations().Any())
        {
            await context.Database.MigrateAsync(); // Migrate işlemini asenkron yap
        }
        var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new AppUser
            {
                FullName = "Haci Songur",
                UserName = adminUser,
                Email = "haci.songur@bel.tr",
                PhoneNumber = "4444444"
            };
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}
