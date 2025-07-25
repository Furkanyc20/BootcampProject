using System;

namespace BootcampProject.Business.DTOs.Instructor
{
    public class InstructorResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalityIdentity { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
    }
}