using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public abstract class UserRepository : BaseRepository<User>, IUserRepository
    {
        protected UserRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}