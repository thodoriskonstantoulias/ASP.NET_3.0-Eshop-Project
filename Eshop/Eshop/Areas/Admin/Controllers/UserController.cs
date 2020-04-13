using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize(Roles = StatDetails.Admin)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //Find all users except the one logged in 
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(_unitOfWork.User.GetAll(u => u.Id != claims.Value));
        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _unitOfWork.User.LockUser(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _unitOfWork.User.UnLockUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}