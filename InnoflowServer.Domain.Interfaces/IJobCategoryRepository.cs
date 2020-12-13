using InnoflowServer.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Interfaces
{
    public interface IJobCategoryRepository
    {
        Task CreateAsync(JobCategory jobCategorie);

        Task<IEnumerable<JobCategory>> GetAllAsync();

        Task<JobCategory> GetAsync(int id);
    }
}
