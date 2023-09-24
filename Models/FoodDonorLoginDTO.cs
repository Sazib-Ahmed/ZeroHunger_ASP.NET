using customValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static customValidation.CustomValidation;

namespace ZeroHunger.Models
{
    public class FoodDonorLoginDTO
    {
        public int id { get; set; }

        [Required]
        [CustomValidation.CheckEmail]
        [StringLength(100, ErrorMessage = "Email can be maximum 100 characters long.")]
        public string email { get; set; }

        [Required]
        [CustomValidation.CheckPassword]
        public string password { get; set; }
    }
}