using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {       
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null) // Insert
            {
                return View(frequency);
            }

            frequency = _unitOfWork.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }

            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0) //Insert
                {
                    _unitOfWork.Frequency.Add(frequency);
                }
                else // Update
                {
                    _unitOfWork.Frequency.Update(frequency);
                }
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(frequency);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllFrequencies()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });        
        }

        [HttpDelete]
        public IActionResult DeleteFrequency(int id)
        {
            Frequency freqToDelete = _unitOfWork.Frequency.Get(id); 
            if (freqToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Frequency.Remove(freqToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Category deleted" });
        }
        #endregion
    }
}