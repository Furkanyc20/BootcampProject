using System;

namespace BootcampProject.Business.DTOs.Employee
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalityIdentity { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}