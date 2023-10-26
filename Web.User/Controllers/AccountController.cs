using Microsoft.AspNetCore.Mvc;
using Web.User.Models;

namespace Web.User.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error([FromQuery(Name = "error")] string error)
        {
            if (string.IsNullOrWhiteSpace(error)) 
            {
                return View();
            }
            var errorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationErrorModel>(error);
            if (errorMessage != null)
            {
                return View(errorMessage);
            }
            return View();
        }
    }
}
