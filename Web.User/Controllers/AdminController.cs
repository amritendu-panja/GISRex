using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Web.User.Helpers;
using Web.User.Models;
using Web.User.Services;

namespace Web.User.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.Administrator)]
    public class AdminController : Controller
    {
        private readonly PartnerService _partnerService;
        private readonly ViewHelper _viewHelper;
		private readonly string _profileImageFolder;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly FileHelper _fileHelper;
        private readonly Mapper _mapper;

		public AdminController(PartnerService partnerService, 
            ViewHelper viewHelper, 
            IWebHostEnvironment webHostEnvironment, 
            FileHelper fileHelper,
            Mapper mapper
            )
		{
			this._partnerService = partnerService;
			this._viewHelper = viewHelper;
			_webHostEnvironment = webHostEnvironment;
			_profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
			_fileHelper = fileHelper;
            _mapper = mapper;
		}

		public async Task<IActionResult> Index()
        {
            return View();
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
            var loginDetails = _viewHelper.GetLoginDetails(User);
            var loginData = loginDetails.Item2;
            var partnerMruListDto = await _partnerService.GetRecentPartners(loginData.AccessToken, cancellationToken);
            if (partnerMruListDto.Success)
            {
                model.PartnerMruList = partnerMruListDto.Organizations;
            }
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
				string userid = _viewHelper.GetUserId(User);
				if (!string.IsNullOrEmpty(registerPartnerModel.ImageData) && !string.IsNullOrEmpty(registerPartnerModel.ImageFilename))
				{
					registerPartnerModel.LogoPath = _fileHelper.UploadImage(registerPartnerModel.ImageData, registerPartnerModel.ImageFilename, userid, _profileImageFolder);
				}

				var loginDetails = _viewHelper.GetLoginDetails(User);
				var loginData = loginDetails.Item2;
                var result = await _partnerService.AddAsync(registerPartnerModel, loginData.AccessToken, cancellationToken);
                if(result.Success)
                {
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

            var loginDetails = _viewHelper.GetLoginDetails(User);
            var loginData = loginDetails.Item2;
            var organization = await _partnerService.GetByIdAsync(id, loginData.AccessToken, cancellationToken);
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
        [HttpGet("groups")]
        public async Task<IActionResult> Groups()
        {
            return View();
        }

        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            return View();
        }
    }
}
