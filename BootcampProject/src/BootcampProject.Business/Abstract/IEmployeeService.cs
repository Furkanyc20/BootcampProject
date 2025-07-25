using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Employee;

namespace BootcampProject.Business.Abstract
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto?> GetByIdAsync(Guid id);
        Task<EmployeeResponseDto> CreateAsync(EmployeeCreateRequestDto dto);
        Task<EmployeeResponseDto> UpdateAsync(EmployeeUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}