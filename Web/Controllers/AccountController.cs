using System.Security.Claims;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            try
            {
                _userService.RegisterUser(registerDto);

                TempData["SuccessMessage"] = "Kayıt işlemi tamamlandı. ";

                return RedirectToAction("Login");
            }
            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError("Email", exception.Message);

                return View(registerDto);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var existingUser = _userService.GetUserByEmail(loginDto.Email, false);

            if (existingUser is null)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi ile kayıtlı kullanıcı bulunamadı.");

                return View(loginDto);
            }

            var user = _userService.ValidateUser(loginDto);

            if (user is null)
            {
                ModelState.AddModelError("Password", "Şifre hatalı.");

                return View(loginDto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),

                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),

                new Claim(ClaimTypes.Email,user.Email)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "PublicEvent");

        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Profile()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdValue);

            var user = _userService.GetUserById(userId, false);

            if (user is null)
            {
                return RedirectToAction("Login");

            }

            var profileDto = new UserProfileDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate
            };

            return View(profileDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Profile(UserProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return View(profileDto);
            }

            try
            {
                _userService.UpdateProfile(profileDto);

                var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.NameIdentifier,profileDto.UserId.ToString()),

                      new Claim(ClaimTypes.Name,$"{profileDto.FirstName} {profileDto.LastName}"),

                      new Claim(ClaimTypes.Email,profileDto.Email)
                     };

                var identity = new ClaimsIdentity(claims, "CookieAuth");

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);


                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi";

                return RedirectToAction("Profile");
            }

            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError("Email", exception.Message);

                return View(profileDto);
            }
        }
    }
}
