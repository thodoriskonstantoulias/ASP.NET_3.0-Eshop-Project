using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM vm = new ServiceVM()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWork.Category.GetCategoryForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyForDropDown()
            };

            if( id != null) //Edit
            {
                vm.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVm)
        {
            if (ModelState.IsValid)
            {
                string webrootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (serviceVm.Service.Id == 0) //Insert
                {
                    //Steps to save image to server
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootPath, @"images\servives");
                    var extension = Path.GetExtension(files[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    serviceVm.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(serviceVm.Service);  
                }
            }
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAllServices()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties:"Category,Frequency") });
        }
        #endregion
    }
}