using System;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.DTOs.Application
{
    public class ApplicationCreateRequestDto
    {
        public Guid ApplicantId { get; set; }
        public Guid BootcampId { get; set; }
        public ApplicationState ApplicationState { get; set; }
    }
}