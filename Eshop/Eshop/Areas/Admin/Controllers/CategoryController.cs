using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize]
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

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }

            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id  == 0) //Insert
                {
                    _unitOfWork.Category.Add(category);                 
                }
                else // Update
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            //return Json(new { data = _unitOfWork.Category.GetAll() });

            //2nd way using stored procedures 
            return Json(new { data = _unitOfWork.SP_Call.ReturnList<Category>(StatDetails.usp_GetAllCategories) });
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