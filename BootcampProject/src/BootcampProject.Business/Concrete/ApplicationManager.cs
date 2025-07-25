using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Application;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class ApplicationManager : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationBusinessRules _businessRules;

        public ApplicationManager(IApplicationRepository applicationRepository, IMapper mapper, ApplicationBusinessRules businessRules)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<List<ApplicationResponseDto>> GetAllAsync()
        {
            var applications = await _applicationRepository.GetAllAsync();
            return _mapper.Map<List<ApplicationResponseDto>>(applications);
        }

        public async Task<ApplicationResponseDto?> GetByIdAsync(Guid id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null) return null;

            return _mapper.Map<ApplicationResponseDto>(application);
        }

        public async Task<ApplicationResponseDto> CreateAsync(ApplicationCreateRequestDto dto)
        {
            await _businessRules.CheckIfApplicantNotAppliedToBootcampAsync(dto.ApplicantId, dto.BootcampId);
            await _businessRules.CheckIfBootcampActiveAsync(dto.BootcampId);
            await _businessRules.CheckIfApplicantNotInBlacklistAsync(dto.ApplicantId);

            var application = _mapper.Map<Application>(dto);
            application.Id = Guid.NewGuid();

            var createdApplication = await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return _mapper.Map<ApplicationResponseDto>(createdApplication);
        }

        public async Task<ApplicationResponseDto> UpdateAsync(ApplicationUpdateRequestDto dto)
        {
            var existingApplication = await _applicationRepository.GetByIdAsync(dto.Id);
            if (existingApplication == null)
                throw new ArgumentException($"Application with ID {dto.Id} not found.");

            await _businessRules.CheckIfApplicationStatusTransitionValidAsync(existingApplication.ApplicationState, dto.ApplicationState);

            _mapper.Map(dto, existingApplication);

            var updatedApplication = await _applicationRepository.UpdateAsync(existingApplication);
            await _applicationRepository.SaveChangesAsync();

            return _mapper.Map<ApplicationResponseDto>(updatedApplication);
        }

        public async Task DeleteAsync(Guid id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null)
                throw new ArgumentException($"Application with ID {id} not found.");

            await _applicationRepository.DeleteAsync(application);
            await _applicationRepository.SaveChangesAsync();
        }
    }
}