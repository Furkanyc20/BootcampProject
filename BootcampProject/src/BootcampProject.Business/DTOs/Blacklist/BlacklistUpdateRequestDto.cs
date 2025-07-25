using System;

namespace BootcampProject.Business.DTOs.Blacklist
{
    public class BlacklistUpdateRequestDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid ApplicantId { get; set; }
    }
}