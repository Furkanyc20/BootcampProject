using Microsoft.EntityFrameworkCore;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;

namespace BootcampProject.Repositories.Concrete
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BootcampDbContext context) : base(context)
        {
        }
    }
}