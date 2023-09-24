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
    public class FoodDistributionDTO
    {
        public FoodDistributionDTO()
        {
            food_distribution_details = new List<food_distribution_details>();
        }

        public int id { get; set; }

        [CheckAddress]
        [StringLength(100, ErrorMessage = "Address can be maximum 100 characters long.")]
        public string address { get; set; }
        public System.DateTime time { get; set; }
        [StringLength(100, ErrorMessage = "Beneficiary Count can be maximum 100 characters long.")]
        public string beneficiary_count { get; set; }
        public int collect_request_id { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public virtual collect_request collect_request { get; set; }
        public virtual ICollection<food_distribution_details> food_distribution_details { get; set; }
    }
}