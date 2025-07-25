using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public class InstructorRepository : BaseRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}