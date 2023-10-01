using Microsoft.AspNetCore.Mvc;

namespace Web.User.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
