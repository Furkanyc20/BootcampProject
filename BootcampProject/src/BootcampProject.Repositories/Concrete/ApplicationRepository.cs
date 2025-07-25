using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}