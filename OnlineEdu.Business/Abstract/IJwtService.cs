using OnlineEdu.Entity.Entities;
using OnlineEdu.DTO.DTOs.LoginDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Business.Abstract
{
    public interface IJwtService 
    { 
        Task<LoginResponseDto> CreateTokenAsync(AppUser user);
    }
}
