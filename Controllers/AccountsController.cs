using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareWeb.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HardwareWeb.Models;
using HardwareWeb.DataStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Policy = "NormalUserPolicy")]
        public async Task<IActionResult> Profile()
        {

            string claim_user_id = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;

            int id = -1;

            try
            {
                id = int.Parse(claim_user_id);
            }
            catch
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [Authorize(Policy = "NormalUserPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id, [Bind("UserId,Name,Email,Password,UserGroup")] User user)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                NotificationStore notification = new NotificationStore();
                notification.MessageText = "Profile Updated!";
                ViewBag.Message = notification;
                return View(user);
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
