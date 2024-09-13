using Application.DTO;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FishApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IFishService _fishService;
        private readonly IStaffService _staffService;
        private readonly IPondService _pondService;
        public AdminController(IRoleService roleService, IFishService fishService, IStaffService staffService, IPondService pondService)
        {
            _roleService = roleService;
            _fishService = fishService;
            _staffService = staffService;
            _pondService = pondService;
        }

        [HttpGet]
        public IActionResult DashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {    
            return View();
        }

        [HttpPost]
        public IActionResult CreateRole(RoleRequestModel model)
        {
            var response = _roleService.Create(model);
            if (response.Value == null)
            {
                return View(model);
            }
            TempData["notice"] = response.Message;
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpGet]
        public IActionResult GetRole(string name)
        {
            var role = _roleService.GetRole(name);
            if (!role.Status && role.Value == null)
            {
                return Content(role.Message);
            }
            return View(role.Value);
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            var roles = _roleService.GetAll();
            if (roles.Value == null)
            {
                return Content(roles.Message);
            }
            return View(roles.Value);

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
        public IActionResult AllManager()
        {
            var managers = _staffService.GetAllStaffs();
            if (managers.Value == null)
            {
                return Content(managers.Message);
            }
            return View(managers.Value);
        }

        [HttpGet]
        public IActionResult AllPonds()
        {
            var ponds = _pondService.GetAll();
            return View(ponds.Value);
        }

        [HttpGet]
        public IActionResult GetManager(Guid id)
        {
            var manager = _staffService.GetStaffById(id);
            if (manager.Value == null)
            {
                return Content(manager.Message);
            }
            return View(manager.Value);
        }

        [HttpGet]
        public IActionResult AllFish()
        {
            var category = _fishService.GetAllFish();
            return View(category.Value);
        }
    }
}
