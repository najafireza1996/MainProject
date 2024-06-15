using Common.Constants;
using Features.Core.Infrastructure.DataAccess;
using Features.User.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.DataAccess
{
    public static class AppDbInitializer
    {
        public static async Task SeedDataAsync(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                await SeedUsersAndRolesAsync(serviceScope);
                await SeedDataAsync(serviceScope);
            }
        }


        public static async Task SeedDataAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            context.Database.EnsureCreated();


        }

        public static async Task SeedUsersAndRolesAsync(IServiceScope serviceScope)
        {
            //Roles
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            var dbcontext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            #region UsersSeed
            if (!dbcontext.Users.Any())
            {
                var user1 =
                    new ApplicationUser
                    {
                        FirstName = "user",
                        LastName = "user",
                        Email = "user@gmail.com",
                        UserName = "user123",

                    };

                var user2 =
                    new ApplicationUser
                    {
                        FirstName = "reza",
                        LastName = "najafi",
                        Email = "reza@gmail.com",
                        UserName = "reza123",

                    };

                var Admin =
                    new ApplicationUser
                    {
                        FirstName = "admin",
                        LastName = "ln",
                        Email = "admin@gmail.com",
                        UserName = "admin123",

                    };


                await userManager.CreateAsync(user1, "Passwd@123");
                await userManager.AddToRoleAsync(user1, Roles.User);

                await userManager.CreateAsync(user2, "Passwd@123");
                await userManager.AddToRoleAsync(user2, Roles.User);

                await userManager.CreateAsync(Admin, "Passwd@123");
                await userManager.AddToRoleAsync(Admin, Roles.User);
                await userManager.AddToRoleAsync(Admin, Roles.Admin);

                #endregion

            }
        }
    }
}