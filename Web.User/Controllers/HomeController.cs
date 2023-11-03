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
        private readonly LookupsService _lookupsService;
        private readonly FileHelper _fileHelper;
        private readonly ViewHelper _viewHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Mapper _mapper;
        private readonly string _profileImageFolder;

        public HomeController(ILogger<HomeController> logger,
            AuthService authService,
            AuthHelper authHelper,
            CacheHelper cacheHelper,
            CacheKeyGenrator cachekeyGen,
            LookupsService lookupsService,
            FileHelper fileHelper,
            IWebHostEnvironment webHostEnvironment,
            ViewHelper viewHelper,
            Mapper mapper)
        {
            _logger = logger;
            _authService = authService;
            _authHelper = authHelper;
            _cacheHelper = cacheHelper;
            _cachekeyGen = cachekeyGen;
            _lookupsService = lookupsService;
            _fileHelper = fileHelper;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _viewHelper = viewHelper;
            _profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
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
        public async Task<IActionResult> Login(LoginModel model, CancellationToken cancellationToken)
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

                    var userDetails = await _authService.CheckUserExists(_viewHelper.GetUsername(principal), cancellationToken);
                    if (userDetails.Success)
                    {
                        var userKey = _cachekeyGen.CreateCacheKey(principal, Constants.LoggedInUserCachekey);
                        _cacheHelper.Set<ApplicationUserResponseDto>(userKey, userDetails);
                        HttpContext.Session.SetString(Constants.LoggedInUserCachekey, userKey);
                    }

                    var roleType = _viewHelper.GetUserRole(principal);
                    string controllerName = string.Empty; 
                    string actionName = "Index";
                    switch (roleType)
                    {
                        case RoleTypes.AppUser:
                            controllerName = "Home";
                            break;
                        case RoleTypes.Administrator:
                            controllerName = "Admin";
                            break;
                        case RoleTypes.Partner:
                            controllerName = "Partner";
                            break;
                    }
                    return RedirectToAction(actionName, controllerName);
                }
                else
                {
                    ModelState.AddModelError("", loginResponse.Message);
                }
            }
            return View(model);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterAppUserModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var registerResponse = await _authService.RegisterAsync(model.Email, model.Username, model.FirstName, model.LastName, model.Password, (int) RoleTypes.AppUser, cancellationToken);

                if (registerResponse.Success)
                {
                    LoginModel loginModel = new LoginModel { Username = model.Username, Password = model.Password };
                    return await Login(loginModel, cancellationToken);
                }
                else
                {
                    ModelState.AddModelError("", registerResponse.Message);
                }
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var loginDetails = _viewHelper.GetLoginDetails(User);
            var key = loginDetails.Item1;
            var loginData = loginDetails.Item2;
            await _authHelper.SignoutAsync(HttpContext);
            if (loginData != null)
            {
                string userid = _viewHelper.GetUserId(User);
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
            string userid = _viewHelper.GetUserId(User);
            string accessToken = _viewHelper.GetAccessToken(User);
            var userDto = await _authService.ProfileAsync(userid, accessToken, cancellationToken);
            AppUserProfileModel profileModel = new AppUserProfileModel();
            _mapper.Map(userDto, profileModel);
            if(!string.IsNullOrEmpty(profileModel.ImagePath) && !_fileHelper.IsProfileImageExists(profileModel.ImagePath, _profileImageFolder))
            {
                profileModel.ImagePath = Constants.DefaultProfileImage;
            }
            return View(profileModel);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(AppUserProfileModel profileModel, CancellationToken cancellationToken)
        {
            if(ModelState.IsValid)
            {
                string userid = _viewHelper.GetUserId(User);
                if(!string.IsNullOrEmpty(profileModel.ImageData) && !string.IsNullOrEmpty(profileModel.ImageFilename))
                {
                    profileModel.ImagePath = _fileHelper.UploadImage(profileModel.ImageData, profileModel.ImageFilename, userid, _profileImageFolder);
                }
                string accessToken = _viewHelper.GetAccessToken(User);
                var userDto = await _authService.UpdateProfileAsync(profileModel, accessToken, cancellationToken);
                if(userDto != null && userDto.Success)
                {
                    _mapper.Map(userDto, profileModel);
                    var userKey = HttpContext.Session.GetString(Constants.LoggedInUserCachekey);
                    if (!string.IsNullOrEmpty(userKey))
                    {
                        _cacheHelper.Set<ApplicationUserResponseDto>(userKey, userDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", userDto?.Message ?? "Server error occured, please contact support");
                }
                
            }
           
            return View(profileModel);
        }


        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("changepassword")]
        public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken)
        {
            ChangePasswordModel model = new ChangePasswordModel
            {
                UserId = _viewHelper.GetUserId(User)
            };
            return View(model);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("changepassword")]
        public async Task<IActionResult> PostChangePassword(ChangePasswordModel model, CancellationToken cancellationToken)
        {
            string userid = _viewHelper.GetUserId(User);
            var loginDetails = _viewHelper.GetLoginDetails(User);
            var key = loginDetails.Item1;
            var loginData = loginDetails.Item2;
            if (loginData != null)
            {
                var result = await _authService.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword, loginData.AccessToken, cancellationToken);
                if (result.Success)
                {
                    var logoutResult = await _authService.LogoutAsync(userid, loginData.AccessToken, cancellationToken);
                    if (logoutResult.Success)
                    {
                        await _authHelper.SignoutAsync(HttpContext);

                        HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                        _cacheHelper.Remove(key);

                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View("ChangePassword", model);
        }


        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.Administrator)]
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