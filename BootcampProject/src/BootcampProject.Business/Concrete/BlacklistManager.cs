using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Blacklist;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Business.Concrete
{
    public class BlacklistManager : IBlacklistService
    {
        private readonly IBlacklistRepository _blacklistRepository;
        private readonly IMapper _mapper;
        private readonly BlacklistBusinessRules _businessRules;

        public BlacklistManager(IBlacklistRepository blacklistRepository, IMapper mapper, BlacklistBusinessRules businessRules)
        {
            _blacklistRepository = blacklistRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<List<BlacklistResponseDto>> GetAllAsync()
        {
            var blacklists = await _blacklistRepository.GetAllAsync();
            return _mapper.Map<List<BlacklistResponseDto>>(blacklists);
        }

        public async Task<BlacklistResponseDto?> GetByIdAsync(Guid id)
        {
            var blacklist = await _blacklistRepository.GetByIdAsync(id);
            if (blacklist == null) return null;

            return _mapper.Map<BlacklistResponseDto>(blacklist);
        }

        public async Task<BlacklistResponseDto> CreateAsync(BlacklistCreateRequestDto dto)
        {
            await _businessRules.CheckIfReasonNotEmptyAsync(dto.Reason);
            await _businessRules.CheckIfApplicantNotAlreadyInBlacklistAsync(dto.ApplicantId);

            var blacklist = _mapper.Map<Blacklist>(dto);
            blacklist.Id = Guid.NewGuid();

            var createdBlacklist = await _blacklistRepository.AddAsync(blacklist);
            await _blacklistRepository.SaveChangesAsync();

            return _mapper.Map<BlacklistResponseDto>(createdBlacklist);
        }

        public async Task<BlacklistResponseDto> UpdateAsync(BlacklistUpdateRequestDto dto)
        {
            await _businessRules.CheckIfReasonNotEmptyAsync(dto.Reason);

            var existingBlacklist = await _blacklistRepository.GetByIdAsync(dto.Id);
            if (existingBlacklist == null)
                throw new ArgumentException($"Blacklist with ID {dto.Id} not found.");

            _mapper.Map(dto, existingBlacklist);

            var updatedBlacklist = await _blacklistRepository.UpdateAsync(existingBlacklist);
            await _blacklistRepository.SaveChangesAsync();

            return _mapper.Map<BlacklistResponseDto>(updatedBlacklist);
        }

        public async Task DeleteAsync(Guid id)
        {
            var blacklist = await _blacklistRepository.GetByIdAsync(id);
            if (blacklist == null)
                throw new ArgumentException($"Blacklist with ID {id} not found.");

            await _blacklistRepository.DeleteAsync(blacklist);
            await _blacklistRepository.SaveChangesAsync();
        }
    }
}