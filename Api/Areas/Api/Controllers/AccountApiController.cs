using Api.Models;
using Logic.Models;
using Logic.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Areas.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private IUserRepository _userRepository;

        public AccountApiController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterRequest model)
        {
            #region Валидация

            if (_userRepository.CheckPhoneExist(model.Phone)) ModelState.AddModelError(nameof(model.Email), "Введен существующий телефон");
            if (_userRepository.CheckEmailExist(model.Email)) ModelState.AddModelError(nameof(model.Email), "Введен существующий Email");
            if (!ModelState.IsValid) return BadRequest(GetErrorResponse("Пользователь уже зарегистрирован"));

            #endregion

            var user = new User() { Phone = model.Phone, Email = model.Email, FIO = model.FIO, Password = model.Password };
            _userRepository.Create(user);
            return Ok("");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (User.Identity.Name == model.Phone) return BadRequest(GetErrorResponse("Пользователь уже авторизован"));

            if (!ModelState.IsValid && !_userRepository.CheckUserExist(model.Phone, model.Password)) 
                return BadRequest(GetErrorResponse("Некорректный логин или пароль"));

            _userRepository.Update(model.Phone);

            await Authenticate(model.Phone);

            return Ok(HttpContext.Request.Cookies);
        }

        [HttpGet]
        [Route("get-my-info")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userData = _userRepository.Read(User.Identity.Name);
            return Ok(userData);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
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

        private ErrorResponse GetErrorResponse(string message)
        {
            return new ErrorResponse() { Code = BadRequest(ModelState).StatusCode, Message = message };
        }
    }
}