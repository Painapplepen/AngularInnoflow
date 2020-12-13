using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnoflowServer.Domain.Core.Models
{
    public class ProfileUserData
    {
        public string FirstName;
        public string LastName;
        public string Email;
        public IdentityRole Role;
        public string PhoneNumber;
    }
}
