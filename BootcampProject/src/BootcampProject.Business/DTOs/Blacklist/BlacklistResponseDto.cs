using System;

namespace BootcampProject.Business.DTOs.Blacklist
{
    public class BlacklistResponseDto
    {
        public Guid Id { get; set; }
        public required string Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid ApplicantId { get; set; }
        public required string ApplicantFirstName { get; set; }
        public required string ApplicantLastName { get; set; }
        public required string ApplicantEmail { get; set; }
    }
}