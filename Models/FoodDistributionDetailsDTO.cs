using customValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.DB;

namespace ZeroHunger.Models
{
    public class FoodDistributionDetailsDTO
    {
        public int id { get; set; }
        public Nullable<int> food_distribution_id { get; set; }
        public Nullable<int> employee_id { get; set; }

        public virtual employee employee { get; set; }
        public virtual food_distribution food_distribution { get; set; }
    }
}