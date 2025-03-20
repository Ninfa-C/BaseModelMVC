using System.Reflection.Emit;
using BaseModel.Models;
using BaseModel.Services;
using BaseModel.ViewModels;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseModel.Controllers
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
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                BirthDate = model.BirthDate,
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

            var user = await _userManager.FindByNameAsync(newUser.UserName);
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }


    }
}
