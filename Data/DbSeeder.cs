using Microsoft.AspNetCore.Identity;

namespace DiaspoDirect.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "User", "Manager01" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        const string adminEmail = "ceo@flaubertgroup.com";
        const string adminPassword = "Aglagtin111@@@";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, adminPassword);
        }
        else
        {
            // Ensure existing user has email confirmed and correct password
            admin.EmailConfirmed = true;
            await userManager.UpdateAsync(admin);

            var token = await userManager.GeneratePasswordResetTokenAsync(admin);
            await userManager.ResetPasswordAsync(admin, token, adminPassword);
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");

        const string manager01Email = "manager01@flaubertgroup.com";
        const string manager01Password = "Aglagtin1$";

        var manager01 = await userManager.FindByEmailAsync(manager01Email);

        if (manager01 is null)
        {
            manager01 = new ApplicationUser
            {
                UserName = manager01Email,
                Email = manager01Email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(manager01, manager01Password);
        }
        else
        {
            manager01.EmailConfirmed = true;
            await userManager.UpdateAsync(manager01);

            var token = await userManager.GeneratePasswordResetTokenAsync(manager01);
            await userManager.ResetPasswordAsync(manager01, token, manager01Password);
        }

        if (!await userManager.IsInRoleAsync(manager01, "Manager01"))
            await userManager.AddToRoleAsync(manager01, "Manager01");
    }
}
