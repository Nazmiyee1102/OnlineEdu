using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEdu.WebUI.Controllers
{
    [AllowAnonymous]
    public class ErrorPageController : Controller
    {
        public IActionResult _404NotFound()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
