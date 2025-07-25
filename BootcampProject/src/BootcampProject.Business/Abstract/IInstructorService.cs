using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Instructor;

namespace BootcampProject.Business.Abstract
{
    public interface IInstructorService
    {
        Task<List<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto?> GetByIdAsync(Guid id);
        Task<InstructorResponseDto> CreateAsync(InstructorCreateRequestDto dto);
        Task<InstructorResponseDto> UpdateAsync(InstructorUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}