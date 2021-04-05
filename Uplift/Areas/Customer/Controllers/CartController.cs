using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public CartViewModel CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ServiceList = new List<Service>()
            };
        }
        
        public IActionResult Index()
        {
            if (HttpContext.Session.IsExist(SD.SessionCart))
            {
                var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (var serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(unitOfWork.Service
                        .GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
                return View(CartVM);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (var serviceId in sessionList)
                {
                    CartVM.ServiceList
                        .Add(unitOfWork.Service.GetFirstOrDefault(
                            u => u.Id == serviceId,
                            includeProperties: "Frequency,Category"));
                }

                return View(CartVM);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(unitOfWork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }

            if (!ModelState.IsValid)
                return View(CartVM);
            
            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.Status = SD.StatusSubmitted;
            CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;
            unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            unitOfWork.Save();

            foreach (var item in CartVM.ServiceList)
            {
                OrderDetail orderDetails = new OrderDetail
                {
                    ServiceId = item.Id,
                    OrderHeaderId = CartVM.OrderHeader.Id,
                    ServiceName = item.Name,
                    Price = item.Price
                };

                unitOfWork.OrderDetail.Add(orderDetails);
                unitOfWork.Save();
            }

            HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
            return RedirectToAction("OrderConfirmation", "Cart", new {id = CartVM.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }
    }
}