using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class AuthController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel? model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if(!_userService.CheckUserExistance(model.Username, model.Password).Result)
            {
                Console.WriteLine("AM ajuns aici deci nu gaseste");
                ModelState.AddModelError(string.Empty, "Nume de utilizator sau parolă incorecte.");
                return View(model);
            }
            Console.WriteLine("AM ajuns aici deci merge");
            // Autentificare utilizator
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Privacy", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
