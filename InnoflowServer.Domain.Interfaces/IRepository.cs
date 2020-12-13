using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(string email);

        Task CreateAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task<bool> DeleteAsync(string email);
        
    }
}
