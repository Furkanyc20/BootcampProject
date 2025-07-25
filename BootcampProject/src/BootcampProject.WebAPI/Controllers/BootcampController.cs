using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Bootcamp;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BootcampController : ControllerBase
    {
        private readonly IBootcampService _bootcampService;

        public BootcampController(IBootcampService bootcampService)
        {
            _bootcampService = bootcampService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BootcampResponseDto>>> GetAll()
        {
            try
            {
                var bootcamps = await _bootcampService.GetAllAsync();
                return Ok(bootcamps);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BootcampResponseDto>> GetById(Guid id)
        {
            try
            {
                var bootcamp = await _bootcampService.GetByIdAsync(id);
                if (bootcamp == null)
                {
                    return NotFound($"Bootcamp with ID {id} not found.");
                }
                return Ok(bootcamp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BootcampResponseDto>> Create([FromBody] BootcampCreateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBootcamp = await _bootcampService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdBootcamp.Id }, createdBootcamp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<BootcampResponseDto>> Update([FromBody] BootcampUpdateRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedBootcamp = await _bootcampService.UpdateAsync(dto);
                return Ok(updatedBootcamp);
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
                await _bootcampService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}