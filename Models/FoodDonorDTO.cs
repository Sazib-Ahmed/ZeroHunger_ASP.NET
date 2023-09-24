using customValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeroHunger.DB;
using static customValidation.CustomValidation;

namespace ZeroHunger.Models
{
    public class FoodDonorDTO
    {
        public FoodDonorDTO()
        {
            collect_request = new List<collect_request>();
        }

        public int id { get; set; }

        public string type { get; set; }
        [Required]
        [CheckName]
        [StringLength(50, ErrorMessage = "Name can be maximum 50 characters long.")]
        public string name { get; set; }
        [Required]
        [CheckAddress]
        [StringLength(100, ErrorMessage = "Address can be maximum 100 characters long.")]
        public string address { get; set; }
        [Required]
        public int phone { get; set; }
        [Required]
        [CheckEmail]
        [StringLength(100, ErrorMessage = "Email can be maximum 100 characters long.")]
        public string email { get; set; }
        [Required]
        [CheckPassword]
        public string password { get; set; }

        public virtual ICollection<collect_request> collect_request { get; set; }
    }
}