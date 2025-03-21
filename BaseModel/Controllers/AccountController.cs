using System.Net;
using System.Reflection.Emit;
using System.Security.Claims;
using HotelManagment.Models;
using HotelManagment.Services;
using HotelManagment.ViewModels;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly EmailServices _emailServices;
        public AccountController(EmailServices email, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _roleManager = roleManager;
            _emailServices = email;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var newUser = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token }, Request.Scheme);
            var emailModel = new EmailViewModel
            {
                Name = newUser.Name,
                ConfirmationEmail = confirmationLink
            };
            var email = await _emailServices
                .To(newUser.Email)
                .Subject("Conferma la tua registrazione")
                .UsingTemplateFromFile(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Email", "ConfirmationEmail.cshtml"), emailModel)
                .SendAsync();

            var user = await _userManager.FindByEmailAsync(newUser.Email);
            await _userManager.AddToRoleAsync(user, "Receptionist");
            return RedirectToAction("Index", "Prenotazioni");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Prenotazioni");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var signIn = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (!signIn.Succeeded)
            {
                return RedirectToAction("Register", "Account");
            }
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.GivenName, $"{user.Name} {user.Surname}"));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Prenotazioni");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
