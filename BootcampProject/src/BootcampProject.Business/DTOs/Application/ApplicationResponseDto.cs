using System;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.DTOs.Application
{
    public class ApplicationResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid BootcampId { get; set; }
        public ApplicationState ApplicationState { get; set; }
        public required string ApplicantFirstName { get; set; }
        public required string ApplicantLastName { get; set; }
        public required string BootcampName { get; set; }
    }
}