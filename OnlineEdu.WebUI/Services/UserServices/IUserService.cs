﻿using Microsoft.AspNetCore.Identity;
using OnlineEdu.Entity.Entities;
using OnlineEdu.WebUI.DTOs.UserDtos;

namespace OnlineEdu.WebUI.Services.UserDtos
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserRegisterDto userRegisterDto);

        Task<string> LoginAsync(UserLoginDto userLoginDto);

        Task<bool> LogoutAsync();

        Task<bool> CreateRoleAsync(UserRoleDto userRoleDto);

        Task<bool> AssignRoleAsync(List<AssignRoleDto> assignRoleDto);

        Task<List<AppUser>> GetAllUserAsync();

        Task<AppUser> GetUserByIdAsync(int id);
    }
}
