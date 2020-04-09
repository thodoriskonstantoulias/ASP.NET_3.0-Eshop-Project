using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
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


        #region API CALLS
        [HttpGet]
        public IActionResult GetAllFrequencies()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });        
        }
        #endregion
    }
}