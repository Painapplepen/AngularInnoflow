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
using InnoflowServer.Domain.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

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

            user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName };
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

        public async Task<string> Login(UserDTO model)
        {
            var user = await db.Users.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var checkPassword = await db.SignIn.CheckPasswordSignInAsync(user, model.Password, false);

                if (!checkPassword.Succeeded)
                {
                    return "";
                }

                if (!await db.Users.IsEmailConfirmedAsync(user))
                {
                    return "";
                }

                await db.SignIn.SignInAsync(user, true);
                var role = await db.Users.GetRolesAsync(user);

                var now = DateTime.UtcNow;
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role[0])
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: claimsIdentity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return encodedJwt;
            }

            return "";
        }

        public async Task<bool> SendEmail(UserDTO model, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Innoflow-admin", "innoflowa@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", model.Email));
                emailMessage.Subject = "Confirm your mail";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("innoflowa@gmail.com", "innoflowadmin");
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to send message" + e.Message);
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

        public async Task UpdateProfile(UserDTO model)
        {
            var user = await db.Users.FindByEmailAsync(model.Email);
            
            user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            await db.Users.UpdateAsync(user);
        }

        public async Task<ProfileUserData> GetProfile(string userEmail)
        {
            var user = await db.Users.FindByEmailAsync(userEmail);

            var result = new ProfileUserData()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = await db.Roles.FindByIdAsync(user.Id)
            };

            return result;
        }

        public async Task Logout()
        {
            await db.SignIn.SignOutAsync();
        }
    }
}
