using System.Linq;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Business.BusinessRules
{
    public class BlacklistBusinessRules
    {
        private readonly IBlacklistRepository _blacklistRepository;

        public BlacklistBusinessRules(IBlacklistRepository blacklistRepository)
        {
            _blacklistRepository = blacklistRepository;
        }

        public async Task CheckIfApplicantNotAlreadyInBlacklistAsync(Guid applicantId)
        {
            var blacklistEntries = await _blacklistRepository.GetAllAsync();
            var existingEntry = blacklistEntries.FirstOrDefault(b => b.ApplicantId == applicantId);
            
            if (existingEntry != null)
            {
                throw new InvalidOperationException($"Applicant with ID {applicantId} is already in the blacklist.");
            }
        }

        public async Task CheckIfReasonNotEmptyAsync(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Blacklist reason cannot be null or empty.", nameof(reason));
            }
            await Task.CompletedTask;
        }
    }
}