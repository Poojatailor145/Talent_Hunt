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
    public class EventRateController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: EventRate
        public ActionResult Index()
        {
            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.eventrates.Where(x => x.productionevent.pid.Equals(pid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No Ratings Yet";
                }
                TempData["pdata"] = result;
                return View(db.eventrates.ToList());
            }
            var eventrates = db.eventrates.Include(e => e.productionevent).Include(e => e.user);
            return View(eventrates.ToList());
        }

        public ActionResult ErateView()
        {
            if (Session["aid"] != null)
            {
                return View(db.eventrates.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult ErateView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.eventrates.ToList());
                }
                List<eventrate> rates = db.eventrates.Where(p => p.productionevent.ename.Contains(Search)).ToList();
                if (rates.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(rates.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: EventRate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrate eventrate = db.eventrates.Find(id);
            if (eventrate == null)
            {
                return HttpNotFound();
            }
            return View(eventrate);
        }

        // GET: EventRate/Create
        public ActionResult Create(int peid,int userid)
        {
            ViewBag.peid = peid;
            ViewBag.userid = userid;
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename");
            ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: EventRate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "erid,peid,userid,rating,comment")] eventratev eventratev)
        {
            if (ModelState.IsValid)
            {
                eventrate eventrate = new eventrate();
                eventrate.peid = eventratev.peid;
                eventrate.userid = eventratev.userid;
                eventrate.rating = eventratev.rating;
                eventrate.comment = eventratev.comment;

                db.eventrates.Add(eventrate);
                db.SaveChanges();
                return RedirectToAction("Details","ProductionEvent", new { id = eventratev.peid });
            }

            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventratev.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", eventratev.userid);
            return View(eventratev);
        }

        // GET: EventRate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrate eventrate = db.eventrates.Find(id);
            if (eventrate == null)
            {
                return HttpNotFound();
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventrate.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", eventrate.userid);
            return View(eventrate);
        }

        // POST: EventRate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "erid,peid,userid,rating,comment")] eventrate eventrate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventrate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventrate.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", eventrate.userid);
            return View(eventrate);
        }

        // GET: EventRate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrate eventrate = db.eventrates.Find(id);
            if (eventrate == null)
            {
                return HttpNotFound();
            }
            return View(eventrate);
        }

        // POST: EventRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eventrate eventrate = db.eventrates.Find(id);
            db.eventrates.Remove(eventrate);
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
