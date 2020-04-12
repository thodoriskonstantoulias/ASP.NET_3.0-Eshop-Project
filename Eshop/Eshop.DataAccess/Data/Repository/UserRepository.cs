using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void LockUser(string userId)
        {
            var userFromDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _context.SaveChanges();
        }

        public void UnLockUser(string userId)
        {
            var userFromDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
