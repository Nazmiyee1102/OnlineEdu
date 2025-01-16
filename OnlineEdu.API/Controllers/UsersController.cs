using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.Business.Abstract;
using OnlineEdu.DTO.DTOs.UserDtos;
using OnlineEdu.Entity.Entities;

namespace OnlineEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IJwtService _jwtService, IMapper _mapper) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto _userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(_userLoginDto.Email);
            if (user == null)
            {
                return BadRequest("Bu Email Sistemde Kayıtlı Değil!");
            }
            var result = await _signInManager.PasswordSignInAsync(user, _userLoginDto.Password, false,false);//false: kullanıcı sürekli sistemde olsun mu? hayır, false: girişte hata olursa sistem kitlensin mi? hayır
            if (!result.Succeeded)
            {
                return BadRequest("Kullanıcı Adı veya Şifre Hatalı!");
            }

            var token = await _jwtService.CreateTokenAsync(user);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto _registerDto)
        {
            var user = _mapper.Map<AppUser>(_registerDto);
            if (ModelState.IsValid )
            {
                var result = await _userManager.CreateAsync(user, _registerDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await _userManager.AddToRoleAsync(user, "Student");
                return Ok("Kullanıcı Oluşturuldu");
            }

            return BadRequest();
        }

        [HttpGet("TeacherList")]
        public async Task<IActionResult> TeacherList()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            return Ok(teachers);
        }

        [HttpGet("StudentList")]
        public async Task<IActionResult> StudentList()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            return Ok(students);
        }
    }
}
