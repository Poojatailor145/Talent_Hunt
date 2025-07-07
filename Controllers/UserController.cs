using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;
using System.IO;

namespace TalentHunt.Controllers
{
    public class UserController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: User
        public ActionResult Index()
        {
            var talents = db.talents.ToList();
            TempData["talents"] = talents;
            return View(db.userprofiles.ToList());
        }
        [HttpPost]

        public ActionResult Index(int tid)
        {
            if (Session["pid"] != null)
            {
                if(tid.Equals(null)|| tid == 0)
                {
                    var talent = db.talents.ToList();
                    TempData["talents"] = talent;
                    return View(db.userprofiles.ToList());
                }
                List<userprofile> plan = db.userprofiles.Where(p => p.tid.Equals(tid)).ToList();
                if (plan.Count() == 0)
                {
                    TempData["NotFound"] = "No such talent found";
                }
                var talents = db.talents.ToList();
                TempData["talents"] = talents;
                return View(plan.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public String talentlist()
        {
            string output = "Testing";
            return output;
        }

        public ActionResult UserView()
        {
            if (Session["aid"] != null)
            {
                return View(db.users.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult UserView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.users.ToList());
                }
                List<user> users= db.users.Where(p => p.fname.Contains(Search) || Search == null).ToList();
                if (users.Count() == 0)
                {
                    List<user> userg = db.users.Where(p => p.gender.Equals(Search)).ToList();
                    if(userg.Count() ==0)
                    {
                        try
                        {
                            int s = Convert.ToInt32(Search);
                            List<user> ages = db.users.Where(p => p.age <= s).ToList();
                            if (ages.Count() == 0)
                            {
                                TempData["NotFound"] = "User Not Found";
                            }
                            else
                            {
                                return View(ages.ToList());
                            }
                        }
                        catch
                        {
                            TempData["NotFound"] = "User Not Found";
                        }
                    }
                    else
                    {
                        return View(userg.ToList());
                    }
                    
                    
                }
                else
                {
                    return View(users.ToList());
                }

                return View(users.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }
        public ActionResult ShortView(int? id, user users, talent talents, userprofile userprofiles, image images)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["aid"] != null)
            {
                int usrid = Convert.ToInt32(id);
                var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
                TempData["talentlists"] = result;

                var elists = db.images.Where(p => p.userid.Equals(usrid));
                TempData["piclists"] = elists;

                var videolists = db.videos.Where(p => p.userid.Equals(usrid));
                TempData["videolists"] = videolists;

                var rates = db.ratings.Where(p => p.userid.Equals(usrid));
                if (rates.Count() != 0)
                {
                    double sum = 0;
                    double finalrate = 0;
                    double total = rates.Count();
                    foreach (var rt in rates)
                    {
                        sum += rt.rating1;
                    }
                    finalrate = sum / total;
                    TempData["ratings"] = finalrate;
                }
                else
                {
                    TempData["ratings"] = 0;
                }

                user user = db.users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
                
        }

        public ActionResult Profile(user users,talent talents,userprofile userprofiles,image images)
        {
            if(Session["uid"]!=null)
            {

                int usrid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
                TempData["talentlists"] = result;

                var elists = db.images.Where(p => p.userid.Equals(usrid));
                TempData["piclists"] = elists;

                var videolists = db.videos.Where(p => p.userid.Equals(usrid));
                TempData["videolists"] = videolists;

                var rates = db.ratings.Where(p => p.userid.Equals(usrid));
                if(rates.Count()!=0)
                {
                    double sum = 0;
                    double finalrate = 0;
                    double total = rates.Count();
                    foreach (var rt in rates)
                    {
                        sum += rt.rating1;
                    }
                    finalrate = sum / total;
                    TempData["ratings"] = finalrate;
                }   
                else
                {
                    TempData["ratings"] = 0;
                }

                var usr = db.users.Where(p => p.userid.Equals(usrid)).FirstOrDefault();
                if (result!=null)
                {
                    TempData["fname"] = usr.fname.ToString();
                    TempData["lname"] = usr.lname.ToString();
                    TempData["age"] = usr.age.ToString();
                    TempData["gender"] = usr.gender.ToString();
                    TempData["mail"] = usr.email.ToString();
                }
                return View(db.users.ToList());


            }
            else
            {
                return RedirectToAction("Login","User");
            }
        }

        [HttpPost]

        public ActionResult Login(login login, string options)
        {

            if (options == "user")
            {
                user usr = db.users.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                if (usr != null)
                {
                   if(usr.status=="active")
                    {
                        Session["uid"] = usr.userid.ToString();
                        Session["photo"] = usr.photo.ToString();
                        Session["uname"] = usr.fname.ToString() + " " + usr.lname.ToString();
                        TempData["gender"] = usr.gender;
                        TempData["age"] = usr.age;
                        return RedirectToAction("Profile");
                    }
                   else
                    {
                        ViewBag.err = "You are banned from using TalentHunt";
                    }
                }
                else
                {
                    ViewBag.err = "Invalid User Credentials";
                    TempData["register"] = "<div class='font-16 weight-600 pt-10 pb-10 text-center' data-color='#707373'>OR</div><div class='input-group mb-0'><a class='btn btn-outline-primary btn-lg btn-block' href='/User/Create'>Register To Create User Account</a></div>";
                }
            }
            else if (options == "production")
            {
                production pro = db.productions.Where(p => p.username.Equals(login.username) && p.password.Equals(login.password)).FirstOrDefault();
                if (pro != null)
                {
                    if(pro.status=="active")
                    {
                        Session["pid"] = pro.pid.ToString();
                        Session["pimage"] = pro.pimage.ToString();
                        Session["pname"] = pro.pname.ToString();
                        return RedirectToAction("Proinfo", "Production");
                    }
                    else
                    {
                        ViewBag.err = "You are banned from using TalentHunt";
                    }
                }
                else
                {
                    ViewBag.err = "Invalid Production Credentials";
                    TempData["register"] = "<div class='font-16 weight-600 pt-10 pb-10 text-center' data-color='#707373'>OR</div><div class='input-group mb-0'><a class='btn btn-outline-primary btn-lg btn-block' href='/Production/Create'>Register To Create Production Account</a></div>";
                }
            }
            else
            {
                ViewBag.select = "Select User or Production";
            }
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int? id, user users, talent talents, userprofile userprofiles, image images)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           

            int usrid = Convert.ToInt32(id);
            var result = db.userprofiles.Where(p => p.userid.Equals(usrid));
            TempData["tlist"] = result;

            var elists = db.images.Where(p => p.userid.Equals(usrid));
            TempData["piclists"] = elists;

            var videolists = db.videos.Where(p => p.userid.Equals(usrid));
            TempData["videolists"] = videolists;

            var rates = db.ratings.Where(p => p.userid.Equals(usrid));
            if (rates.Count() != 0)
            {
                double sum = 0;
                double finalrate = 0;
                double total = rates.Count();
                foreach (var rt in rates)
                {
                    sum += rt.rating1;
                }
                finalrate = sum / total;
                TempData["ratings"] = finalrate;
            }
            else
            {
                TempData["ratings"] = 0;
            }

            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password,ImageFile,status")] userv userv)
        {
            if (ModelState.IsValid)
            {
                if (userv.ImageFile == null)
                {
                    ViewBag.emptyerr = "*";
                }
                else if (userv.ImageFile.ContentType == "image/jpeg" || userv.ImageFile.ContentType == "image/png" || userv.ImageFile.ContentType == "image/jpg")
                {
                    string fileName = Path.GetFileNameWithoutExtension(userv.ImageFile.FileName);
                    string extension = Path.GetExtension(userv.ImageFile.FileName);
                    fileName = fileName + extension;
                    userv.photo = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    userv.ImageFile.SaveAs(fileName);
                    userv.status = "active";

                    user user = new user();
                    AutoMapper.Mapper.Map(userv, user);

                    db.users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Profile");
                }
                else
                {
                    ViewBag.picformat = "Invalid Format";
                }
            }
            return View(userv);
        }

        public ActionResult EditStatus(int? id,string status)
        {
            if(id==null || status==null)
            {
                return RedirectToAction("UserView","User");
            }
            user user = db.users.Find(id);

            if(status=="blocked")
            {
                user.status = status;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserView","User");
            }
            else if(status=="active")
            {
                user.status = status;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserView", "User");
            }
            else
            {
                return RedirectToAction("UserView","User");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Profile","User");
            }
            user user = db.users.Find(id);
            edituser edituser=  new edituser();
            AutoMapper.Mapper.Map(user, edituser);
            if (user == null)
            {
                return RedirectToAction("Profile","User");
            }
            return View(edituser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,fname,lname,gender,age,address,city,state,pincode,photo,email,username,password,status")] edituser edituser)
        {
            if (ModelState.IsValid)
            {
                user user = new user();

                user.userid = edituser.userid;
                user.fname = edituser.fname;
                user.lname = edituser.lname;
                user.gender = edituser.gender;
                user.age = edituser.age;
                user.address = edituser.address;
                user.city = edituser.city;
                user.state = edituser.state;
                user.pincode = edituser.pincode;
                user.photo = edituser.photo;
                user.email = edituser.email;
                user.username = edituser.username;
                user.password = edituser.password;
                user.status = edituser.status;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(edituser);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if(Session["uid"]!=null || Session["pid"]!=null)
            {
                Session.Abandon();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
