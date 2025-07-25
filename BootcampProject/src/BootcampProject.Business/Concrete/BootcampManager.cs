using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Bootcamp;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class BootcampManager : IBootcampService
    {
        private readonly IBootcampRepository _bootcampRepository;
        private readonly IMapper _mapper;
        private readonly BootcampBusinessRules _businessRules;

        public BootcampManager(IBootcampRepository bootcampRepository, IMapper mapper, BootcampBusinessRules businessRules)
        {
            _bootcampRepository = bootcampRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<List<BootcampResponseDto>> GetAllAsync()
        {
            var bootcamps = await _bootcampRepository.GetAllAsync();
            return _mapper.Map<List<BootcampResponseDto>>(bootcamps);
        }

        public async Task<BootcampResponseDto?> GetByIdAsync(Guid id)
        {
            var bootcamp = await _bootcampRepository.GetByIdAsync(id);
            if (bootcamp == null) return null;

            return _mapper.Map<BootcampResponseDto>(bootcamp);
        }

        public async Task<BootcampResponseDto> CreateAsync(BootcampCreateRequestDto dto)
        {
            await _businessRules.CheckIfBootcampNameNotDuplicateAsync(dto.Name);
            await _businessRules.CheckIfInstructorExistsAsync(dto.InstructorId);
            await _businessRules.CheckIfStartDateBeforeEndDateAsync(dto.StartDate, dto.EndDate);

            var bootcamp = _mapper.Map<Bootcamp>(dto);
            bootcamp.Id = Guid.NewGuid();

            var createdBootcamp = await _bootcampRepository.AddAsync(bootcamp);
            await _bootcampRepository.SaveChangesAsync();

            return _mapper.Map<BootcampResponseDto>(createdBootcamp);
        }

        public async Task<BootcampResponseDto> UpdateAsync(BootcampUpdateRequestDto dto)
        {
            await _businessRules.CheckIfBootcampExistsAsync(dto.Id);

            var existingBootcamp = await _bootcampRepository.GetByIdAsync(dto.Id);
            if (existingBootcamp == null)
                throw new ArgumentException($"Bootcamp with ID {dto.Id} not found.");

            var bootcamps = await _bootcampRepository.GetAllAsync();
            var duplicateBootcamp = bootcamps.FirstOrDefault(b => b.Name == dto.Name && b.Id != dto.Id);
            if (duplicateBootcamp != null)
            {
                throw new InvalidOperationException($"A bootcamp with name '{dto.Name}' already exists.");
            }

            await _businessRules.CheckIfInstructorExistsAsync(dto.InstructorId);
            await _businessRules.CheckIfStartDateBeforeEndDateAsync(dto.StartDate, dto.EndDate);

            _mapper.Map(dto, existingBootcamp);

            var updatedBootcamp = await _bootcampRepository.UpdateAsync(existingBootcamp);
            await _bootcampRepository.SaveChangesAsync();

            return _mapper.Map<BootcampResponseDto>(updatedBootcamp);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _businessRules.CheckIfBootcampExistsAsync(id);

            var bootcamp = await _bootcampRepository.GetByIdAsync(id);
            if (bootcamp == null)
                throw new ArgumentException($"Bootcamp with ID {id} not found.");

            await _bootcampRepository.DeleteAsync(bootcamp);
            await _bootcampRepository.SaveChangesAsync();
        }
    }
}