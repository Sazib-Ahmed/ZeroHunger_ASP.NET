using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeroHunger.Auth
{

    public class AdminAccess : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            /*
            if (httpContext.Session["user"] != null && httpContext.Session["type"].ToString().Equals("Admin")) return true;
            return false;

            */return true;
        }


        /*
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var isAuthorized = base.AuthorizeCore(httpContext);
                if (!isAuthorized)
                {
                    return false;
                }
                return httpContext.Session["UserType"].ToString() == "Admin";
            }
        } 
        */

    }
}