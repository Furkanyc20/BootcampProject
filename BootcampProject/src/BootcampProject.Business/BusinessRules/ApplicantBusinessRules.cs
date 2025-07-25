using System.Linq;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Business.BusinessRules
{
    public class ApplicantBusinessRules
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IBlacklistRepository _blacklistRepository;

        public ApplicantBusinessRules(IApplicantRepository applicantRepository, IBlacklistRepository blacklistRepository)
        {
            _applicantRepository = applicantRepository;
            _blacklistRepository = blacklistRepository;
        }

        public async Task CheckIfApplicantExistsAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                throw new InvalidOperationException($"Applicant with ID {applicantId} does not exist.");
            }
        }

        public async Task CheckIfApplicantNotInBlacklistAsync(Guid applicantId)
        {
            var blacklistEntries = await _blacklistRepository.GetAllAsync();
            var isBlacklisted = blacklistEntries.Any(b => b.ApplicantId == applicantId);
            
            if (isBlacklisted)
            {
                throw new InvalidOperationException($"Applicant with ID {applicantId} is blacklisted and cannot perform this action.");
            }
        }

        public async Task CheckIfNationalityIdentityNotDuplicateAsync(string nationalityIdentity)
        {
            var applicants = await _applicantRepository.GetAllAsync();
            var existingApplicant = applicants.FirstOrDefault(a => a.NationalityIdentity == nationalityIdentity);
            
            if (existingApplicant != null)
            {
                throw new InvalidOperationException($"An applicant with nationality identity {nationalityIdentity} already exists.");
            }
        }
    }
}