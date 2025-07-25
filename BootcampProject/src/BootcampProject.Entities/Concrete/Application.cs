using System;
using BootcampProject.Entities.Abstract;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Entities.Concrete
{
    public class Application : BaseEntity
    {
        public Guid ApplicantId { get; set; }
        public Guid BootcampId { get; set; }
        public ApplicationState ApplicationState { get; set; }
        
        public virtual Applicant Applicant { get; set; }
        public virtual Bootcamp Bootcamp { get; set; }
    }
}