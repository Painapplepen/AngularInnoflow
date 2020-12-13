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
    public class CaseRepository : ICaseRepository
    {
        private readonly ApplicationDbContext _context;
        public CaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Case>> GetAllAsync(string email)
        {
            return await _context.Cases.AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(Case caseTemplate)
        {
            await _context.Cases.AddAsync(caseTemplate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Case caseTemplate)
        {
            _context.Entry(caseTemplate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Case> GetAsync(int id)
        {
            return await _context.Cases.FindAsync(id);
        }
    }
}
