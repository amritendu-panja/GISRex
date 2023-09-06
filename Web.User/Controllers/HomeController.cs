using Common.Dtos;
using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Principal;
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
        private readonly CacheHelper _cacheHelper;
        private readonly CacheKeyGenrator _cachekeyGen;
        private readonly Mapper _mapper;

        public HomeController(ILogger<HomeController> logger,
            AuthService authService,
            AuthHelper authHelper,
            CacheHelper cacheHelper,
            CacheKeyGenrator cachekeyGen,
            Mapper mapper)
        {
            _logger = logger;
            _authService = authService;
            _authHelper = authHelper;
            _cacheHelper = cacheHelper;
            _cachekeyGen = cachekeyGen;
            _mapper = mapper;
        }

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
            if (ModelState.IsValid)
            {
                var loginResponse = await _authService.LoginAsync(model.Username, model.Password, cancellationToken);
                if (loginResponse.Success)
                {
                    var principal = await _authHelper.SignInAsync(loginResponse.AccessToken, loginResponse.ExpiresAt, HttpContext);
                    var key = _cachekeyGen.CreateCacheKey(principal, Constants.AuthenticationCacheKey);
                    _cacheHelper.Set<LoginResponseDto>(key, loginResponse);
                    HttpContext.User = principal;
                    HttpContext.Session.SetString(Constants.AuthenticationCacheKey, key);
                    return Redirect("/");
                }
            }
            return View();
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
            if (ModelState.IsValid)
            {
                var registerResponse = await _authService.RegisterAsync(model.Email, model.Username, model.Password, cancellationToken);

                if (registerResponse.Success)
                {
                    LoginModel loginModel = new LoginModel { Username = model.Username, Password = model.Password };
                    return await PostLogin(loginModel, cancellationToken);
                }
            }

            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var key = _cachekeyGen.CreateCacheKey(User, Constants.AuthenticationCacheKey);
            var loginData = _cacheHelper.Get<LoginResponseDto>(key);
            await _authHelper.SignoutAsync(HttpContext);
            if (loginData != null)
            {
                string userid = User.FindFirst(Constants.JwtIdKey).Value;
                var logoutResponse = await _authService.LogoutAsync(userid, loginData.AccessToken, cancellationToken);
                if (logoutResponse.Success)
                {
                    _cacheHelper.Remove(key);
                }
            }
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            string userid = User.FindFirst(Constants.JwtIdKey).Value;
            var key = _cachekeyGen.CreateCacheKey(User, Constants.AuthenticationCacheKey);
            var loginData = _cacheHelper.Get<LoginResponseDto>(key);
            var userDto = await _authService.ProfileAsync(userid, loginData.AccessToken, cancellationToken);
            ProfileModel profileModel = new ProfileModel();
            _mapper.Map(userDto, profileModel);
            return View(profileModel);
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