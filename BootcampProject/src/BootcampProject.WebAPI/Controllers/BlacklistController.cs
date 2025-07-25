using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Blacklist;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _blacklistService;

        public BlacklistController(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlacklistResponseDto>>> GetAll()
        {
            try
            {
                var blacklists = await _blacklistService.GetAllAsync();
                return Ok(blacklists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlacklistResponseDto>> GetById(Guid id)
        {
            try
            {
                var blacklist = await _blacklistService.GetByIdAsync(id);
                if (blacklist == null)
                {
                    return NotFound($"Blacklist entry with ID {id} not found.");
                }
                return Ok(blacklist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BlacklistResponseDto>> Create([FromBody] BlacklistCreateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBlacklist = await _blacklistService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdBlacklist.Id }, createdBlacklist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<BlacklistResponseDto>> Update([FromBody] BlacklistUpdateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedBlacklist = await _blacklistService.UpdateAsync(dto);
                return Ok(updatedBlacklist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _blacklistService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}