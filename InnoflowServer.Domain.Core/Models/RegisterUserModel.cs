using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Core.Models
{
    public class RegisterUserModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password not confirmed")]
        public string PasswordConfirm { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public List<string> UserJobCategories { get; set; }
    }
}
