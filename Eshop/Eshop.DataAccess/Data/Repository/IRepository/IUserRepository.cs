using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void LockUser(string userId);
        void UnLockUser(string userId);
    }
}
