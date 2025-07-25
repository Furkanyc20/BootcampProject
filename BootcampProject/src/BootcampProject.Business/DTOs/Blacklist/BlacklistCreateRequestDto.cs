using System;

namespace BootcampProject.Business.DTOs.Blacklist
{
    public class BlacklistCreateRequestDto
    {
        public required string Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid ApplicantId { get; set; }
    }
}