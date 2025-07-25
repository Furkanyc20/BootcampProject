using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Applicant;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicantResponseDto>>> GetAll()
        {
            try
            {
                var applicants = await _applicantService.GetAllAsync();
                return Ok(applicants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicantResponseDto>> GetById(Guid id)
        {
            try
            {
                var applicant = await _applicantService.GetByIdAsync(id);
                if (applicant == null)
                {
                    return NotFound($"Applicant with ID {id} not found.");
                }
                return Ok(applicant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApplicantResponseDto>> Create([FromBody] ApplicantCreateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdApplicant = await _applicantService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdApplicant.Id }, createdApplicant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApplicantResponseDto>> Update([FromBody] ApplicantUpdateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedApplicant = await _applicantService.UpdateAsync(dto);
                return Ok(updatedApplicant);
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
                await _applicantService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}