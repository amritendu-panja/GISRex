using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Settings;
using Web.User.Models;
using Web.User.Services;
using Web.User.Helpers;
using Microsoft.AspNetCore.Hosting;

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

		public AdminController(PartnerService partnerService, ViewHelper viewHelper, IWebHostEnvironment webHostEnvironment, FileHelper fileHelper)
		{
			this._partnerService = partnerService;
			this._viewHelper = viewHelper;
			_webHostEnvironment = webHostEnvironment;
			_profileImageFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "userprofiles");
			_fileHelper = fileHelper;
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

        [HttpGet("partners")]
        public async Task<IActionResult> Partners()
        {
            return View();
        }

        [HttpGet("partner")]
        public async Task<IActionResult> Partner([FromQuery] int? orgId=null)
        {
            RegisterPartnerModel registerPartnerModel = new RegisterPartnerModel();
            return View(registerPartnerModel);
        }

        [HttpPost("partner")]
        //[ValidateAntiForgeryToken]
		public async Task<IActionResult> Partner(RegisterPartnerModel registerPartnerModel, CancellationToken cancellationToken)
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
