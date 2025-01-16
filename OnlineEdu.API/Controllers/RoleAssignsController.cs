using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SharePoint.Client;
using OnlineEdu.Business.Abstract;
using OnlineEdu.DTO.DTOs.UserDtos;
using OnlineEdu.Entity.Entities;
using System.Security.Claims;

namespace OnlineEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAssignsController(IUserService userService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IHttpContextAccessor _contextAccessor) : ControllerBase
    {
        
        public async Task<IActionResult> Index()
        {
            var values = await userService.GetAllUserAsync();
            var userList = (from user in values
                            select new UserListDto
                            {
                                UserViewModelId = user.Id,
                                UserName = user.UserName,
                                NameSurname = user.FirstName + " " + user.LastName,
                                Roles = userManager.GetRolesAsync(user).Result.ToList(),
                            }).ToList();
            return Ok(userList);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserForRoleAssign(int id)
        {
            var user = await userService.GetUserByIdAsync(id);

            var roles = await roleManager.Roles.ToListAsync();

            var userRoles = await userManager.GetRolesAsync(user);

            List<AssignRoleDto> assignRoleList = new List<AssignRoleDto>();

            foreach (var role in roles)
            {
                var assignRole = new AssignRoleDto();

                assignRole.RoleId = role.Id;
                assignRole.RoleName = role.Name;
                assignRole.RoleExist = userRoles.Contains(role.Name);

                assignRoleList.Add(assignRole);
            }
            return Ok(assignRoleList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<AssignRoleDto> assignRoleList)
        {
            Int128 userId = int.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await userService.GetUserByIdAsync(userId);

            foreach (var item in assignRoleList)
            {
                if (item.RoleExist)
                {
                    await userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, item.RoleName);
                }

            }
            return Ok("Rol Atama Başarılı");
        }
    }
}
