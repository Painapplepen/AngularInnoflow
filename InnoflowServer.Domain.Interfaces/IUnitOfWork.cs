using InnoflowServer.Domain.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<User> Users { get; }
        RoleManager<IdentityRole> Roles { get; }
        SignInManager<User> SignIn { get; }
        ICaseRepository Cases { get; }
        IJobCategoryRepository JobCategories { get; }
    }
}
