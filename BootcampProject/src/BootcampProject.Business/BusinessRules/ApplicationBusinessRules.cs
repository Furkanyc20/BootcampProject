using System.Linq;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Entities.Concrete;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.BusinessRules
{
    public class ApplicationBusinessRules
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IBootcampRepository _bootcampRepository;
        private readonly IBlacklistRepository _blacklistRepository;

        public ApplicationBusinessRules(IApplicationRepository applicationRepository, IBootcampRepository bootcampRepository, IBlacklistRepository blacklistRepository)
        {
            _applicationRepository = applicationRepository;
            _bootcampRepository = bootcampRepository;
            _blacklistRepository = blacklistRepository;
        }

        public async Task CheckIfApplicantNotAppliedToBootcampAsync(Guid applicantId, Guid bootcampId)
        {
            var applications = await _applicationRepository.GetAllAsync();
            var existingApplication = applications.FirstOrDefault(a => a.ApplicantId == applicantId && a.BootcampId == bootcampId);
            
            if (existingApplication != null)
            {
                throw new InvalidOperationException($"Applicant with ID {applicantId} has already applied to bootcamp with ID {bootcampId}.");
            }
        }

        public async Task CheckIfBootcampActiveAsync(Guid bootcampId)
        {
            var bootcamp = await _bootcampRepository.GetByIdAsync(bootcampId);
            if (bootcamp == null)
            {
                throw new InvalidOperationException($"Bootcamp with ID {bootcampId} does not exist.");
            }

            if (bootcamp.BootcampState == BootcampState.CANCELLED || bootcamp.BootcampState == BootcampState.FINISHED)
            {
                throw new InvalidOperationException($"Cannot apply to bootcamp '{bootcamp.Name}' as it is {bootcamp.BootcampState.ToString().ToLower()}.");
            }
        }

        public async Task CheckIfApplicantNotInBlacklistAsync(Guid applicantId)
        {
            var blacklistEntries = await _blacklistRepository.GetAllAsync();
            var isBlacklisted = blacklistEntries.Any(b => b.ApplicantId == applicantId);
            
            if (isBlacklisted)
            {
                throw new InvalidOperationException($"Applicant with ID {applicantId} is blacklisted and cannot apply to bootcamps.");
            }
        }

        public async Task CheckIfApplicationStatusTransitionValidAsync(ApplicationState currentState, ApplicationState newState)
        {
            var validTransitions = new Dictionary<ApplicationState, List<ApplicationState>>
            {
                [ApplicationState.PENDING] = new List<ApplicationState> { ApplicationState.IN_REVIEW, ApplicationState.CANCELLED },
                [ApplicationState.IN_REVIEW] = new List<ApplicationState> { ApplicationState.APPROVED, ApplicationState.REJECTED, ApplicationState.CANCELLED },
                [ApplicationState.APPROVED] = new List<ApplicationState> { ApplicationState.CANCELLED },
                [ApplicationState.REJECTED] = new List<ApplicationState>(),
                [ApplicationState.CANCELLED] = new List<ApplicationState>()
            };

            if (!validTransitions[currentState].Contains(newState))
            {
                throw new InvalidOperationException($"Invalid state transition from {currentState} to {newState}.");
            }
            await Task.CompletedTask;
        }
    }
}