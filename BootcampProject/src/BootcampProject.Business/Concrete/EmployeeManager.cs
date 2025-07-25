using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Employee;
using BootcampProject.Core.Utilities;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeManager(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<EmployeeResponseDto> CreateAsync(EmployeeCreateRequestDto dto)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var duplicateEmployee = employees.FirstOrDefault(e => e.NationalityIdentity == dto.NationalityIdentity);
            if (duplicateEmployee != null)
            {
                throw new InvalidOperationException($"An employee with nationality identity {dto.NationalityIdentity} already exists.");
            }

            var employee = _mapper.Map<Employee>(dto);
            employee.Id = Guid.NewGuid();
            employee.PasswordHash = HashPassword(dto.Password);
            employee.PasswordSalt = GenerateSalt();

            var createdEmployee = await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(createdEmployee);
        }

        public async Task<EmployeeResponseDto> UpdateAsync(EmployeeUpdateRequestDto dto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(dto.Id);
            if (existingEmployee == null)
                throw new ArgumentException($"Employee with ID {dto.Id} not found.");

            var employees = await _employeeRepository.GetAllAsync();
            var duplicateEmployee = employees.FirstOrDefault(e => e.NationalityIdentity == dto.NationalityIdentity && e.Id != dto.Id);
            if (duplicateEmployee != null)
            {
                throw new InvalidOperationException($"An employee with nationality identity {dto.NationalityIdentity} already exists.");
            }

            _mapper.Map(dto, existingEmployee);

            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);
            await _employeeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(updatedEmployee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException($"Employee with ID {id} not found.");

            await _employeeRepository.DeleteAsync(employee);
            await _employeeRepository.SaveChangesAsync();
        }

        private string HashPassword(string password)
        {
            var salt = HashingHelper.GenerateSalt();
            return HashingHelper.CreateSHA256Hash(password, salt);
        }

        private string GenerateSalt()
        {
            return HashingHelper.GenerateSalt();
        }
    }
}