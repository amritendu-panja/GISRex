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
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.Partner)]
    [Route("Partners")]
    public class PartnerController : Controller
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

        public PartnerController(
            PartnerService partnerService, 
            LookupsService lookupsService,
            ViewHelper viewHelper,
            IWebHostEnvironment webHostEnvironment, 
            FileHelper fileHelper, 
            Mapper mapper,
            CacheHelper cacheHelper,
            IOptions<AppSettings> options)
        {
            _partnerService = partnerService;
            _lookupsService = lookupsService;
            _viewHelper = viewHelper;
            _webHostEnvironment = webHostEnvironment;
            _fileHelper = fileHelper;
            _mapper = mapper;
            _cacheHelper = cacheHelper;
            _appSettings = options.Value;
            _profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
        }

        private int GetOrganizationId()
        {
            string key = HttpContext.Session.GetString(Constants.LoggedInUserCachekey);
            var orgUser = _cacheHelper.Get<GetApplicationOrganizationResponseDto>(key);
            return orgUser.OrganizationId;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            PartnerLandingModel model = new PartnerLandingModel();
            var orgId = GetOrganizationId();
            var userList = await _partnerService.GetOrganizationUserListAsync(orgId, _appSettings.Partners.MruListCount, _viewHelper.GetAccessToken(User), cancellationToken);
            if (userList.Success)
            {
                model.UserMruList = userList.Users;
            }
            
            return View(model);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #region Groups
        [HttpGet("groups")]
        public async Task<IActionResult> Groups()
        {
            return View();
        }

        [HttpGet("group/{id}")]
        public async Task<IActionResult> Group(int id, CancellationToken cancellationToken)
        {
            ApplicationGroupModel model = new ApplicationGroupModel();
            var accessToken = _viewHelper.GetAccessToken(User);
            var result = await _lookupsService.GetGroupByIdAsync(id, accessToken, cancellationToken);
            if (result.Success)
            {
                _mapper.Map(result, model);
            }
            return View(model);
        }

        #endregion Groups

        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            return View();
        }

        [HttpGet("adduser")]
        public async Task<IActionResult> AddUser(CancellationToken cancellationToken)
        {
            RegisterPartnerUserModel model = new RegisterPartnerUserModel();
            return View(model);
        }

        [HttpPost("adduser")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(RegisterPartnerUserModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string accessToken = _viewHelper.GetAccessToken(User);
                CreateOrganizationUserCommand command = new CreateOrganizationUserCommand();
                _mapper.Map(model, command);
                command.OrganizationId = GetOrganizationId();
                command.RoleId = (int)RoleTypes.PartnerUser;                
                var result = await _partnerService.CreateOrganizationUserAsync(command, accessToken, cancellationToken);
                if (result.Success)
                {
                    if (!string.IsNullOrEmpty(model.ImageData) && !string.IsNullOrEmpty(model.ImageFilename))
                    {
                        model.ImagePath = _fileHelper.UploadImage(model.ImageData, model.ImageFilename, result.UserGuid.ToString(), _profileImageFolder);
                    }

                    UpdateOrganizationUserProfileCommand updateCommand = new UpdateOrganizationUserProfileCommand();

                    _mapper.Map(model, updateCommand);
                    updateCommand.UserId = result.UserId;

                    var userDto = await _partnerService.UpdateOrganizationUserAsync(updateCommand, accessToken, cancellationToken);
                    if (!userDto.Success)
                    {
                        ModelState.AddModelError("", userDto?.Message ?? "Server error occured, please contact support");
                    }
                    else
                    {
                        return RedirectToAction("User", "Partners", new { id = result.UserGuid });
                    }
                }
                else
                {
                    ModelState.AddModelError("", result?.Message ?? "Server error occured, please contact support");
                }
            }
            return View(model);
        }

        [HttpGet("user/{userGuid}")]
        public async Task<IActionResult> GetUser(string userGuid, CancellationToken cancellationToken)
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

        [HttpPost("user/{userGuid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostUser(PartnerUserProfileModel profileModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(profileModel.ImageData) && !string.IsNullOrEmpty(profileModel.ImageFilename))
                {
                    profileModel.ImagePath = _fileHelper.UploadImage(profileModel.ImageData, profileModel.ImageFilename, profileModel.UserGuid.ToString(), _profileImageFolder);
                }
                string accessToken = _viewHelper.GetAccessToken(User);

                UpdateOrganizationUserProfileCommand command = new UpdateOrganizationUserProfileCommand();

                _mapper.Map(profileModel, command);

                var userDto = await _partnerService.UpdateOrganizationUserAsync(command, accessToken, cancellationToken);
                if (userDto != null && userDto.Success)
                {
                    _mapper.Map(userDto, profileModel);                    
                }
                else
                {
                    ModelState.AddModelError("", userDto?.Message ?? "Server error occured, please contact support");
                }
            }
            return View("GetUser", profileModel);
        }
    }
}
