using Common.Dtos;
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
            _profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
        }

        private GetOrganizationUserResponseDto GetCurrentUser()
        {
            var userKey = HttpContext.Session.GetString(Constants.LoggedInUserCachekey);
            return _cacheHelper.Get<GetOrganizationUserResponseDto>(userKey);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            var orgUser = GetCurrentUser();
            PartnerUserProfileModel profileModel = new PartnerUserProfileModel();
            _mapper.Map(orgUser, profileModel);
            if (!string.IsNullOrEmpty(profileModel.ImagePath) && !_fileHelper.IsProfileImageExists(profileModel.ImagePath, _profileImageFolder))
            {
                profileModel.ImagePath = Constants.DefaultProfileImage;
            }
            return View(profileModel);
        }

        [HttpPost("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(PartnerUserProfileModel model, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(model.ImageData) && !string.IsNullOrEmpty(model.ImageFilename))
            {
                model.ImagePath = _fileHelper.UploadImage(model.ImageData, model.ImageFilename, model.UserGuid.ToString(), _profileImageFolder);
            }
            UpdateOrganizationUserProfileCommand updateCommand = new UpdateOrganizationUserProfileCommand();
            _mapper.Map(model, updateCommand);

            var accessToken = _viewHelper.GetAccessToken(User);
            var userDto = await _partnerService.UpdateOrganizationUserAsync(updateCommand, accessToken, cancellationToken);
            if (!userDto.Success)
            {
                ModelState.AddModelError("", userDto?.Message ?? "Server error occured, please contact support");
            }
            return View("Profile", model);
        }


        [HttpGet("Group")]
        public async Task<IActionResult> GetGroup(CancellationToken cancellationToken)
        {
            ApplicationGroupModel model = new ApplicationGroupModel();
            var orgUser = GetCurrentUser();
            var accessToken = _viewHelper.GetAccessToken(User);
            var result = await _lookupsService.GetGroupByIdAsync(orgUser.GroupId, accessToken, cancellationToken);
            if (result.Success)
            {
                _mapper.Map(result, model);
            }
            return View(model);
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
