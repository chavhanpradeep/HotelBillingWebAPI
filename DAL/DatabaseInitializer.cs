using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Core;
using DAL.Core.Interfaces;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                await CreateUserAsync("admin", "tempP@ss123", "Inbuilt Administrator", "lastname", "admin@claritytechnologies.com", "+1 (123) 000-0000", new string[] { adminRoleName });
                await CreateUserAsync("user", "tempP@ss123", "Inbuilt Standard User", "lastname", "user@claritytechnologies.com", "+1 (123) 000-0001", new string[] { userRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }

            // if (!await _context.Products.AnyAsync() && !await _context.Params.AnyAsync())
            // {
            //     _logger.LogInformation("Seeding initial data");

            //     Product prod_1 = new Product
            //     {
            //         Title = "BMW M6",
            //         Description = "Yet another masterpiece from the world's best car manufacturer",

            //     };
            //     var param1 = new Param{Id = 1001};
            //     var param2 = new Param{Id = 1002};
            //     var param3 = new Param{Id = 1003};

            //     //var prodVat1 = new ProductVariant{VariantParam = param1, VariantValue = "xyz", Product = prod_1};
                
            //     Product prod_2 = new Product
            //     {
            //         Title = "Nissan Patrol",
            //         Description = "A true man's choice",
                    
            //     };

            //     _context.Products.Add(prod_1);
            //     _context.Products.Add(prod_2);

            //     await _context.SaveChangesAsync();

            //     _logger.LogInformation("Seeding initial data completed");
            // }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string firstname, string lastname, string email, string phoneNumber, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");


            return applicationUser;
        }
    }
}
