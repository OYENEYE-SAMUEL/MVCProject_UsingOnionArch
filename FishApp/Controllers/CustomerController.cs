using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.X509;
using System.Text.Json;

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
            return View();
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

        [HttpGet]
        public IActionResult GetFish(Guid id)
        {
            var category = _fishService.GetById(id);
            if (!category.Status && category.Value == null)
            {
                return Content(category.Message);
            }
            return View(category.Value);
        }

        /* [HttpGet]
         public IActionResult OrderCount()
         {
             return View();
         }

         [HttpPost]
         public IActionResult OrderCount(int orderCount)
         {
             TempData["count"] = orderCount;
             return RedirectToAction("MakeOrder");
         }*/

        [HttpGet]
        public IActionResult OrderMessage()
        {
            return View();
        }


        [HttpGet]
        public IActionResult MakeOrder()
        {

            var allFish = _fishService.GetAllFish().Value
                .Select(c => new 
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    ImageUrl = c.FishImage,
                    Quantity = c.Quantity
                });

            var selectItems = allFish.Select(x => new
            {
                Id = x.Name,
                DisplayText = $"{x.Name} {x.Price}",
               /* ImageUrl = x.FishImage*/
            });

            ViewBag.allFish = new SelectList(selectItems, "Id", "DisplayText");
            ViewBag.FishListJson = JsonSerializer.Serialize(allFish);

            return View();
        }

            /*ViewBag.allFish = new SelectList(allFish, "Id", "DisplayText");
            return View();
        }


        /* [HttpGet]
         public IActionResult MakeOrder()
         {
             //var orders = new OrderRequestModel();
             //ViewBag.count = orders.OrderFishItems;
             var allFish = _fishService.GetAllFish()
                  .Value.Select(c => new
                  {
                      Id = c.Id,
                      DisplayText = c.Name + " - " + $"#{c.Price}" + " - " + c.FishImage
                  }).ToList();
             ViewBag.allFish = new SelectList(allFish, "Id", "DisplayText");
             ViewBag.FishListJson = JsonConvert.SerializeObject(allFish);
             return View();
             //ViewBag.FishListJson = JsonConverter(ViewBag.allFish.).se
         }*/
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
        public IActionResult OrderByCustomer()
        {
            var customer = _customerService.GetCustomer(_currentUser.GetCurrentUser());
            var order = _orderService.GetOrderByCustomer(customer.Value.Id);
            if (!order.Status && order.Value == null)
            {
                return Content(order.Message);
            }
            return View(order.Value);
        }

        [HttpGet]
        public IActionResult GetOrder(Guid id)
        {
            var category = _orderService.GetOrderById(id);
            if (!category.Status && category.Value == null)
            {
                return Content(category.Message);
            }
            return View(category.Value);
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
                ModelState.AddModelError(string.Empty, customer.Message);
                return View(model);
            }
            TempData["fundMessage"] = customer.Message;
            return RedirectToAction("DashBoard");
        }

        [HttpGet]
        public IActionResult WalletBalance()
        {
            var user = _currentUser.GetCurrentUser();
            var customer = _customerService.ViewCustomerWallet(user);
            if (!customer.Status)
            {
                ModelState.AddModelError(string.Empty, customer.Message);
                return RedirectToAction("DashBoard");
            }
            return View(customer.Value);
        }

        [HttpGet]
        public IActionResult ViewProfile()
        {
            var user = _currentUser.GetCurrentUser();
            var customer = _customerService.ViewProfile(user);
            if (!customer.Status)
            {
                ModelState.AddModelError(string.Empty, customer.Message);
                return RedirectToAction("DashBoard");
            }
            return View(customer.Value);
        }

        [HttpGet]
        public IActionResult UpdateCustomer(Guid customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            if (!customer.Status)
            {
                ModelState.AddModelError(string.Empty, customer.Message);
                return RedirectToAction("DashBoard");
            }
            return View(customer.Value);
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Guid id, CustomerRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = _customerService.Update(id, model);

            if (!customer.Status)
            {
                ModelState.AddModelError(string.Empty, customer.Message);
                return View(model);
            }
            TempData["updateMessage"] = customer.Message;
            return RedirectToAction("DashBoard");

        }

    }
}
