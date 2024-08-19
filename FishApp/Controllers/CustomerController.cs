using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FishApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IFishService _fishService;
        private readonly IOrderService _orderService;
        private readonly ICurrentUser _currentUser;
        public CustomerController(ICustomerService customerService, IFishService fishService, IOrderService orderService, ICurrentUser currentUser)
        {
            _customerService = customerService;
            _fishService = fishService;
            _orderService = orderService;
            _currentUser = currentUser;
        }
        [HttpGet]
        public IActionResult DashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            var model = new CustomerRequestModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterCustomer(CustomerRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerService.Register(model);
                {
                    if (!customer.Status)
                    {
                        return Content(customer.Message);
                    }
                    TempData["request"] = customer.Message;
                    return RedirectToAction("Login", "User");
                }
            }

            ModelState.AddModelError("", "Registration failed. Email may already be in use or role not found.");
            return View(model);
        }

        [HttpGet]
        public IActionResult AllFish()
        {
            var category = _fishService.GetAllFish();
            return View(category.Value);
        }

        
       /* public IActionResult OrderCount()
        {
            return View();
        }*/

       /* public IActionResult OrderCount(int orderCount)
        {
            TempData["count"] = orderCount;
            return RedirectToAction("MakeOrder");
        }*/

        [HttpGet]
        public IActionResult MakeOrder()
        {
            var allFish = _fishService.GetAllFish()
                 .Value.Select(c => new
                 {
                     DisplayText = c.Name + " - " + c.Price
                 }).ToList();
            ViewBag.allFish = new SelectList(allFish, "Id", "DisplayText");
            ViewBag.FishListJson = JsonConvert.SerializeObject(ViewBag.FishList.Items);
            //ViewBag.FishListJson = JsonConverter(ViewBag.allFish.).se
            return View();
        }
        [HttpPost]
        public IActionResult MakeOrder(OrderRequestModel model)
        {
            var response = _orderService.MakeOrder(model);
            if (!response.Status)
            {
                TempData["reply"] = response.Message;
                return RedirectToAction("OrderMessage");
            }
            if (response.Message.Contains("which is less than the quantity ordered"))
            {
                ViewBag.response = response.Message.Split('/')[0];
                return View();
            }
            if (response.Value == null)
            {
                return View();
            }
            TempData["alert"] = response.Message;
            return RedirectToAction("DashBoard");
        }

        [HttpGet]
        public IActionResult GetOrderByCustomer(Guid customerId)
        {
            var order = _orderService.GetOrderByCustomer(customerId);
            if (!order.Status && order.Value == null)
            {
                return Content(order.Message);
            }
            return View(order.Value);
        }

        [HttpGet]
        public IActionResult FundWallet()
        {
            var user = _currentUser.GetCurrentUser();
            ViewData["user"] = user;
            return View();
        }

        [HttpPost]
        public IActionResult FundWallet(CustomerRequestModel model, decimal amount)
        {
            var customer = _customerService.FundWallet(model, amount);
            if (!customer.Status)
            {
                return Content(customer.Message);
            }

            return RedirectToAction("DashBoard");
        }

    }
}
