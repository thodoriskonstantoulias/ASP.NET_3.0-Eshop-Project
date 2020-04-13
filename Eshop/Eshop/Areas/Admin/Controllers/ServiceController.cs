using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize]
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
                    var uploads = Path.Combine(webrootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    serviceVm.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(serviceVm.Service);  
                }
                else // Update
                {
                    var serviceFromDb = _unitOfWork.Service.Get(serviceVm.Service.Id);

                    //Check image
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webrootPath, @"images\servives");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webrootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        serviceVm.Service.ImageUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        serviceVm.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(serviceVm.Service);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Repopulate the dropdowns
                serviceVm.CategoryList = _unitOfWork.Category.GetCategoryForDropDown();
                serviceVm.FrequencyList = _unitOfWork.Frequency.GetFrequencyForDropDown();
                return View(serviceVm);
            }
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAllServices()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties:"Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult DeleteService(int id)
        {
            var objToDelete = _unitOfWork.Service.Get(id);
            string webrootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webrootPath, objToDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (objToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Service.Remove(objToDelete);
            _unitOfWork.Save(); 

            return Json(new { success = true, message = "Service deleted" });
        }
        #endregion
    }
}