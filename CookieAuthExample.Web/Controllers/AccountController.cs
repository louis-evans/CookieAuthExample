using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CookieAuthExample.Web.ViewModels;
using CookieAuthExample.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CookieAuthExample.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl = null)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }

            ViewData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginModel)
        {
            var user = await GetUserAsync(loginModel.UserName, loginModel.Password);

            if (user != null)
            {
                var claims = new List<Claim> 
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                };

                claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                if(string.IsNullOrWhiteSpace(loginModel.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(loginModel.ReturnUrl);
                }
            }

            TempData["Error"] = "Either username of password is incorrect";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private Task<User> GetUserAsync(string userName, string password)
        {
            //Usually look up a user in a database here   
            return Task.Run(() => 
            {
                if(userName == "levans" && password == "pass")
                {
                    return new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "Louis",
                        LastName = "Evans",
                        Roles = new List<string> 
                        {
                            "Admin",
                            "Developer"
                        }
                    };
                }

                return null;
            });
        }
    }
}