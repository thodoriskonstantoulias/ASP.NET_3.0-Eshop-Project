using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Extensions;
using Eshop.Models;
using Eshop.Models.ViewModels;
using Eshop.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        CartVM cartVm;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cartVm = new CartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>()
            };

        }

        public IActionResult Index()
        {           
            //Get the services from the session
            if (HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    cartVm.ServiceList.Add(_unitOfWork.Service.GetFirstOrDefault(s => s.Id == serviceId, "Frequency,Category"));
                }
            }

            return View(cartVm);
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(StatDetails.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            //Get the services from the session
            if (HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(StatDetails.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    cartVm.ServiceList.Add(_unitOfWork.Service.GetFirstOrDefault(s => s.Id == serviceId, "Frequency,Category"));
                }
            }

            return View(cartVm);
        }
    }
}