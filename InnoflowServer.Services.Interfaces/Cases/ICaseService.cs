using InnoflowServer.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Services.Interfaces.Cases
{
    public interface ICaseService
    {
        Task CreateAsync(Case caseTemplate);

        Task<IEnumerable<Case>> GetAllCasesAsync(string email);

        Task<bool> UpdateAsync(int id, string userId);
    }
}
