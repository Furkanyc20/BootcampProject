using System;

namespace BootcampProject.Business.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string NationalityIdentity { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserType { get; set; }
        public string? About { get; set; }
        public string? CompanyName { get; set; }
        public string? Position { get; set; }
    }
}