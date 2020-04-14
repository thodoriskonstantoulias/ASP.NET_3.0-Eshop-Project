using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.DataAccess.Data.Intitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            //Check to see if there are roles already
            if (_context.Roles.Any(r => r.Name == StatDetails.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(StatDetails.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StatDetails.Manager)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            }, "Admin123!").GetAwaiter().GetResult();

            ApplicationUser user = _context.ApplicationUser.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, StatDetails.Admin).GetAwaiter().GetResult();
        }
    }
}
