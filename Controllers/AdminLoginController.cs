using System
    ;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class AdminLoginController : Controller
    {
        huntdbEntities1 db = new huntdbEntities1();

        public static Random random = new Random();
        public static int otp = random.Next(100000, 999999);
        // GET: Admin
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["rememberAdmin"];
            adminlogin adminlogin = new adminlogin();
            if (cookie != null)
            {
                adminlogin.username = cookie["ausername"].ToString();
                adminlogin.password = cookie["apassword"].ToString();
                adminlogin.rememberme = Convert.ToBoolean(cookie["arememberme"]);
                return View(adminlogin);
            }
            return View(adminlogin);
        }

        public ActionResult ResetPassword()
        {
            if (Session["aid"] == null)
            {
                if (TempData["adminemail"] == null)
                {
                    return RedirectToAction("check", "AdminLogin");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(checkpass checkpass, string email)
        {
            if (ModelState.IsValid)
            {
                admin admin = db.admins.Where(p => p.email.Equals(email)).FirstOrDefault();
                admin.password = checkpass.password;
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                if (Session["aid"] == null)
                {
                    return RedirectToAction("Login", "AdminLogin");
                }
                else
                {
                    return RedirectToAction("Dashboard", "AdminLogin");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Talent(talent t)
        {
            db.talents.Add(t);
            db.SaveChanges();

            return View();
        }



        [HttpPost]
        public ActionResult Login(adminlogin adminlogin )
        {
            if (ModelState.IsValid)
            {
                admin adm = db.admins.Where(a => a.username.Equals(adminlogin.username) && a.password.Equals(adminlogin.password)).FirstOrDefault();
                if (adm != null)
                {
                    HttpCookie cookie = new HttpCookie("RememberAdmin");
                    if (adminlogin.rememberme == true)
                    {
                        cookie["ausername"] = adminlogin.username;
                        cookie["apassword"] = adminlogin.password;
                        cookie["arememberme"] = adminlogin.rememberme.ToString();
                        cookie.Expires = DateTime.Now.AddDays(20);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Response.Cookies.Add(cookie);
                    }

                    Session["aid"] = adm.aid.ToString();
                    Session["adminemail"] = adm.email.ToString();
                    Session["aname"] = adm.aname.ToString();
                    Session["image"] = adm.photo.ToString();
                    return RedirectToAction("Dashboard");
                }
                else if (adminlogin.username != null && adminlogin.password != null)
                {
                    ViewBag.errmsg = "Invalid Credentials";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["aid"] != null)
            {
                Session.Clear();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        public ActionResult Dashboard()
        {
            if(Session["aid"] != null)
            {
                ViewBag.user = db.users;
                ViewBag.production = db.productions;
                ViewBag.bids = db.userapplies;
                ViewBag.events = db.productionevents;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult check()
        {
            return View();
        }

        [HttpPost]
        public ActionResult check(emailcheck emailcheck)
        {
            if (ModelState.IsValid)
            {

                admin admin = db.admins.Where(p => p.email.Equals(emailcheck.email)).SingleOrDefault();
                if (admin == null)
                {
                    TempData["emailerr"] = "Email invalid or not available";
                    return RedirectToAction("check", "AdminLogin");
                }

                string subject = "OTP";
                string message = "One Time Password: " + otp + ".<br/><br/> Don't share it with anyone.";

                email email = new email(admin.email, subject, message);

                if (email != null)
                {
                    TempData["otp"] = true;
                }
            }
            TempData["email"] = emailcheck.email;
            return View(emailcheck);
        }

        [HttpPost]
        public ActionResult checkotp(int? eotp, emailcheck emailcheck)
        {
            if (eotp == null)
            {
                TempData["otp"] = true;
                TempData["otperr"] = "*";
                TempData["email"] = emailcheck.email;
                return RedirectToAction("check", "AdminLogin");
            }

            if (otp.Equals(eotp))
            {
                TempData["adminemail"] = emailcheck.email;
                return RedirectToAction("ResetPassword", "AdminLogin");
            }
            else
            {
                TempData["otp"] = true;
                TempData["otperr"] = "InCorrect OTP";
                return RedirectToAction("Check", "AdminLogin");
            }

        }
    }
}