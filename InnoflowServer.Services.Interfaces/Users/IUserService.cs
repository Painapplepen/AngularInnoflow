using InnoflowServer.Domain.Core.DTO;
using System.Threading.Tasks;

namespace InnoflowServer.Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<bool> Register(UserDTO model);
        
        Task<bool> Login(UserDTO model);
        
        Task Logout();
    }
}
