using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public class BootcampRepository : BaseRepository<Bootcamp>, IBootcampRepository
    {
        public BootcampRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}