using customValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static customValidation.CustomValidation;
using ZeroHunger.DB;
using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.Models
{
    public class CollectRequestDTO
    {

        public CollectRequestDTO()
        {
            collect_reqest_details = new List<collect_reqest_details>();
            food_distribution = new List<food_distribution>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Food Description can be maximum 200 characters long.")]
        public string food_description { get; set; }

        [StringLength(100, ErrorMessage = "Quantity can be maximum 100 characters long.")]
        public string quantity { get; set; }

        [Required]
        public System.DateTime expire_time { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public string preferred_time { get; set; }

        [Required]
        [CheckAddress]
        [StringLength(100, ErrorMessage = "Address can be maximum 100 characters long.")]
        public string address { get; set; }
        public int food_donor_id { get; set; }

        public virtual ICollection<collect_reqest_details> collect_reqest_details { get; set; }
        public virtual food_donor food_donor { get; set; }
        public virtual ICollection<food_distribution> food_distribution { get; set; }
    }
}