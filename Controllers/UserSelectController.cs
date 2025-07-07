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
    public class UserSelectController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: UserSelect
        public ActionResult Index()
        {
            var userselects = db.userselects.Include(u => u.productionevent).Include(u => u.user);
            return View(userselects.ToList());
        }

        // GET: UserSelect/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userselect userselect = db.userselects.Find(id);
            if (userselect == null)
            {
                return HttpNotFound();
            }
            return View(userselect);
        }

        // GET: UserSelect/Create
        public ActionResult Create(userselectv userselectv, int uid, int eid, int pay)
        {

            if (ModelState.IsValid)
            {
                userselectv.userid = uid;
                userselectv.peid = eid;
                userselectv.finalpay = pay;

                userselect userselect = new userselect();
                AutoMapper.Mapper.Map(userselectv, userselect);
                db.userselects.Add(userselect);

                userapply userapply = db.userapplies.Where(p=>p.peid.Equals(userselectv.peid) && p.userid.Equals(userselectv.userid)).SingleOrDefault();
                userapply.status = "selected";
                db.Entry(userapply).State = EntityState.Modified;

                db.SaveChanges();
                               
                return RedirectToAction("Details", "ProductionEvent", new { id = eid });
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename");
            ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: UserSelect/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usid,peid,userid,finalpay")] userselectv userselectv)
        {
            if (ModelState.IsValid)
            {
                userselect userselect = new userselect();
                AutoMapper.Mapper.Map(userselectv, userselect);
                db.userselects.Add(userselect);
                db.SaveChanges();
                return RedirectToAction("Details","ProductionEvent");
            }

            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userselectv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userselectv.userid);
            return View(userselectv);
        }

        // GET: UserSelect/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userselect userselect = db.userselects.Find(id);
            userselectv userselectv = new userselectv();
            AutoMapper.Mapper.Map(userselect, userselectv);
            if (userselect == null)
            {
                return HttpNotFound();
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userselectv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userselectv.userid);
            return View(userselectv);
        }

        // POST: UserSelect/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usid,peid,userid,finalpay")] userselectv userselectv)
        {
            if (ModelState.IsValid)
            {
                userselect userselect = new userselect();
                AutoMapper.Mapper.Map(userselectv, userselect);
                db.Entry(userselect).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", userselectv.peid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", userselectv.userid);
            return View(userselectv);
        }

        // GET: UserSelect/Delete/5
        public ActionResult Delete(int? id, int? peid)
        {
            if (Session["pid"] != null)
            {
                if (id == null || peid == null)
                {
                    return RedirectToAction("Details", "ProductionEvent", new { id = peid });
                }
                userselect userselect = db.userselects.Where(p => p.userid == id && p.peid == peid).FirstOrDefault();
                if (userselect != null)
                {
                    userapply userapply = db.userapplies.Where(p => p.userid.Equals(userselect.userid) && p.peid.Equals(userselect.peid)).SingleOrDefault();
                    if (userapply == null)
                    {
                        return RedirectToAction("Details", "ProductionEvent", new { id = peid });
                    }
                    userapply.status = "applied";
                    db.Entry(userapply).State = EntityState.Modified;
                    db.userselects.Remove(userselect);
                    db.SaveChanges();
                    return RedirectToAction("Details", "ProductionEvent", new { id = peid });
                }
                else
                {
                    return RedirectToAction("Details", "ProductionEvent", new { id = peid });
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        // POST: UserSelect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userselect userselect = db.userselects.Find(id);
            db.userselects.Remove(userselect);
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
