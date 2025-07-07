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
    public class RatingController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Rating
        public ActionResult Index()
        {
            if (Session["uid"] != null)
            {
                int uid = Convert.ToInt32(HttpContext.Session["uid"]);
                var result = db.ratings.Where(x => x.userid.Equals(uid));
                if (result.Count() == 0)
                {
                    TempData["NotFound"] = "No Ratings Yet";
                }
                TempData["pdata"] = result;
                return View(db.ratings.ToList());
            }
            var ratings = db.ratings.Include(r => r.productionevent).Include(r => r.user);
            return View(ratings.ToList());
        }

        public ActionResult UrateView()
        {
            if (Session["aid"] != null)
            {
                return View(db.ratings.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult UrateView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.ratings.ToList());
                }
                List<rating> rates = db.ratings.Where(p => p.user.fname.Contains(Search)).ToList();
                if (rates.Count() == 0)
                {
                    TempData["NotFoundRate"] = "Data Not Found";
                }

                return View(rates.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Rating/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Rating/Create
        public ActionResult Create(int uid,int eid)
        {
            ViewBag.uid = uid;
            ViewBag.eid = eid;
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename");
            ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: Rating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rid,userid,peid,rating1,comment")] ratingv ratingv)
        {
            if (ModelState.IsValid)
            {
                rating rating = new rating();
                AutoMapper.Mapper.Map(ratingv, rating);

                db.ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Details","ProductionEvent",new { id=ratingv.peid});
            }

            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", ratingv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", ratingv.userid);
            return View(ratingv);
        }

        // GET: Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", rating.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", rating.userid);
            return View(rating);
        }

        // POST: Rating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rid,userid,peid,rating1,comment")] rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", rating.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", rating.userid);
            return View(rating);
        }

        // GET: Rating/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rating rating = db.ratings.Find(id);
            db.ratings.Remove(rating);
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
