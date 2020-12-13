using InnoflowServer.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Interfaces
{
    public interface ICaseRepository
    {
        Task CreateAsync(Case caseTemplate);

        Task<IEnumerable<Case>> GetAllAsync(string email);

        Task UpdateAsync(Case caseTemplate);

        Task<Case> GetAsync(int id);
    }
}
