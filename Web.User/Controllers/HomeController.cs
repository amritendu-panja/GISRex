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
                else
                {
                    ModelState.AddModelError("", loginResponse.Message);
                }
            }
            return View("Login", model);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var registerResponse = await _authService.RegisterAsync(model.Email, model.Username, model.Password, cancellationToken);

                if (registerResponse.Success)
                {
                    LoginModel loginModel = new LoginModel { Username = model.Username, Password = model.Password };
                    return await PostLogin(loginModel, cancellationToken);
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
            var loginDetails = GetLoginDetails();
            var key = loginDetails.Item1;
            var loginData = loginDetails.Item2;
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
            var loginDetails = GetLoginDetails();
            var loginData = loginDetails.Item2;
            var userDto = await _authService.ProfileAsync(userid, loginData.AccessToken, cancellationToken);
            ProfileModel profileModel = new ProfileModel();
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
        public async Task<IActionResult> Profile(ProfileModel profileModel, CancellationToken cancellationToken)
        {
            if(ModelState.IsValid)
            {
                string userid = User.FindFirst(Constants.JwtIdKey).Value;
                if(!string.IsNullOrEmpty(profileModel.ImageData) && !string.IsNullOrEmpty(profileModel.ImageFilename))
                {
                    profileModel.ImagePath = _fileHelper.UploadImage(profileModel.ImageData, profileModel.ImageFilename, userid, _profileImageFolder);
                }
                var loginDetails = GetLoginDetails();
                var loginData = loginDetails.Item2;
                var userDto = await _authService.UpdateProfileAsync(profileModel, loginData.AccessToken, cancellationToken);

                _mapper.Map(userDto, profileModel);
            }
           
            return View(profileModel);
        }


        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("changepassword")]
        public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken)
        {
            ChangePasswordModel model = new ChangePasswordModel
            {
                UserId = User.FindFirst(Constants.JwtIdKey).Value
            };
            return View(model);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("changepassword")]
        public async Task<IActionResult> PostChangePassword(ChangePasswordModel model, CancellationToken cancellationToken)
        {
            string userid = User.FindFirst(Constants.JwtIdKey).Value;
            var loginDetails = GetLoginDetails();
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

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Api Endpoints

        [ValidateAntiForgeryToken]
        [HttpGet("checkuser/{userName}")]
        public async Task<IActionResult> CheckUserExists(string userName, CancellationToken cancellationToken)
        {
            var result = await _authService.CheckUserExists(userName, cancellationToken);
            BaseResponseDto baseResponseDto = new BaseResponseDto
            {
                Success = result.Success
            };
            return new JsonResult(baseResponseDto);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
        {
            var countryList = _cacheHelper.Get<CountryLookupResponseDto>(Constants.CountryListCacheKey);
            if (countryList == null)
            {
                var loginDetails = GetLoginDetails();
                var loginData = loginDetails.Item2;
                countryList = await _lookupsService.GetAllCountriesAsync(loginData.AccessToken, cancellationToken);
                if (countryList.Success)
                {
                    _cacheHelper.Set<CountryLookupResponseDto>(Constants.CountryListCacheKey, countryList);
                }
            }
            return new JsonResult(countryList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("getCallingCode/{countryCode}")]
        public async Task<IActionResult> GetCallingCode(string countryCode, CancellationToken cancellationToken)
        {
            var countryList = _cacheHelper.Get<CountryLookupResponseDto>(Constants.CountryListCacheKey);
            if (countryList != null)
            {
                if (countryList.Countries.Any(countryList => countryList.CountryCode == countryCode))
                {
                    var callingCode = countryList.Countries.First(countryList => countryList.CountryCode == countryCode).CallingCode;
                    var response = new GetCallingCodeResponseDto { Success = true, CallingCode = callingCode };
                    return new JsonResult(response);
                }
            }
            
            var loginDetails = GetLoginDetails();
            var loginData = loginDetails.Item2;
            var result = await _lookupsService.GetCallingCodeAsync(countryCode, loginData.AccessToken, cancellationToken);
            return new JsonResult(result);            
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("states")]
        public async Task<IActionResult> GetAllStates(CancellationToken cancellationToken)
        {
            var stateList = _cacheHelper.Get<StateLookupResponseDto>(Constants.StateListCacheKey);
            if (stateList == null)
            {
                var loginDetails = GetLoginDetails();
                var loginData = loginDetails.Item2;
                stateList = await _lookupsService.GetAllStatesAsync(loginData.AccessToken, cancellationToken);
                if (stateList.Success)
                {
                    _cacheHelper.Set<StateLookupResponseDto>(Constants.StateListCacheKey, stateList);
                }
            }
            return new JsonResult(stateList);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("states/{countryCode}")]
        public async Task<IActionResult> GetStatesByCountry(string countryCode, CancellationToken cancellationToken)
        {
            var stateList = _cacheHelper.Get<StateLookupResponseDto>(Constants.StateListCacheKey);
            if (stateList == null)
            {
                var loginDetails = GetLoginDetails();
                var loginData = loginDetails.Item2;
                stateList = await _lookupsService.GetAllStatesAsync(loginData.AccessToken, cancellationToken);
                if (stateList.Success)
                {
                    _cacheHelper.Set<StateLookupResponseDto>(Constants.StateListCacheKey, stateList);
                }
            }
            StateLookupResponseDto stateLookupResponseDto = new StateLookupResponseDto();
            if (stateList.Success)
            {
                stateLookupResponseDto.States = stateList.States.Where(s => s.CountryCode == countryCode).ToList();
            }
            else
            {
                stateLookupResponseDto.SetError(stateList.Message);
            }
            return new JsonResult(stateLookupResponseDto);
        }
        #endregion

        #region Helpers
        private Tuple<string, LoginResponseDto> GetLoginDetails()
        {
            var key = _cachekeyGen.CreateCacheKey(User, Constants.AuthenticationCacheKey);
            var loginData = _cacheHelper.Get<LoginResponseDto>(key);
            return new Tuple<string, LoginResponseDto>(key, loginData);
        }
        #endregion
    }
}