using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DB;

namespace ZeroHunger.Auth
{
    public class FoodDonorAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ///*
            if (httpContext.Session["user"] != null && httpContext.Session["type"].ToString().Equals("FoodDonor"))
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
                        // Fetch the user details from the database based on the user ID
                        var db = new zerohungerEntities3();
                        var user = db.food_donor.FirstOrDefault(u => u.id == userId);

                        if (user != null && emailCookie.Value.Trim() == user.email.Trim() && passwordCookie.Value.Trim() == user.password.Trim())
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