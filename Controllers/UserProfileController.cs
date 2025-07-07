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
    public class UserProfileController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: UserProfile
        public ActionResult Index()
        {
            var userprofiles = db.userprofiles.Include(u => u.talent).Include(u => u.user);
            return View(userprofiles.ToList());
        }

        // GET: UserProfile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userprofile userprofile = db.userprofiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        // GET: UserProfile/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.uid = id;
            List<userprofile> talentCreated = db.userprofiles.Where(p => p.userid == id).ToList();
            if (talentCreated.Count() == 0)
            {
                ViewBag.tid = new SelectList(db.talents, "tid", "ttype");
            }
            else
            {
                List<talent> talent = db.talents.ToList();

                List<talent> talentRemain = talent.Where(t => talentCreated.All(p => p.tid != t.tid)).ToList();

                ViewBag.tid = new SelectList(talentRemain, "tid", "ttype");
            }
            //ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: UserProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "upid,userid,tid,experience,portfolio")] userprofilev userprofilev,int uid)
        {
            if (ModelState.IsValid)
            {
              
                userprofilev.userid = uid;
                userprofile userprofile = new userprofile();
                AutoMapper.Mapper.Map(userprofilev, userprofile);
                db.userprofiles.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Profile","User");
            }

            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", userprofilev.tid);
            //ViewBag.userid = new SelectList(db.users, "userid", "fname", userprofile.userid);
            return View(userprofilev);
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userprofile userprofile = db.userprofiles.Find(id);
            userprofilev userprofilev = new userprofilev();
            AutoMapper.Mapper.Map(userprofile, userprofilev);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            ViewBag.upid = id;
            ViewBag.ttid = userprofilev.tid;
            ViewBag.uuserid = userprofilev.userid;
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", userprofilev.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userprofilev.userid);
            return View(userprofilev);
        }

        // POST: UserProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "upid,userid,tid,experience,portfolio")] userprofilev userprofilev)
        {
            if (ModelState.IsValid)
            {
                userprofile userprofile = new userprofile();
                userprofile.upid = userprofilev.upid;
                userprofile.userid = userprofilev.userid;
                userprofile.tid = userprofilev.tid;
                userprofile.experience = userprofilev.experience;
                userprofile.portfolio = userprofilev.portfolio;
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile","User");
            }
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", userprofilev.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userprofilev.userid);
            return View(userprofilev);
        }

        // GET: UserProfile/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userprofile userprofile = db.userprofiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        // POST: UserProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userprofile userprofile = db.userprofiles.Find(id);
            db.userprofiles.Remove(userprofile);
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
