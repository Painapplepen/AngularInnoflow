using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InnoflowServer.Domain.Core.Models
{
    public class ChangeProfileUserModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
