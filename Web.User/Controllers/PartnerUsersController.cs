using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.User.Helpers;
using Web.User.Models;
using Web.User.Services;

namespace Web.User.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.PartnerUser)]
    [Route("PartnerUsers")]
    public class PartnerUsersController : Controller
    {
        private readonly PartnerService _partnerService;
        private readonly LookupsService _lookupsService;
        private readonly ViewHelper _viewHelper;
        private readonly string _profileImageFolder;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileHelper _fileHelper;
        private readonly Mapper _mapper;
        private readonly CacheHelper _cacheHelper;
        private readonly AppSettings _appSettings;

        public PartnerUsersController(
            PartnerService partnerService,
            LookupsService lookupsService,
            ViewHelper viewHelper,
            IWebHostEnvironment webHostEnvironment,
            FileHelper fileHelper,
            Mapper mapper,
            CacheHelper cacheHelper,
            IOptions<AppSettings> appSettings)
        {
            _partnerService = partnerService;
            _lookupsService = lookupsService;
            _viewHelper = viewHelper;
            _webHostEnvironment = webHostEnvironment;
            _fileHelper = fileHelper;
            _mapper = mapper;
            _cacheHelper = cacheHelper;
            _appSettings = appSettings.Value;
            _profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(Guid userId, CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(Guid userId, CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet("Group")]
        public async Task<IActionResult> GetGroup(Guid userId, CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetOrganizationUsers(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet("User/{userGuid}")]
        public async Task<IActionResult> GetUserView(string userGuid, CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            var userDto = await _partnerService.GetOrganizationUserAsync(userGuid, accessToken, cancellationToken);
            PartnerUserProfileModel profileModel = new PartnerUserProfileModel();
            _mapper.Map(userDto, profileModel);
            if (!string.IsNullOrEmpty(profileModel.ImagePath) && !_fileHelper.IsProfileImageExists(profileModel.ImagePath, _profileImageFolder))
            {
                profileModel.ImagePath = Constants.DefaultProfileImage;
            }
            return View(profileModel);
        }
    }
}
