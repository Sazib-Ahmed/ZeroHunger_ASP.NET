using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.DB;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        [AdminAccess]
        public ActionResult AdminIndex()
        {
            var db = new zerohungerEntities3();
            var data = db.collect_request.ToList();
            return View(Convert(data));
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(AdminLoginDTO obj)
        {
            /*
            if (ModelState.IsValid)
            {

                var e_password = EncryptPassword(obj.password.Trim());

                var db = new zerohungerEntities3();
                var admin = (from e in db.admins
                            where e.email.Equals(obj.email) &&
                            e.password.Equals(e_password)
                            select e).FirstOrDefault();
                if (admin != null)
                {
                    Session["id"] = admin.id;  // Assign the admin's ID to the session
                    Session["user"] = admin.email;
                    Session["password"] = admin.password.Trim();
                    Session["type"] = "Admin";

                    // Save admin's ID, email, and encrypted password in cookies
                    Response.Cookies["UserId"].Value = admin.id.ToString();
                    Response.Cookies["Email"].Value = admin.email;
                    Response.Cookies["Password"].Value = admin.password.Trim(); // Encrypt the password before saving

                    TempData["Msg"] = "Login successful";
                    return RedirectToAction("AdminIndex");
                }
                else
                {
                    TempData["Msg"] = "Invalid email or password";
                    return View();
                }
            }
            else
            {
                TempData["Msg"] = "Invalid email or password";
                return View();
            }
            */

            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public ActionResult AdminLogout()
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

            TempData["Msg"] = "Admin Logout successful";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AdminAccess]
        public ActionResult AdminRegistration()
        {
            return View();
        }

        [HttpGet]
        [AdminAccess]
        public ActionResult AcceptCollectRequest(int requestId)
        {


            var db = new zerohungerEntities3();

            var c_request = db.collect_request.Find(requestId);
            Session["requestId"] = requestId;

            if (c_request != null)
            {

                c_request.status = "Acceped";

                db.SaveChanges();
                RedirectToAction("AdminIndex");
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
                return RedirectToAction("AdminIndex");
            }
            return RedirectToAction("AdminIndex");
        }
        [HttpGet]
        [AdminAccess]
        public ActionResult RejectCollectRequest(int requestId)
        {
            var db = new zerohungerEntities3();

            var c_request = db.collect_request.Find(requestId);
            if (c_request != null)
            {

                c_request.status = "Rejected";

                db.SaveChanges();
                RedirectToAction("AdminIndex");
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
                return RedirectToAction("AdminIndex");
            }
            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        [AdminAccess]
        public ActionResult AssignEmployee(int requestId)
        {
            var db = new zerohungerEntities3();
            var employees = db.employees.ToList();
            ViewBag.CollectRequestId = requestId;
            return View(employees);
        }

        [HttpPost]
        [AdminAccess]
        public ActionResult AssignEmployee(int collectRequestId, int[] selectedEmployees)
        {

            var db = new zerohungerEntities3();
            // Remove existing assigned employees for the collect request
            var existingAssignments = db.collect_reqest_details
                .Where(cd => cd.collect_request_id == collectRequestId)
                .ToList();
            db.collect_reqest_details.RemoveRange(existingAssignments);

            // Assign selected employees to the collect request
            if (selectedEmployees != null)
            {
                foreach (var empId in selectedEmployees)
                {
                    var assignment = new collect_reqest_details
                    {
                        collect_request_id = collectRequestId,
                        employee_id = empId
                    };
                    db.collect_reqest_details.Add(assignment);
                }
                var c_request = db.collect_request.Find(collectRequestId);
                if (c_request != null)
                {

                    c_request.status = "Collecting";
                }
                else
                {
                    TempData["Msg"] = "Invalid Request";
                }
            }

            db.SaveChanges();
            return RedirectToAction("AdminIndex"); // Replace "Index" with the desired action
        }

        [HttpGet]
        [AdminAccess]
        public ActionResult AssignDistributionInfo(int requestId)
        {
            var db = new zerohungerEntities3();
            ViewBag.CollectRequestId = requestId;
            var employee = db.employees.ToList();
            return View(employee);
        }

        [HttpPost]
        [AdminAccess]
        public ActionResult AssignDistributionInfo(FoodDistributionDTO obj, int[] selectedEmployees)
        {
            if (ModelState.IsValid)
            {
                if (selectedEmployees != null)
                {


                    var db = new zerohungerEntities3();
                    var foodDistribution = new food_distribution();
                    foodDistribution.address = obj.address;
                    foodDistribution.beneficiary_count = obj.beneficiary_count;
                    foodDistribution.time = obj.time;
                    foodDistribution.collect_request_id = obj.collect_request_id;
                    foodDistribution.status = "Distributing";
                    db.food_distribution.Add(foodDistribution);


                    var c_request = db.collect_request.Find(obj.collect_request_id);
                    if (c_request != null)
                    {

                        c_request.status = "Distributing";
                    }
                    else
                    {
                        TempData["Msg"] = "Invalid Request";
                    }







                    db.SaveChanges();


                    var Distribution = (from DistributionInfo in db.food_distribution
                                        where DistributionInfo.collect_request_id.Equals(obj.collect_request_id)
                                        select DistributionInfo).FirstOrDefault();
                    if (Distribution != null)
                    {


                        foreach (var empId in selectedEmployees)
                        {
                            var foodDistributionDatils = new food_distribution_details();
                            {
                                foodDistributionDatils.food_distribution_id = Distribution.id;
                                foodDistributionDatils.employee_id = empId;
                            };
                            db.food_distribution_details.Add(foodDistributionDatils);
                        }


                    }
                    else
                    {
                        TempData["Msg"] = "Invalid Request";
                    }
                    db.SaveChanges();
                }
                else
                {
                    TempData["Msg"] = "Select Employees";
                }

                return RedirectToAction("AdminIndex");
            }
            return View(obj);
        }

        [HttpGet]
        [AdminAccess]
        public ActionResult DistributionList()
        {
            var db = new zerohungerEntities3();
            var distributionList = db.food_distribution.ToList();
            return View(distributionList);
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