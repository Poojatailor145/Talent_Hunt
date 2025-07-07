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
    public class EventRequireController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: EventRequire
        public ActionResult Index()
        {
            var eventrequires = db.eventrequires.Include(e => e.production).Include(e => e.productionevent).Include(e => e.talent);
            return View(eventrequires.ToList());
        }

        // GET: EventRequire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrequire eventrequire = db.eventrequires.Find(id);
            if (eventrequire == null)
            {
                return HttpNotFound();
            }
            return View(eventrequire);
        }

        // GET: EventRequire/Create
        public ActionResult Create(int? peid)
        {

            ViewBag.pid = new SelectList(db.productions, "pid", "pname");
            ViewBag.peid = peid;
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype");
            return View();
        }

        // POST: EventRequire/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "erid,pid,peid,tid,agerange,gender,payrange")] eventrequirev eventrequirev,int peid)
        {
            if (ModelState.IsValid)
            {
                eventrequirev.pid = Convert.ToInt32(HttpContext.Session["pid"]);
                eventrequirev.peid = peid;

                eventrequire eventrequire = new eventrequire();
                AutoMapper.Mapper.Map(eventrequirev, eventrequire);
                db.eventrequires.Add(eventrequire);
                db.SaveChanges();
                return RedirectToAction("Index","ProductionEvent");
            }

            ViewBag.pid = new SelectList(db.productions, "pid", "pname", eventrequirev.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventrequirev.peid);
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", eventrequirev.tid);
            return View(eventrequirev);
        }

        // GET: EventRequire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrequire eventrequire = db.eventrequires.Find(id);
            eventrequirev eventrequirev = new eventrequirev();
            AutoMapper.Mapper.Map(eventrequire, eventrequirev);
            if (eventrequire == null)
            {
                return HttpNotFound();
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", eventrequirev.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventrequirev.peid);
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", eventrequirev.tid);
            return View(eventrequirev);
        }

        // POST: EventRequire/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "erid,pid,peid,tid,agerange,gender,payrange")] eventrequirev eventrequirev)
        {
            if (ModelState.IsValid)
            {
                eventrequire eventrequire = new eventrequire();
                eventrequire.erid = eventrequirev.erid;
                eventrequire.pid= eventrequirev.pid;
                eventrequire.peid = eventrequirev.peid;
                eventrequire.tid = eventrequirev.tid;
                eventrequire.agerange = eventrequirev.agerange;
                eventrequire.gender = eventrequirev.gender;
                eventrequire.payrange = eventrequirev.payrange;


                db.Entry(eventrequire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","ProductionEvent",new { id=eventrequirev.peid});
            }
            ViewBag.pid = new SelectList(db.productions, "pid", "pname", eventrequirev.pid);
            ViewBag.peid = new SelectList(db.productionevents, "peid", "ename", eventrequirev.peid);
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", eventrequirev.tid);
            return View(eventrequirev);
        }

        // GET: EventRequire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventrequire eventrequire = db.eventrequires.Find(id);
            if (eventrequire == null)
            {
                return HttpNotFound();
            }
            return View(eventrequire);
        }

        // POST: EventRequire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eventrequire eventrequire = db.eventrequires.Find(id);
            db.eventrequires.Remove(eventrequire);
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
