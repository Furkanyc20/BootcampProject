using System;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.DTOs.Bootcamp
{
    public class BootcampCreateRequestDto
    {
        public string Name { get; set; }
        public Guid InstructorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BootcampState BootcampState { get; set; }
    }
}