using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineEdu.DataAccess.Context;
using OnlineEdu.Entity.Entities;
using OnlineEdu.WebUI.DTOs.UserDtos;
using OnlineEdu.WebUI.Models;
using OnlineEdu.WebUI.Services.UserServices;

namespace OnlineEdu.WebUI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }

        public async Task<bool> AssignRoleAsync(List<AssignRoleDto> assignRoleDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRoleAsync(UserRoleDto userRoleDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegisterDto userRegisterDto)
        {
            var user = new AppUser
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email
            };
            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            {
                return new IdentityResult();
            }
            var result = await userManager.CreateAsync(user, userRegisterDto.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Student");
                return result;
            }
            return result;
        }

        public async Task<List<ResultUserDto>> Get4Teachers()
        {
            //var teacherList = await userManager.GetUsersInRoleAsync("Teacher");
            //// var teacherSocials = teacherList.Where(x=>x.TeacherSocials.Any()).ToList();
            //var values = teacherList.Take(4).ToList();
            //return mapper.Map<List<ResultUserDto>>(teacherList);

            var users = await userManager.Users.Include(x=>x.TeacherSocials).ToListAsync();
             var teachers = users.Where(user =>userManager.IsInRoleAsync(user,"Teacher").Wait(2000)).OrderByDescending(x=>x.Id).Take(4).ToList();
            return mapper.Map<List<ResultUserDto>>(teachers);

        }

        public async Task<List<ResultUserDto>> GetAllTeachers()
        {
            var users = await userManager.Users.Include(x=>x.TeacherSocials).ToListAsync();
            var teachers = users.Where(user => userManager.IsInRoleAsync(user, "Teacher").Result).ToList();
            return mapper.Map<List<ResultUserDto>>(teachers);
        }

        public async Task<List<UserViewModel>> GetAllUserAsync()
        {
            return await _client.GetFromJsonAsync<List<UserViewModel>>("roleAssigns");
        }

        public async Task<int> GetTeacherCount()
        {
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            return teachers.Count();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AssignRoleDto>> GetUserForRoleAssign(int id)
        {
            return await _client.GetFromJsonAsync<List<AssignRoleDto>>("roleAssigns/" + id);
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                return null;
            }

            var result = await signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            else
            {
                var IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
                if (IsAdmin) { return "Admin"; }
                var IsTeacher = await userManager.IsInRoleAsync(user, "Teacher");
                if (IsTeacher) { return "Teacher"; }
                var IsStudent = await userManager.IsInRoleAsync(user, "Student");
                if (IsStudent) { return "Student"; }
            }

            return null;


        }
        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
