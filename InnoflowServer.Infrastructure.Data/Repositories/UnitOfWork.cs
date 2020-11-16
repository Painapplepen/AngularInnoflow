using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<User> _signInManager;
        public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public UserManager<User> Users
        {
            get
            {
                return _userManager;
            }
        }

        public RoleManager<IdentityRole> Roles
        {
            get
            {
                return _roleManager;
            }
        }

        public SignInManager<User> SignIn
        {
            get
            {
                return _signInManager;
            }
        }
    }
}
