using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Eshop.Models;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models.ViewModels;

namespace Eshop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll(includeProperties:"Frequency")
            };

            return View(homeVm);
        }

        public IActionResult Details(int id)
        {
            var service = _unitOfWork.Service.GetFirstOrDefault(includeProperties:"Category,Frequency", filter:c=> c.Id == id);

            return View(service); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
