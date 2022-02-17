//!--Author xkrukh00-- >
using Microsoft.AspNetCore.Identity;

public static class ApplicationDbInitializer
{
    public static void SeedUsersRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (roleManager.FindByNameAsync("Administrator").Result == null)
        {
            var AdministratorRole = new IdentityRole()
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            };
            roleManager.CreateAsync(AdministratorRole).Wait();
        }

        if (roleManager.FindByNameAsync("ShelterAdministrator").Result == null)
        {
            var ShelterAdministratorRole = new IdentityRole()
            {
                Name = "ShelterAdministrator",
                NormalizedName = "SHELTERADMINISTRATOR"
            };
            roleManager.CreateAsync(ShelterAdministratorRole).Wait();
        }

        if (roleManager.FindByNameAsync("Volunteer").Result == null)
        {
            var VolunteerRole = new IdentityRole()
            {
                Name = "Volunteer",
                NormalizedName = "VOLUNTEER"
            };
            roleManager.CreateAsync(VolunteerRole).Wait();
        }



        if (userManager.FindByEmailAsync("admin@mail.com").Result == null)
        {
            var Admin = new IdentityUser()
            {
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN"
            };

            var result = userManager.CreateAsync(Admin, "Admin123.").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(Admin, "Administrator").Wait();
            }
        }

        if (userManager.FindByEmailAsync("shelteradmin@mail.com").Result == null)
        {
            var ShelterAdmin = new IdentityUser()
            {
                Email = "shelteradmin@mail.com",
                NormalizedEmail = "SHELTERADMIN@MAIL.COM",
                UserName = "Shelteradmin",
                NormalizedUserName = "SHELTERADMIN"
            };

            IdentityResult result = userManager.CreateAsync(ShelterAdmin, "Shelteradmin123.").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(ShelterAdmin, "ShelterAdministrator").Wait();
            }
        }
    }
}