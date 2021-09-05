using Logic.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private IUserRepository _userRepository;

        public CabinetController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("cabinet")]
        [Authorize]
        public IActionResult UserInformation()
        {
            var user = _userRepository.Read(User.Identity.Name);
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "account");
        }
    }
}
