using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Interfaces;
using InnoflowServer.Services.Interfaces.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace InnoflowServer.Infrastructure.Business.Users
{
    public class UserService : IUserService
    { 
        private IUnitOfWork db { get; set; }
        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<bool> Register(UserDTO model)
        {
            var user = await db.Users.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return false;
            }

            user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName};
            var checkAdd = await db.Users.CreateAsync(user, model.Password);

            if (!checkAdd.Succeeded)
            {
                return false;
            }

            var userRole = await db.Roles.FindByNameAsync("CaseDevoleper");
            
            if (userRole == null)
            {
                await db.Roles.CreateAsync(new IdentityRole("CaseDevoleper"));
            }
            await db.Users.AddToRoleAsync(user, userRole.Name);

            await db.SignIn.SignInAsync(user, true);
            return true;

        }

        public async Task<bool> Login(UserDTO model)
        {
            var user = await db.Users.FindByEmailAsync(model.Email);
            
            if (user != null)
            {
                var checkPassword = await db.SignIn.CheckPasswordSignInAsync(user, model.Password, false);
                
                if (!checkPassword.Succeeded)
                {
                    return false;
                }

                await db.SignIn.SignInAsync(user, true);
                return true;
            }
            
            return false;
        }

        public async Task Logout()
        {
            await db.SignIn.SignOutAsync();
        }
    }
}
