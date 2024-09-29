using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineEdu.Entity.Entities;
using OnlineEdu.WebUI.DTOs.RoleDtos;

namespace OnlineEdu.WebUI.Services.RoleServices
{
    public class RoleService(RoleManager<AppRole> roleManager, IMapper mapper) : IRoleService
    {
        public async Task CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var role = mapper.Map<AppRole>(createRoleDto);

            await roleManager.CreateAsync(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
             var value =  await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            await roleManager.DeleteAsync(value);
        }

        public async Task<List<ResultRoleDto>> GetAllRolesAsync()
        {
            var values = await roleManager.Roles.ToListAsync();
            return mapper.Map<List<ResultRoleDto>>(values);
        }

        public async Task<UpdateRoleDto> GetRoleByIdAsync(int id)
        {
             var value =  await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<UpdateRoleDto>(value);

        }
    }
}
