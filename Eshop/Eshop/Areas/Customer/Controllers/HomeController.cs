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
using Microsoft.AspNetCore.Http;
using Eshop.Utility;
using Eshop.Extensions;

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

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>(); 

            //Check to see if there is a session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(StatDetails.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(StatDetails.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart);
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(StatDetails.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
