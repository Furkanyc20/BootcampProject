using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Application;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationResponseDto>>> GetAll()
        {
            try
            {
                var applications = await _applicationService.GetAllAsync();
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationResponseDto>> GetById(Guid id)
        {
            try
            {
                var application = await _applicationService.GetByIdAsync(id);
                if (application == null)
                {
                    return NotFound($"Application with ID {id} not found.");
                }
                return Ok(application);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationResponseDto>> Create([FromBody] ApplicationCreateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdApplication = await _applicationService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdApplication.Id }, createdApplication);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApplicationResponseDto>> Update([FromBody] ApplicationUpdateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedApplication = await _applicationService.UpdateAsync(dto);
                return Ok(updatedApplication);
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
                await _applicationService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}