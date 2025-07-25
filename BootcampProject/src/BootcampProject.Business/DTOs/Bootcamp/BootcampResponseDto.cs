using System;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.DTOs.Bootcamp
{
    public class BootcampResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InstructorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BootcampState BootcampState { get; set; }
        public string InstructorFirstName { get; set; }
        public string InstructorLastName { get; set; }
        public string InstructorCompanyName { get; set; }
    }
}