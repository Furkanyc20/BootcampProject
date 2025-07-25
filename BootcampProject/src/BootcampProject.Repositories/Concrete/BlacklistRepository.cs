using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public class BlacklistRepository : BaseRepository<Blacklist>, IBlacklistRepository
    {
        public BlacklistRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}