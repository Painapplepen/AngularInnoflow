using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Models;
using System.Threading.Tasks;

namespace InnoflowServer.Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<string> Register(UserDTO model);
        
        Task<string> Login(UserDTO model);

        Task<bool> SendEmail(UserDTO model, string message);

        Task<bool> ConfirmEmail(string userEmail, string code);

        Task UpdateProfile(UserDTO model);

        Task<ProfileUserData> GetProfile(string userEmail);
        
        Task Logout();
    }
}
