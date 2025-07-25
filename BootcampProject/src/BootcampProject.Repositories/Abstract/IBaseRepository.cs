using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampProject.Repositories.Abstract
{
    public interface IBaseRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}