using System;

namespace BootcampProject.Business.DTOs.Instructor
{
    public class InstructorUpdateRequestDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string NationalityIdentity { get; set; }
        public required string Email { get; set; }
        public required string CompanyName { get; set; }
    }
}