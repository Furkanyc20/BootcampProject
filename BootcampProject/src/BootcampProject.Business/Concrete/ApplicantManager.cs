using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Applicant;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Core.Utilities;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class ApplicantManager : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;
        private readonly ApplicantBusinessRules _businessRules;

        public ApplicantManager(IApplicantRepository applicantRepository, IMapper mapper, ApplicantBusinessRules businessRules)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<List<ApplicantResponseDto>> GetAllAsync()
        {
            var applicants = await _applicantRepository.GetAllAsync();
            return _mapper.Map<List<ApplicantResponseDto>>(applicants);
        }

        public async Task<ApplicantResponseDto?> GetByIdAsync(Guid id)
        {
            var applicant = await _applicantRepository.GetByIdAsync(id);
            if (applicant == null) return null;

            return _mapper.Map<ApplicantResponseDto>(applicant);
        }

        public async Task<ApplicantResponseDto> CreateAsync(ApplicantCreateRequestDto dto)
        {
            await _businessRules.CheckIfNationalityIdentityNotDuplicateAsync(dto.NationalityIdentity);

            var applicant = _mapper.Map<Applicant>(dto);
            applicant.Id = Guid.NewGuid();
            applicant.PasswordHash = HashPassword(dto.Password);
            applicant.PasswordSalt = GenerateSalt();

            var createdApplicant = await _applicantRepository.AddAsync(applicant);
            await _applicantRepository.SaveChangesAsync();

            return _mapper.Map<ApplicantResponseDto>(createdApplicant);
        }

        public async Task<ApplicantResponseDto> UpdateAsync(ApplicantUpdateRequestDto dto)
        {
            await _businessRules.CheckIfApplicantExistsAsync(dto.Id);

            var existingApplicant = await _applicantRepository.GetByIdAsync(dto.Id);
            if (existingApplicant == null)
                throw new ArgumentException($"Applicant with ID {dto.Id} not found.");

            var applicantsWithSameNationalityId = await _applicantRepository.GetAllAsync();
            var duplicateApplicant = applicantsWithSameNationalityId.FirstOrDefault(a => a.NationalityIdentity == dto.NationalityIdentity && a.Id != dto.Id);
            if (duplicateApplicant != null)
            {
                throw new InvalidOperationException($"An applicant with nationality identity {dto.NationalityIdentity} already exists.");
            }

            _mapper.Map(dto, existingApplicant);

            var updatedApplicant = await _applicantRepository.UpdateAsync(existingApplicant);
            await _applicantRepository.SaveChangesAsync();

            return _mapper.Map<ApplicantResponseDto>(updatedApplicant);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _businessRules.CheckIfApplicantExistsAsync(id);

            var applicant = await _applicantRepository.GetByIdAsync(id);
            if (applicant == null)
                throw new ArgumentException($"Applicant with ID {id} not found.");

            await _applicantRepository.DeleteAsync(applicant);
            await _applicantRepository.SaveChangesAsync();
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