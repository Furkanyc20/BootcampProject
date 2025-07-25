using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Instructor;
using BootcampProject.Core.Utilities;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class InstructorManager : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;

        public InstructorManager(IInstructorRepository instructorRepository, IMapper mapper)
        {
            _instructorRepository = instructorRepository;
            _mapper = mapper;
        }

        public async Task<List<InstructorResponseDto>> GetAllAsync()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return _mapper.Map<List<InstructorResponseDto>>(instructors);
        }

        public async Task<InstructorResponseDto?> GetByIdAsync(Guid id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return null;

            return _mapper.Map<InstructorResponseDto>(instructor);
        }

        public async Task<InstructorResponseDto> CreateAsync(InstructorCreateRequestDto dto)
        {
            var instructors = await _instructorRepository.GetAllAsync();
            var duplicateInstructor = instructors.FirstOrDefault(i => i.NationalityIdentity == dto.NationalityIdentity);
            if (duplicateInstructor != null)
            {
                throw new InvalidOperationException($"An instructor with nationality identity {dto.NationalityIdentity} already exists.");
            }

            var instructor = _mapper.Map<Instructor>(dto);
            instructor.Id = Guid.NewGuid();
            instructor.PasswordHash = HashPassword(dto.Password);
            instructor.PasswordSalt = GenerateSalt();

            var createdInstructor = await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return _mapper.Map<InstructorResponseDto>(createdInstructor);
        }

        public async Task<InstructorResponseDto> UpdateAsync(InstructorUpdateRequestDto dto)
        {
            var existingInstructor = await _instructorRepository.GetByIdAsync(dto.Id);
            if (existingInstructor == null)
                throw new ArgumentException($"Instructor with ID {dto.Id} not found.");

            var instructors = await _instructorRepository.GetAllAsync();
            var duplicateInstructor = instructors.FirstOrDefault(i => i.NationalityIdentity == dto.NationalityIdentity && i.Id != dto.Id);
            if (duplicateInstructor != null)
            {
                throw new InvalidOperationException($"An instructor with nationality identity {dto.NationalityIdentity} already exists.");
            }

            _mapper.Map(dto, existingInstructor);

            var updatedInstructor = await _instructorRepository.UpdateAsync(existingInstructor);
            await _instructorRepository.SaveChangesAsync();

            return _mapper.Map<InstructorResponseDto>(updatedInstructor);
        }

        public async Task DeleteAsync(Guid id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
                throw new ArgumentException($"Instructor with ID {id} not found.");

            await _instructorRepository.DeleteAsync(instructor);
            await _instructorRepository.SaveChangesAsync();
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