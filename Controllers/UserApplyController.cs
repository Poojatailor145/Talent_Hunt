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

namespace TalentHunt.Controllers
{
    public class UserApplyController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: UserApply
        public ActionResult Index()
        {
            if (Session["uid"] != null)
            {
                DateTime date = DateTime.Today;
                ViewBag.date = date;
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.userapplies.Where(x => x.userid.Equals(uid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No biddings yet";
                }
                TempData["pdata"] = result;
                List<rating> rates = db.ratings.Where(p => p.userid.Equals(uid)).ToList();
                if (rates.Count() == 0)
                {
                    TempData["nrates"] = "No";
                    TempData["rates"] = rates;
                }
                else
                {
                    TempData["rates"] = rates;
                    TempData["nrates"] = "Yes";
                }
                return View(db.userapplies.ToList());
            }

            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.userapplies.Where(x => x.pid.Equals(pid) && x.productionevent.status.Equals("active"));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No biddings yet";
                }
                TempData["pdata"] = result;
                return View(result.ToList());
            }
            var userapplies = db.userapplies.Include(u => u.production).Include(u => u.productionevent).Include(u => u.user);
            return View(userapplies.ToList());
        }

        public ActionResult BidView()
        {
            if (Session["aid"] != null)
            {
                return View(db.userapplies.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult BidView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.userapplies.ToList());
                }
                List<userapply> bids= db.userapplies.Where(p => p.productionevent.ename.Contains(Search) || Search == null).ToList();
                if (bids.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(bids.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: UserApply/Details/5
        public ActionResult Details(int? id)
        {    
            if(Session["uid"]!=null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                userapply userapply = db.userapplies.Find(id);
                if (userapply == null)
                {
                    return HttpNotFound();
                }
                return View(userapply);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
            
        }

        // GET: UserApply/Create
        public ActionResult Create(int? id)
        {
            if(Session["uid"]!=null)
            {
                ViewBag.eid = id;
                ViewBag.pid = new SelectList(db.productions, "pid", "pname");
                ViewBag.peid = new SelectList(db.productionevents, "peid", "ename");
                ViewBag.userid = new SelectList(db.users, "userid", "fname");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
        }

        // POST: UserApply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "uaid,pid,peid,userid,appdate,expay,message,status")] userapplyv userapplyv,int eid)
        {
            if (ModelState.IsValid)
            {
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.productionevents.Where(p => p.peid.Equals(eid)).SingleOrDefault();

                userapplyv.appdate = DateTime.Now;
                userapplyv.pid = result.pid;
                userapplyv.peid = eid;
                userapplyv.userid = uid;
                userapplyv.status = "applied";

                userapply userapply = new userapply();
                AutoMapper.Mapper.Map(userapplyv, userapply);

                db.userapplies.Add(userapply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapplyv.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapplyv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapplyv.userid);
            return View(userapplyv);
        }

        // GET: UserApply/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userapply userapply = db.userapplies.Find(id);
            userapplyv userapplyv = new userapplyv();
            AutoMapper.Mapper.Map(userapply, userapplyv);
            if (userapply == null)
            {
                return HttpNotFound();
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapplyv.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapplyv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapplyv.userid);
            return View(userapplyv);
        }

        // POST: UserApply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uaid,pid,peid,userid,appdate,expay,message,status")] userapplyv userapplyv)
        {
            if (ModelState.IsValid)
            {
                userapply userapply = new userapply();
                userapply.uaid = userapplyv.uaid;
                userapply.pid = userapplyv.pid;
                userapply.peid = userapplyv.peid;
                userapply.userid = userapplyv.userid;
                userapply.appdate = userapplyv.appdate;
                userapply.expay = userapplyv.expay;
                userapply.message = userapplyv.message;
                userapply.status = userapplyv.status;


                db.Entry(userapply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","ProductionEvent",new { id=userapplyv.peid});
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", userapplyv.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userapplyv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userapplyv.userid);
            return View(userapplyv);
        }

        // GET: UserApply/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "UserApply");
                }
                userapply userapply = db.userapplies.Find(id);
                if (userapply != null)
                {
                    userselect userselect = db.userselects.Where(p => p.peid.Equals(userapply.peid) && p.userid.Equals(userapply.userid)).SingleOrDefault();
                    if (userselect != null)
                    {
                        string msg = $"{userapply.user.fname} {userapply.user.lname} has withdrawn from your {userapply.production.pname} Event.";
                        string sub = "Expert Withdraw";
                        email email = new email(userapply.production.email, sub, msg);

                        db.userselects.Remove(userselect);
                        db.userapplies.Remove(userapply);
                        db.SaveChanges();

                        return RedirectToAction("Index", "UserApply");
                    }
                    userapply userappply = db.userapplies.Where(p => p.peid.Equals(userapply.peid) && p.userid.Equals(userapply.userid)).SingleOrDefault();
                    if (userappply != null)
                    {
                        string msg = $"{userapply.user.fname} {userapply.user.lname} has withdrawn from your {userapply.production.pname} Event.";
                        string sub = "Expert Withdraw";
                        email email = new email(userapply.production.email, sub, msg);
                        db.userapplies.Remove(userappply);
                        db.userapplies.Remove(userapply);
                        db.SaveChanges();

                        return RedirectToAction("Index", "UserApply");
                    }


                    return RedirectToAction("Index", "UserApply");
                }
                else
                {
                    return RedirectToAction("Index", "UserApply");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: UserApply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userapply userapply = db.userapplies.Find(id);
            db.userapplies.Remove(userapply);
            db.SaveChanges();
            return RedirectToAction("Index");
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
