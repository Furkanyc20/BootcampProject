using System;

namespace BootcampProject.Business.DTOs.Auth
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public Guid? UserId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserType { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}