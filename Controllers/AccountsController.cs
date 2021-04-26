using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareWeb.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HardwareWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace HardwareWeb.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HardwareContext _context;

        public AccountsController(HardwareContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            
                          
            if (ModelState.IsValid)
            {
                var accountQ = _context.Users
                .Where(e => e.Email == model.Email && e.Password == model.Password);

                if (accountQ.Any())
                {
                    var account = accountQ.FirstOrDefault();

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, account.Name),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim("UserId",account.UserId.ToString()),
                        new Claim("UserGroup",account.UserGroup)
                    };

                    var claimIdentity = new ClaimsIdentity(claims, "Cookie Auth");

                    var userPrincipal = new ClaimsPrincipal(new[] { claimIdentity });

                    HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or Password Incorrect";
                    return View();
                }
            }
            else
            {
                return View();

            }
        }

    }
}
