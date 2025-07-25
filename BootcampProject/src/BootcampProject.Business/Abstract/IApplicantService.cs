using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Applicant;

namespace BootcampProject.Business.Abstract
{
    public interface IApplicantService
    {
        Task<List<ApplicantResponseDto>> GetAllAsync();
        Task<ApplicantResponseDto?> GetByIdAsync(Guid id);
        Task<ApplicantResponseDto> CreateAsync(ApplicantCreateRequestDto dto);
        Task<ApplicantResponseDto> UpdateAsync(ApplicantUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}