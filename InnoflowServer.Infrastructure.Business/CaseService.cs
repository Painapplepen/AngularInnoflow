using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Interfaces;
using InnoflowServer.Services.Interfaces.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Infrastructure.Business
{
    public class CaseService : ICaseService
    {
        private IUnitOfWork _uow { get; set; }
        
        public CaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Case>> GetAllCasesAsync(string email)
        {
            var userok = await _uow.Users.FindByEmailAsync(email);
            var jobs = await _uow.JobCategories.GetAllAsync();
            return (await _uow.Cases.GetAllAsync(email))
                .Where(c => c.CaseAccepted == true && c.UserId == userok.Id 
                || jobs.FirstOrDefault(j => j.Id == c.JobCategorieId) != null);
        }

        public async Task CreateAsync(Case caseTamplete)
        {
            await _uow.Cases.CreateAsync(caseTamplete);
        }

        public async Task<bool> UpdateAsync(int id, string userId) 
        {
            var result = await _uow.Cases.GetAsync(id);

            if(!result.CaseAccepted)
            {
                return false;
            }

            result = new Case()
            {
                CaseAccepted = true,
                UserId = userId
            };
            await _uow.Cases.UpdateAsync(result);
            return true;
        }
    }
}
