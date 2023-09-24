using customValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static customValidation.CustomValidation;

namespace ZeroHunger.Models
{
    public class AdminLoginDTO
    {
        [Required]
        public int id { get; set; }

        [Required]
        [NoSpaceUsername]
        public string username { get; set; }

        [Required]
        [CheckPassword]
        public string password { get; set; }
    }
}