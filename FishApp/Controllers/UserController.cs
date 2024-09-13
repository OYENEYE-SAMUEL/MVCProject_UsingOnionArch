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
            foreach (var role in response.Value.UserRoles)
            {
               // claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var pricipal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal, properties);
            if (response.Value.UserRoles.Select(ur => ur.Name).Contains("Admin"))
            {
                return RedirectToAction("DashBoard");
            }
            //if (response.Value.UserRoles.Any(ur => ur.Name == "Admin"))
            //{
            //    return RedirectToAction("DashBoard");
            //}
            if (response.Value.UserRoles.Any(ur => ur.Name == "Customer"))
            {
                return RedirectToAction("DashBoard");
            }
            if (response.Value.UserRoles.Any(ur => ur.Name == "Manager"))
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

        [HttpGet]
        public IActionResult DashBoard()
        {
            return View();
        }


    }
}
