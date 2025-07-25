using System;
using System.Collections.Generic;
using BootcampProject.Entities.Abstract;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Entities.Concrete
{
    public class Bootcamp : BaseEntity
    {
        public string Name { get; set; }
        public Guid InstructorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BootcampState BootcampState { get; set; }
        
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}