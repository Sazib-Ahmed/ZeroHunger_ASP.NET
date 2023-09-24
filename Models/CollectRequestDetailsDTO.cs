using customValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.DB;

namespace ZeroHunger.Models
{
    public class CollectRequestDetailsDTO
    {
        public int id { get; set; }
        public Nullable<int> collect_request_id { get; set; }
        public Nullable<int> employee_id { get; set; }

        public virtual collect_request collect_request { get; set; }
        public virtual employee employee { get; set; }
    }
}