using Microsoft.AspNetCore.Mvc;
using OnlineEdu.WebUI.DTOs.UserDtos;
using OnlineEdu.WebUI.Services.UserDtos;

namespace OnlineEdu.WebUI.Controllers
{
    public class LoginController(IUserService userService) : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginDto userLoginDto) 
        { 
            var userRole = await userService.LoginAsync(userLoginDto);

            if (userRole== "Admin" )
            {
                return RedirectToAction("Index","About", new {area="Admin"});
            }

            if (userRole == "Teacher")
            {
                return RedirectToAction("Index", "MyCourse", new { area = "Teacher" });
            }

            if (userRole == "Student")
            {
                return RedirectToAction("Index", "CourseRegister", new { area = "Student" });
            }

            else 
            {
                ModelState.AddModelError("", "Email veya Şifre Hatalı.");
                return View();
            }
        }
    }
}
