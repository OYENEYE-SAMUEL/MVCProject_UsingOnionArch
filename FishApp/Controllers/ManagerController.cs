using Application.DTO;
using Application.Interfaces.Services;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FishApp.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IFishService _fishService;
        private readonly IPondService _pondService;
        private readonly IOrderService _orderService;
        public ManagerController(IStaffService staffService, IFishService fishService, IPondService pondService, IOrderService orderService)
        {
            _staffService = staffService;
            _fishService = fishService;
            _pondService = pondService;
            _orderService = orderService;
        }
        [HttpGet]
        public IActionResult DashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterManager()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterManager(StaffRequestModel model)
        {
            var manager = _staffService.RegisterStaff(model);
            if (manager.Value == null)
            {
                return Content(manager.Message);
            }
            TempData["message"] = manager.Message;
            return RedirectToAction("DashBoard", "Admin");
        }
        [HttpGet]
        public IActionResult CreatePond()
        {
            var model = new PondRequestModel();
            return View(model);
        }

        [HttpPost]

        public IActionResult CreatePond(PondRequestModel model)
        {
            var response = _pondService.Create(model);
            if (response.Value == null)
            {
                return Content(response.Message);
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult GetPond(string name)
        {
            var pond = _pondService.GetPondName(name);
            if (!pond.Status && pond.Value == null)
            {
                return NotFound();
            }
            return View(pond.Value);
        }

        [HttpGet]

        public IActionResult AllPonds()
        {
            var ponds = _pondService.GetAll();
            return View(ponds.Value);
        }

        public IActionResult CreateFish()
        {
            var pond = _pondService.GetAll();
            ViewBag.allPond = new SelectList(pond.Value, "Id", "Name");
            var model = new FishRequestModel(); 
            return View(model);
        }

        [HttpPost]

        public IActionResult CreateFish(FishRequestModel model)
        {
            var fish = _fishService.CreateFish(model);
            if (ModelState.IsValid)
            {
                if (fish.Status.Equals(false))
                {
                    return Content(fish.Message);
                }
                return RedirectToAction("DashBoard");
            }

            ModelState.AddModelError("", "Fill the required models");
            return View(fish.Value);

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

        [HttpGet]
        public IActionResult GetPendingOrders()
        {
            var orders = _orderService.GetAllOrder();
            if(orders.Value == null)
            {
                return Content(orders.Message);
            }
            return View(orders.Value);
        }

        [HttpGet]
        public IActionResult ApprovedOrders(Guid id)
        {
            var orders = _orderService.ApproveOrder(id);
            if (!orders.Status)
            {
                return Content(orders.Message);
            }
            return View(orders.Value);
        }

        [HttpGet]
        public IActionResult DeliveredOrder(Guid id)
        {
            var orders = _orderService.DeliveredOrder(id);
            if (!orders.Status)
            {
                return Content(orders.Message);
            }
            return View(orders.Value);
        }

      
    }

    
}
