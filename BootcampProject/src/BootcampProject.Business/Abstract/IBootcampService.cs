using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Bootcamp;

namespace BootcampProject.Business.Abstract
{
    public interface IBootcampService
    {
        Task<List<BootcampResponseDto>> GetAllAsync();
        Task<BootcampResponseDto?> GetByIdAsync(Guid id);
        Task<BootcampResponseDto> CreateAsync(BootcampCreateRequestDto dto);
        Task<BootcampResponseDto> UpdateAsync(BootcampUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}