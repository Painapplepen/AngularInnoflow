using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Infrastructure.Data.Repositories
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public JobCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(JobCategory jobCategorie)
        {
            await _context.JobCategories.AddAsync(jobCategorie);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobCategory>> GetAllAsync()
        {
            return await _context.JobCategories.AsNoTracking().ToListAsync();
        }
        

        public async Task<JobCategory> GetAsync(int id)
        {
            return await _context.JobCategories.FindAsync(id);
        }

    }
}
