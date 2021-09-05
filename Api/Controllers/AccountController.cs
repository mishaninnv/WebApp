using Api.Models;
using Logic.Context;
using Logic.Models;
using Logic.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            #region Валидация

            if (_userRepository.CheckPhoneExist(model.Phone)) ModelState.AddModelError(nameof(model.Phone), "Введен существующий номер");
            if (_userRepository.CheckEmailExist(model.Email)) ModelState.AddModelError(nameof(model.Email), "Введен существующий Email");
            if (!ModelState.IsValid) return View(model);

            #endregion

            var user = new User { FIO = model.FIO, Email = model.Email, Password = model.Password, Phone = model.Phone, LastLogin = DateTime.Now };
            _userRepository.Create(user);

            await Authenticate(model.Phone);

            return RedirectToAction("success", new { model.FIO});
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            #region Валидация

            if (!_userRepository.CheckUserExist(model.Phone, model.Password)) ModelState.AddModelError(nameof(model.Phone), "Ошибка авторизации");

            if (!ModelState.IsValid) return View(model);

            #endregion

            _userRepository.Update(model.Phone);

            await Authenticate(model.Phone);

            return RedirectToAction("UserInformation", "cabinet");
        }

        [Route("success/{fio}")]
        public IActionResult Success(string fio)
        {
            ViewBag.FIO = fio;
            return View();
        }

        private async Task Authenticate(string phone)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, phone)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}