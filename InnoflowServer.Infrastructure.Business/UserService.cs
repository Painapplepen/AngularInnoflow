using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Interfaces;
using InnoflowServer.Services.Interfaces.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InnoflowServer.Infrastructure.Business.Users
{
    public class UserService : IUserService
    {
        private IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<string> Register(UserDTO model)
        {
            var user = await db.Users.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return null;
            }

            user = new User { Email = model.Email, UserName = "User", FirstName = model.FirstName, LastName = model.LastName };
            var checkAdd = await db.Users.CreateAsync(user, model.Password);

            if (!checkAdd.Succeeded)
            {
                return null;
            }

            var code = await db.Users.GenerateEmailConfirmationTokenAsync(user);
            
            var userRole = await db.Roles.FindByNameAsync("CaseDevoleper");

            if (userRole == null)
            {
                await db.Roles.CreateAsync(new IdentityRole("CaseDevoleper"));
            }
            
            await db.Users.AddToRoleAsync(user, userRole.Name);

            return code;

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

                if (!await db.Users.IsEmailConfirmedAsync(user))
                {
                    return false;
                }

                await db.SignIn.SignInAsync(user, true);

                return true;
            }
            
            return false;
        }

        public async Task<bool> SendEmail(UserDTO model, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Innoflow-admin", "sanin17619@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", model.Email));
                emailMessage.Subject = "Подтвердите свою почту";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("sanin17619@gmail.com", "kctoszrouldiouuo");
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при отправке почты" + e.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> ConfirmEmail(string userEmail, string code)
        {
            if (userEmail == null || code == null)
            {
                return false;
            }
            
            var user = await db.Users.FindByEmailAsync(userEmail);
            
            if (user == null)
            {
                return false;
            }
            
            var result = await db.Users.ConfirmEmailAsync(user, code);
            
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task Logout()
        {
            await db.SignIn.SignOutAsync();
        }
    }
}
