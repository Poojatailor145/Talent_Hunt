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
    public class SubUserController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: SubUser
        public ActionResult Index()
        {
            if (Session["uid"] != null)
            {
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.subusers.Where(x => x.userid.Equals(uid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No subscriptions found";
                }
                TempData["pdata"] = result;
                return View(db.subusers.ToList());
            }

            var subusers = db.subusers.Include(s => s.plan).Include(s => s.user);
            return View(subusers.ToList());
        }

        public ActionResult UserSub()
        {
            if (Session["aid"] != null)
            {
                DateTime date = DateTime.Today;
                ViewBag.todaysdate = date;

                return View(db.subusers.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult UserSub(string Search)
        {
            if (Session["aid"] != null)
            {
                DateTime date = DateTime.Today;
                ViewBag.todaysdate = date;
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.subusers.ToList());
                }
                List<subuser> subs = db.subusers.Where(p => p.user.fname.Contains(Search) || Search == null).ToList();
                if (subs.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(subs.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: SubUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subuser subuser = db.subusers.Find(id);
            if (subuser == null)
            {
                return HttpNotFound();
            }
            return View(subuser);
        }

        // GET: SubUser/Create
        public ActionResult Create(int id)
        {
            if(Session["uid"]!=null)
            {
                subuser subuser = new subuser();
                subuser.planid = id;
                subuser.userid = Convert.ToInt32(HttpContext.Session["uid"]);
                subuser.startdate = DateTime.Today;

                var plandata = db.plans.Where(p => p.planid.Equals(id));
                foreach (var pd in plandata)
                {
                    int mnths = Convert.ToInt32(pd.duration);
                    subuser.enddate = subuser.startdate.AddMonths(mnths);
                }
                db.subusers.Add(subuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }          

            ViewBag.planid = new SelectList(db.plans, "planid", "plantype");
            ViewBag.pid = new SelectList(db.productions, "pid", "pname");
            return View();
        }

        // POST: SubUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "suid,planid,userid,startdate,enddate")] subuserv subuserv)
        {
            if (ModelState.IsValid)
            {
                subuser subuser = new subuser();

                
                db.subusers.Add(subuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subuserv.planid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", subuserv.userid);
            return View(subuserv);
        }

        // GET: SubUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subuser subuser = db.subusers.Find(id);
            if (subuser == null)
            {
                return HttpNotFound();
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subuser.planid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", subuser.userid);
            return View(subuser);
        }

        // POST: SubUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "suid,planid,pid,startdate,enddate")] subuser subuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subuser.planid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", subuser.userid);
            return View(subuser);
        }

        // GET: SubUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subuser subuser = db.subusers.Find(id);
            if (subuser == null)
            {
                return HttpNotFound();
            }
            return View(subuser);
        }

        // POST: SubUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subuser subuser = db.subusers.Find(id);
            db.subusers.Remove(subuser);
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
