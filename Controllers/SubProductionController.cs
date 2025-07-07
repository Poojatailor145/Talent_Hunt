using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;

namespace TalentHunt.Controllers
{
    public class SubProductionController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: SubProduction
        public ActionResult Index()
        {
            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.subproductions.Where(x => x.pid.Equals(pid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No subscriptions found";
                }
                TempData["pdata"] = result;
                return View(db.subproductions.ToList());
            }
            var subproductions = db.subproductions.Include(s => s.plan).Include(s => s.production);
            return View(subproductions.ToList());
        }

        public ActionResult ProSub()
        {
            if (Session["aid"] != null)
            {
                DateTime date = DateTime.Today;
                ViewBag.todaysdate = date;
                return View(db.subproductions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult ProSub(string Search)
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
                    return View(db.subproductions.ToList());
                }
                List<subproduction> subs= db.subproductions.Where(p => p.production.pname.Contains(Search) || Search == null).ToList();
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

        // GET: SubProduction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subproduction subproduction = db.subproductions.Find(id);
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            return View(subproduction);
        }

        // GET: SubProduction/Create
        public ActionResult Create(int id)
        {
            if (Session["pid"] != null)
            {
                subproduction subproduction = new subproduction();
                subproduction.planid = id;
                subproduction.pid= Convert.ToInt32(HttpContext.Session["pid"]);
                subproduction.startdate = DateTime.Today;

                var plandata = db.plans.Where(p => p.planid.Equals(id));
                foreach (var pd in plandata)
                {
                    int mnths = Convert.ToInt32(pd.duration);
                    subproduction.enddate = subproduction.startdate.AddMonths(mnths);
                }
                db.subproductions.Add(subproduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.planid = new SelectList(db.plans, "planid", "plantype");
            ViewBag.pid = new SelectList(db.productions, "pid", "pname");
            return View();
        }

        // POST: SubProduction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "spid,planid,pid,startdate,enddate")] subproduction subproduction)
        {
            if (ModelState.IsValid)
            {
                db.subproductions.Add(subproduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // GET: SubProduction/Edit/5
        public ActionResult Edit(int id)
        {

            int pid = Convert.ToInt32(HttpContext.Session["pid"]);
            subproduction subproduction = db.subproductions.Find(pid);
            subproduction.planid = id;
            subproduction.pid = Convert.ToInt32(HttpContext.Session["pid"]);
            subproduction.startdate = DateTime.Today;

            var plandata = db.plans.Where(p => p.planid.Equals(id));
            foreach (var pd in plandata)
            {
                int mnths = Convert.ToInt32(pd.duration);
                subproduction.enddate = subproduction.startdate.AddMonths(mnths);
            }

            db.Entry(subproduction).State = EntityState.Modified;
            return RedirectToAction("ProInfo", "Production");
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // POST: SubProduction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "spid,planid,pid,startdate,enddate")] subproduction subproduction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subproduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.planid = new SelectList(db.plans, "planid", "plantype", subproduction.planid);
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", subproduction.pid);
            return View(subproduction);
        }

        // GET: SubProduction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subproduction subproduction = db.subproductions.Find(id);
            if (subproduction == null)
            {
                return HttpNotFound();
            }
            return View(subproduction);
        }

        // POST: SubProduction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subproduction subproduction = db.subproductions.Find(id);
            db.subproductions.Remove(subproduction);
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
