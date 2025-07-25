using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Blacklist;

namespace BootcampProject.Business.Abstract
{
    public interface IBlacklistService
    {
        Task<List<BlacklistResponseDto>> GetAllAsync();
        Task<BlacklistResponseDto?> GetByIdAsync(Guid id);
        Task<BlacklistResponseDto> CreateAsync(BlacklistCreateRequestDto dto);
        Task<BlacklistResponseDto> UpdateAsync(BlacklistUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}