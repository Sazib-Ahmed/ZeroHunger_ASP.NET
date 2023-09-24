using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DB;

namespace ZeroHunger.Auth
{
    public class EmployeeAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
           // /*
            if (httpContext.Session["user"] != null && httpContext.Session["type"].ToString().Equals("Employee"))
            {
                // Check if the user's ID, email, and encrypted password are saved in cookies
                var userIdCookie = httpContext.Request.Cookies["UserId"];
                var emailCookie = httpContext.Request.Cookies["Email"];
                var passwordCookie = httpContext.Request.Cookies["Password"];

                if (userIdCookie != null && emailCookie != null && passwordCookie != null)
                {
                    int userId;
                    if (int.TryParse(userIdCookie.Value, out userId))
                    {
                        // Fetch the employee details from the database based on the employee ID
                        var db = new zerohungerEntities3();
                        var employee = db.employees.FirstOrDefault(e => e.id == userId);

                        if (employee != null && emailCookie.Value.Trim() == employee.email.Trim() && passwordCookie.Value.Trim() == employee.password.Trim())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;


            //*/return true;
        }


    }

}