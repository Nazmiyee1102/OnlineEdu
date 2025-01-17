﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineEdu.Business.Abstract;
using OnlineEdu.DataAccess.Context;
using OnlineEdu.Entity.Entities;
using OnlineEdu.DTO.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineEdu.Business.Concrete
{
    public class UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole>
        roleManager, IMapper mapper, OnlineEduContext _context) : IUserService
    {
        public async Task<bool> AssignRoleAsync(List<AssignRoleDto> assignRoleDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRoleAsync(UserRoleDto userRoleDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto userRegisterDto)
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

            var users = await userManager.Users.Include(x => x.TeacherSocials).ToListAsync();
            var teachers = users.Where(user => userManager.IsInRoleAsync(user, "Teacher").Wait(2000)).OrderByDescending(x => x.Id).Take(4).ToList();
            return mapper.Map<List<ResultUserDto>>(teachers);
        }

        public async Task<List<ResultUserDto>> GetAllTeachers()
        {
            var users = await userManager.Users.Include(x => x.TeacherSocials).ToListAsync();
            var teachers = users.Where(user => userManager.IsInRoleAsync(user, "Teacher").Result).ToList();
            return mapper.Map<List<ResultUserDto>>(teachers);
        }

        public async Task<List<AppUser>> GetAllUserAsync()
        {
            return await userManager.Users.ToListAsync();
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

        public async Task<string> LoginAsync(LoginDto userLoginDto)
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
