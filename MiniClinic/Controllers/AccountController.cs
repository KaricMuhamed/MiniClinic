using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniClinic.Entities;
using MiniClinic.Models;
using System.Security.Claims;

namespace MiniClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly MiniClinicDbContext _context;
        
        public AccountController(MiniClinicDbContext miniClinicDbContext) 
        {
            _context = miniClinicDbContext;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Registration(RegistrationViewModel request)
        {
            if (ModelState.IsValid) 
            {

                User user = new User
                {
                    Email = request.Email,
                    Name = request.Name,
                    Password = AuthService.HashPassword(request.Password),
                    Role = request.Role
                };

                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{user.Name} registered successfuly!";

                }
                catch (DbUpdateException ex)
                {

                    ModelState.AddModelError("", "Please enter unique Email or Password!");
                    return View(request);
                }
                return View();
            }
            return View(request);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel request)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Email == request.Email).FirstOrDefault();

                if (user == null) 
                {
                    ModelState.AddModelError("", "Email or Password inncorrect! ");
                    return View(request);
                }

                bool isValidPass = AuthService.VerifyPassword(request.Password, user.Password);

                if (isValidPass)
                {
                    var stringRole = "To be set";
                    if(user.Role == UserRole.Admin) 
                    {
                        stringRole = "Admin";
                    }
                    else if (user.Role == UserRole.Doctor) 
                    {
                        stringRole = "Doctor";
                    } 
                    else
                    {
                        stringRole = "Unknown";
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, stringRole)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("SecurePage");
                }
                else 
                {
                    ModelState.AddModelError("", "Email or Password inncorrect! ");
                }
            }
            return View(request);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (ViewBag.Role == "Doctor")
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    ViewBag.Patients = _context.Patients
                        .Where(p => p.DoctorId.ToString() == userId)
                        .ToList();
                }
            }

            return View();
        }


    }
}
