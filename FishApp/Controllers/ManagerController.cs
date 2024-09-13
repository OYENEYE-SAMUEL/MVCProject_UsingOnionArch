using Application.DTO;
using Application.Interfaces.Services;
using Domain.Enum;
using FishApp.ViewModel;
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
        public IActionResult GetPond(Guid id)
        {
            var pond = _pondService.GetPondId(id);
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

        [HttpGet]
        public IActionResult UpdatePond(Guid id)
        {
            var pond = _pondService.GetPondId(id);
            var update = new PondUpdateViewModel
            {
                Id = pond.Value.Id,
                Name = pond.Value.Name,
                Description = pond.Value.Description,
                Dimension = pond.Value.Dimension,
            };
            return View(update);
        }

        [HttpPost]
        public IActionResult UpdatePond(Guid id, PondRequestModel model)
        {
            var pond = _pondService.UpdatePond(id, model);
            if (pond.Value == null)
            {
                return Content(pond.Message);
            }
            TempData["inform"] = pond.Message;
            return RedirectToAction("AllPonds");
        }

        public IActionResult CreateFish()
        {
            var pond = _pondService.GetAll();
            ViewBag.allPond = new SelectList(pond.Value, "Id", "Name");
            return View();
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
                TempData["Fishmessage"] = fish.Message;
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
        public IActionResult UpdateFish(Guid id)
        {
            var fish = _fishService.GetById(id);
            var pond = _pondService.GetAll();
            ViewBag.allPond = new SelectList(pond.Value, "Id", "Name");
            return View(fish.Value);
        }

        [HttpPost]
        public IActionResult UpdateFish(Guid id, FishRequestModel model)
        {
            var fish = _fishService.UpdateFish(id, model);
            if(!fish.Status)
            {
                Content(fish.Message);
            }
            TempData["note"] = fish.Message;
            return RedirectToAction("AllFish");
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
        public IActionResult GetOrder(Guid id)
        {
            var order = _orderService.GetOrderById(id);
            if (!order.Status && order.Value == null)
            {
                return Content(order.Message);
            }
            return View( order.Value);
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
