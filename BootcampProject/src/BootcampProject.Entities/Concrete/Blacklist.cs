using System;
using BootcampProject.Entities.Abstract;

namespace BootcampProject.Entities.Concrete
{
    public class Blacklist : BaseEntity
    {
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid ApplicantId { get; set; }
        
        public virtual Applicant Applicant { get; set; }
    }
}