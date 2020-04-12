using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models.ViewModels;
using Eshop.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderHeader.Id == id)
            };

            return View(orderVM);
        }
        public IActionResult Approve(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderHeader.ChangeOrderStatus(id, StatDetails.StatusApproved);

            return View(nameof(Index));
        }

        public IActionResult Reject(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderHeader.ChangeOrderStatus(id, StatDetails.StatusRejected);

            return View(nameof(Index));
        }



        #region API Calls

        public IActionResult GetAllOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll() });
        }

        public IActionResult GetAllPendingOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(o => o.Status == StatDetails.StatusSubmitted) });
        }

        public IActionResult GetAllApprovedOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(o => o.Status == StatDetails.StatusApproved) });
        }

        #endregion
    }
}