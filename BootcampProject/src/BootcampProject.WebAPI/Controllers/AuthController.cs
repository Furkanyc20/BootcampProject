using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Auth;

namespace BootcampProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(dto);
                
                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(dto);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(Login), new { email = result.Email }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty
                });
            }
        }

        [HttpPost("validate-token")]
        public async Task<ActionResult> ValidateToken([FromBody] string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Token is required.");
                }

                // This would typically involve JWT validation
                // For now, we'll return a simple response
                return Ok(new { isValid = !string.IsNullOrWhiteSpace(token), message = "Token validation endpoint" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return BadRequest("Refresh token is required.");
                }

                // This would typically involve refresh token logic
                // For now, we'll return a placeholder response
                return Ok(new AuthResponseDto
                {
                    Success = false,
                    Message = "Refresh token functionality not yet implemented.",
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty
                });
            }
        }
    }
}