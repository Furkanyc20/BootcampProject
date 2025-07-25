using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Application;

namespace BootcampProject.Business.Abstract
{
    public interface IApplicationService
    {
        Task<List<ApplicationResponseDto>> GetAllAsync();
        Task<ApplicationResponseDto?> GetByIdAsync(Guid id);
        Task<ApplicationResponseDto> CreateAsync(ApplicationCreateRequestDto dto);
        Task<ApplicationResponseDto> UpdateAsync(ApplicationUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}