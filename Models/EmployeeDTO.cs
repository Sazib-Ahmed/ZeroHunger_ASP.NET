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
    public class EmployeeDTO
    {
        public EmployeeDTO()
        {
            Admins = new List<Admin>();
            collect_reqest_details = new List<collect_reqest_details>();
            food_distribution_details = new List<food_distribution_details>();
        }

        public int id { get; set; }

        [Required]
        [CheckName]
        [StringLength(50, ErrorMessage = "Name can be maximum 50 characters long.")]
        public string name { get; set; }

        [Required]
        [StringLength(20)]
        public string gender { get; set; }

        [Required]
        public int phone { get; set; }

        [StringLength(13, ErrorMessage = "NID number can be maximum 13 digits long.")]
        [CheckNationalID]
        public string nid { get; set; }

        [Required]
        [CheckEmail]
        [StringLength(100, ErrorMessage = "Email can be maximum 100 characters long.")]
        public string email { get; set; }

        [StringLength(100, ErrorMessage = "Address can be maximum 100 characters long.")]
        public string address { get; set; }

        [Required]
        [CheckPassword]
        public string password { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<collect_reqest_details> collect_reqest_details { get; set; }
        public virtual ICollection<food_distribution_details> food_distribution_details { get; set; }
    }
}