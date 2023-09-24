using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.DB;
using ZeroHunger.Models;
using System.Security.Cryptography;
using System.Text;

namespace ZeroHunger.Controllers
{
    public class FoodDonorController : Controller
    {
        // GET: FoodDonor
        [HttpGet]
        [FoodDonorAccess]
        public ActionResult FoodDonorIndex()
        {
            return View();
        }


        [HttpGet]
        public ActionResult FoodDonorLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FoodDonorLogin(FoodDonorLoginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new zerohungerEntities3();
                var password = EncryptPassword(obj.password.Trim());
                var user = (from u in db.food_donor
                            where u.email.Trim().Equals(obj.email.Trim()) &&
                            u.password.Trim().Equals(password)
                            select u).FirstOrDefault();
                if (user != null)
                {
                    Session["id"] = user.id;  // Assign the user's ID to the session
                    Session["user"] = user.email;
                    Session["password"] = user.password.Trim();
                    Session["type"] = "FoodDonor";

                    // Save user's ID, email, and encrypted password in cookies
                    Response.Cookies["UserId"].Value = user.id.ToString();
                    Response.Cookies["Email"].Value = user.email;
                    Response.Cookies["Password"].Value = user.password.Trim(); // Encrypt the password before saving

                    TempData["Msg"] = "Login successful";
                    return RedirectToAction("FoodDonorIndex");
                }
                else
                {
                    TempData["Msg"] = "Invalid email or password";
                    return RedirectToAction("FoodDonorLogin");
                }
            }
            return View(obj);
        }

        public string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



        [HttpGet]
        public ActionResult FoodDonorRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FoodDonorRegistration(FoodDonorDTO obj)
        {
            if (ModelState.IsValid)
            {

                var password = EncryptPassword(obj.password);

                var db = new zerohungerEntities3();
                var foodDonor = new food_donor();
                foodDonor.type = obj.type;
                foodDonor.name = obj.name;
                foodDonor.address = obj.address;
                foodDonor.phone = obj.phone;
                foodDonor.email = obj.email;
                foodDonor.password = password;

                db.food_donor.Add(foodDonor);
                db.SaveChanges();

                TempData["Msg"] = "Food Donor registered successfully";
                return RedirectToAction("FoodDonorIndex");
            }
            return View(obj);
        }

        [HttpGet]
        [FoodDonorAccess]
        public ActionResult FoodDonorLogout()
        {
            // Clear session
            Session.Clear();

            // Clear cookies
            var userIdCookie = new HttpCookie("UserId");
            userIdCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(userIdCookie);

            var emailCookie = new HttpCookie("Email");
            emailCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(emailCookie);

            var passwordCookie = new HttpCookie("Password");
            passwordCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(passwordCookie);

            TempData["Msg"] = "Logout successful";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [FoodDonorAccess]
        public ActionResult FoodDonorCollectRequest()
        {
            return View();
        }

        [HttpPost]
        [FoodDonorAccess]
        public ActionResult FoodDonorCollectRequest(CollectRequestDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new zerohungerEntities3();
                var c_request = new collect_request();
                c_request.food_description = obj.food_description;
                c_request.quantity = obj.quantity;
                c_request.expire_time = obj.expire_time;
                c_request.preferred_time = obj.preferred_time;
                c_request.status = "Pending";
                c_request.address = obj.address;
                c_request.food_donor_id = (int)Session["id"]; // Access the ID from the session

                db.collect_request.Add(c_request);
                db.SaveChanges();

                TempData["Msg"] = "Collect Request Placed Successfully with request id " + c_request.id;
                return RedirectToAction("FoodDonorIndex");
            }
            return View(obj);
        }

        [HttpGet]
        [FoodDonorAccess]
        public ActionResult FoodDonorRequests()
        {
            var db = new zerohungerEntities3();
            int foodDonorId = (int)Session["id"]; // Retrieve food donor ID from session
            var data = db.collect_request.Where(c => c.food_donor_id == foodDonorId).ToList();
            return View(Convert(data));
        }

        [HttpGet]
        [FoodDonorAccess]
        public ActionResult CollectRequestDelete(int id)
        {

            var db = new zerohungerEntities3();
            var request = db.collect_request.Find(id);

            if (request != null)
            {
                db.collect_request.Remove(request);
                db.SaveChanges();

                TempData["Msg"] = "Collect Request Deleted Successfully";
            }
            else
            {
                TempData["Msg"] = "Collect Request not found";
            }

            return RedirectToAction("FoodDonorRequests");

        }

        [HttpGet]
        [FoodDonorAccess]
        public ActionResult FoodDonorDonationHistory()
        {
            var db = new zerohungerEntities3();
            int foodDonorId = (int)Session["id"]; // Retrieve food donor ID from session
            var foodDonations = db.collect_request.Where(c => c.food_donor_id == foodDonorId).ToList();

            return View(foodDonations);
        }








        public CollectRequestDTO Convert(collect_request c)
        {
            var request = new CollectRequestDTO()
            {
                id = c.id,
                food_description = c.food_description,
                quantity = c.quantity,
                expire_time = c.expire_time,
                status = c.status,
                preferred_time = c.preferred_time,
                address = c.address
            };
            return request;
        }

        public collect_request Convert(CollectRequestDTO c)
        {
            var request = new collect_request()
            {
                id = c.id,
                food_description = c.food_description,
                quantity = c.quantity,
                expire_time = c.expire_time,
                status = c.status,
                preferred_time = c.preferred_time,
                address = c.address
            };
            return request;
        }

        List<CollectRequestDTO> Convert(List<collect_request> requests)
        {
            var convertedRequests = new List<CollectRequestDTO>();
            foreach (var request in requests)
            {
                var convertedRequest = Convert(request);
                convertedRequests.Add(convertedRequest);
            }
            return convertedRequests;
        }



    }
}