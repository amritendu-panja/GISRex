using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.User.Helpers;
using Web.User.Models;
using Web.User.Services;

namespace Web.User.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.Administrator)]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly PartnerService _partnerService;
        private readonly LookupsService _lookupsService;
        private readonly AuthService _authService;
        private readonly ViewHelper _viewHelper;
		private readonly string _profileImageFolder;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly FileHelper _fileHelper;
        private readonly Mapper _mapper;

		public AdminController(PartnerService partnerService, 
            LookupsService lookupsService,
            AuthService authService,
            ViewHelper viewHelper, 
            IWebHostEnvironment webHostEnvironment, 
            FileHelper fileHelper,
            Mapper mapper
            )
		{
			_partnerService = partnerService;
            _lookupsService = lookupsService;
            _authService = authService;
			_viewHelper = viewHelper;
			_webHostEnvironment = webHostEnvironment;
			_profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
			_fileHelper = fileHelper;
            _mapper = mapper;
		}

		public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            AdminLandingModel model = new AdminLandingModel();
            var accessToken = _viewHelper.GetAccessToken(User);
            var partnerMruListDto = await _partnerService.GetRecentPartners(accessToken, cancellationToken);
            if (partnerMruListDto.Success)
            {
                model.PartnerMruList = partnerMruListDto.Organizations;
            }

            var userListDto = await _authService.GetRecentUsersAsync(accessToken, cancellationToken);
            if (userListDto.Success)
            {
                model.UserMruList = userListDto.Users;
            }

            return View(model);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #region Partners
        [HttpGet("partners")]
        public async Task<IActionResult> Partners(CancellationToken cancellationToken)
        {
            PartnersDashboardModel model = new PartnersDashboardModel();
            
            return View(model);
        }

        [HttpGet("addpartner")]
        public async Task<IActionResult> AddPartner()
        {
            RegisterPartnerModel registerPartnerModel = new RegisterPartnerModel();
            return View(registerPartnerModel);
        }

        [HttpPost("addpartner")]
        //[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddPartner(RegisterPartnerModel registerPartnerModel, CancellationToken cancellationToken)
		{
			if(ModelState.IsValid)
            {
                var accessToken = _viewHelper.GetAccessToken(User);
                var result = await _partnerService.AddAsync(registerPartnerModel, accessToken, cancellationToken);
                if(result.Success)
                {
                    if (!string.IsNullOrEmpty(registerPartnerModel.ImageData) && !string.IsNullOrEmpty(registerPartnerModel.ImageFilename))
                    {
                        registerPartnerModel.LogoPath = _fileHelper.UploadImage(registerPartnerModel.ImageData, registerPartnerModel.ImageFilename, result.UserGuid.ToString(), _profileImageFolder);
                    }
                    return RedirectToAction("partners");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
					return View(registerPartnerModel);
				}
			}
            else
            {
				return View(registerPartnerModel);
			}			
		}

        [HttpGet("partner/{id}")]
        public async Task<IActionResult> PartnerProfile(int id, CancellationToken cancellationToken)
        {
            PartnerProfileModel profileModel = new PartnerProfileModel();

            var accessToken = _viewHelper.GetAccessToken(User);
            var organization = await _partnerService.GetByIdAsync(id, accessToken, cancellationToken);
            if (organization.Success)
            {
                _mapper.Map(organization, profileModel);
            }
            else
            {
                ModelState.AddModelError("", organization.Message);
            }

            return View(profileModel);
        }

        #endregion Partners

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
			var loginDetails = _viewHelper.GetLoginDetails(User);
			var loginData = loginDetails.Item2;
            var result = await _lookupsService.GetGroupByIdAsync(id, loginData.AccessToken, cancellationToken);
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

        [HttpGet("user/{userGuid}")]
        public async Task<IActionResult> GetUser(string userGuid, CancellationToken cancellationToken)
        {
            var accessToken = _viewHelper.GetAccessToken(User);
            var userDto = await _authService.ProfileAsync(userGuid, accessToken, cancellationToken);
            AppUserProfileModel profileModel = new AppUserProfileModel();
            _mapper.Map(userDto, profileModel);
            if (!string.IsNullOrEmpty(profileModel.ImagePath) && !_fileHelper.IsProfileImageExists(profileModel.ImagePath, _profileImageFolder))
            {
                profileModel.ImagePath = Constants.DefaultProfileImage;
            }
            return View(profileModel);
        }

    }
}
