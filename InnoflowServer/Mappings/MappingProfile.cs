using AutoMapper;
using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserModel, UserDTO>();
            CreateMap<UserDTO, RegisterUserModel>();

            CreateMap<LoginUserModel, UserDTO>();
            CreateMap<UserDTO, LoginUserModel>();
        }
    }
}
