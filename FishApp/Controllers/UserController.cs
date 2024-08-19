using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;
using System.Security.Claims;

namespace FishApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        public UserController(IUserService userService, ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserRequestModel model)
        {
            var response = _userService.Login(model);
            if (!response.Status)
            {
                return Content(response.Message);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.Value.Id.ToString()),
                new Claim(ClaimTypes.Email, response.Value.Email),
                new Claim(ClaimTypes.Name, response.Value.FullName),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var pricipal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal, properties);
            if (response.Value.UserRoles.Any(ur => ur.Role.Name == "Admin"))
            {
                return RedirectToAction("DashBoard");
            }
            if (response.Value.UserRoles.Any(ur => ur.Role.Name == "Customer"))
            {
                return RedirectToAction("DashBoard");
            }
            if (response.Value.UserRoles.Any(ur => ur.Role.Name == "Manager"))
            {
                return RedirectToAction("DashBoard");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        /*public IActionResult DashBoard()
        {
            var response = _currentUser.GetCurrentUser();
            if (response.)
            {
                
            }
            return View();
        }*/

       
      }
}
