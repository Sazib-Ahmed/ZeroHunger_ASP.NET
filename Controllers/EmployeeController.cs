using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.DB;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [HttpGet]
        [EmployeeAccess]
        public ActionResult EmployeeIndex()
        {
            var db = new zerohungerEntities3();
            var collectRequests = db.collect_request.ToList();
            return View(collectRequests);
        }

        [HttpGet]
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeLogin(EmployeeLoginDTO obj)
        {
            if (ModelState.IsValid)
            {

                var e_password = EncryptPassword(obj.password.Trim());

                var db = new zerohungerEntities3();
                var employee = (from e in db.employees
                                where e.email.Equals(obj.email) &&
                                e.password.Equals(e_password)
                                select e).FirstOrDefault();
                if (employee != null)
                {
                    Session["id"] = employee.id;  // Assign the employee's ID to the session
                    Session["user"] = employee.email;
                    Session["password"] = employee.password.Trim();
                    Session["type"] = "Employee";

                    // Save employee's ID, email, and encrypted password in cookies
                    Response.Cookies["UserId"].Value = employee.id.ToString();
                    Response.Cookies["Email"].Value = employee.email;
                    Response.Cookies["Password"].Value = employee.password.Trim(); // Encrypt the password before saving

                    TempData["Msg"] = "Login successful";
                    return RedirectToAction("EmployeeIndex");
                }
                else
                {
                    TempData["Msg"] = "Invalid email or password";
                    return RedirectToAction("EmployeeLogin");
                }
            }
            return View(obj);
        }

        public ActionResult EmployeeLogout()
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
        public ActionResult EmployeeRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeRegistration(EmployeeDTO obj)
        {
            if (ModelState.IsValid)
            {
                var password = EncryptPassword(obj.password.Trim());

                var db = new zerohungerEntities3();
                var employee = new employee();
                employee.name = obj.name;
                employee.phone = obj.phone;
                employee.nid = obj.nid;
                employee.email = obj.email;
                employee.address = obj.address;
                employee.password = password;
                employee.status = "penging"; // Default status is "pending
                employee.gender = obj.gender;

                db.employees.Add(employee);
                db.SaveChanges();

                TempData["Msg"] = "Employee registered successfully";
                return RedirectToAction("EmployeeIndex");
            }
            return View(obj);
        }


        [HttpGet]
        [EmployeeAccess]

        public ActionResult EmployeeCollected(int requestId)
        {

            var db = new zerohungerEntities3();

            var c_request = db.collect_request.Find(requestId);
            Session["requestId"] = requestId;

            if (c_request != null)
            {
                c_request.status = "Collected";
                db.SaveChanges();
                RedirectToAction("EmployeeIndex");
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
                return RedirectToAction("EmployeeIndex");
            }
            return RedirectToAction("EmployeeIndex");
        }

        [EmployeeAccess]
        public ActionResult EmployeeDistributed(int distributionId)
        {

            var db = new zerohungerEntities3();

            var e_distrbution = (from d in db.food_distribution
                                 where d.id.Equals(distributionId)
                                 select d).FirstOrDefault();

            var c_request = (from r in db.collect_request
                             where r.id.Equals(e_distrbution.collect_request_id)
                             select r).FirstOrDefault();

            if (c_request != null)
            {
                c_request.status = "Deliverd";
                db.SaveChanges();
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
            }

            if (e_distrbution != null)
            {
                e_distrbution.status = "Deliverd";
                db.SaveChanges();
                
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
            }

            return RedirectToAction("EmployeeFoodDistribution");
        }



        [HttpGet]
        [EmployeeAccess]
        public ActionResult EmployeeFoodDistribution()
        {

            var db = new zerohungerEntities3();
            var foodDistribution = db.food_distribution.ToList();
            return View(foodDistribution);

        }





        public EmployeeDTO Convert(employee e)
        {
            var employee = new EmployeeDTO()
            {
                id = e.id,
                name = e.name,
                gender = e.gender,
                phone = e.phone,
                nid = e.nid,
                email = e.email,
                address = e.address,
                password = e.password,
                status = e.status
            };
            return employee;
        }

        public employee Convert(EmployeeDTO e)
        {
            var employee = new employee()
            {
                id = e.id,
                name = e.name,
                gender = e.gender,
                phone = e.phone,
                nid = e.nid,
                email = e.email,
                address = e.address,
                password = e.password,
                status = e.status
            };
            return employee;
        }

        List<EmployeeDTO> Convert(List<employee> employees)
        {
            var convertedEmployees = new List<EmployeeDTO>();
            foreach (var employee in employees)
            {
                var convertedEmployee = Convert(employee);
                convertedEmployees.Add(convertedEmployee);
            }
            return convertedEmployees;
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