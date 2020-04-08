using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository;
using Eshop.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 

        public IActionResult Index()
        {
            
            return View();
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Json(new { data = _unitOfWork.Category.GetAll() });
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Category deleted" });
        }

        #endregion
    }
}