using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Settings;
using Web.User.Models;

namespace Web.User.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleTypeNames.Administrator)]
    public class AdminController : Controller
    {
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
