using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Instructor;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InstructorResponseDto>>> GetAll()
        {
            try
            {
                var instructors = await _instructorService.GetAllAsync();
                return Ok(instructors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorResponseDto>> GetById(Guid id)
        {
            try
            {
                var instructor = await _instructorService.GetByIdAsync(id);
                if (instructor == null)
                {
                    return NotFound($"Instructor with ID {id} not found.");
                }
                return Ok(instructor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<InstructorResponseDto>> Create([FromBody] InstructorCreateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdInstructor = await _instructorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdInstructor.Id }, createdInstructor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<InstructorResponseDto>> Update([FromBody] InstructorUpdateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedInstructor = await _instructorService.UpdateAsync(dto);
                return Ok(updatedInstructor);
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
                await _instructorService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}