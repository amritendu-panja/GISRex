using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.User.Helpers;
using Web.User.Models;
using Web.User.Services;

namespace Web.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        private readonly AuthHelper _authHelper;

        public HomeController(ILogger<HomeController> logger, AuthService authService, AuthHelper authHelper)
        {
            _logger = logger;
            _authService = authService;
            _authHelper = authHelper;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogin(LoginModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResponse = await _authService.LoginAsync(model.Username, model.Password, cancellationToken);
            if (loginResponse.Success)
            {
                await _authHelper.SignInAsync(loginResponse.AccessToken, HttpContext);
                return Redirect("/");
            }
            return RedirectToAction("login");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRegister(RegisterModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResponse = await _authService.RegisterAsync(model.Email, model.Username, model.Password, cancellationToken);

            if (registerResponse.Success)
            {
                return Redirect("login");
            }

            return RedirectToAction("register");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}