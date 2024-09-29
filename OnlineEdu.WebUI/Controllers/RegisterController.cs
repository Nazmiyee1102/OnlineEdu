using Microsoft.AspNetCore.Mvc;
using OnlineEdu.WebUI.DTOs.UserDtos;
using OnlineEdu.WebUI.Services.UserDtos;

namespace OnlineEdu.WebUI.Controllers
{
    public class RegisterController(IUserService userService) : Controller
    {
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserRegisterDto userRegisterDto)
        {
            var result = await userService.CreateUserAsync(userRegisterDto);
            if (!result.Succeeded || !ModelState.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View();
            }
            return RedirectToAction("SignIn","Login");
        }
    }
}
