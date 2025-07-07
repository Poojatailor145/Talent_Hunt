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
    public class TalentController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Talent
        public ActionResult Index()
        {
            return View(db.talents.ToList());
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.talents.ToList());
                }
                List<talent> talent = db.talents.Where(p => p.ttype.Contains(Search) || Search == null).ToList();
                if (talent.Count() == 0)
                {
                    TempData["NotFound"] = "Data Not Found";
                }

                return View(talent.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Talent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            talent talent = db.talents.Find(id);
            if (talent == null)
            {
                return HttpNotFound();
            }
            return View(talent);
        }

        // GET: Talent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Talent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tid,ttype")] talentv talentv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    List<talent> talent = db.talents.Where(p => p.ttype == talentv.ttype).ToList();
                    if (talent.Count() == 0)
                    {
                        talent talent1 = new talent();
                        AutoMapper.Mapper.Map(talentv, talent1);

                        db.talents.Add(talent1);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    TempData["AlreadyExists"] = "Talent Already Exists";
                }

                return View(talentv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Talent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            talent talent = db.talents.Find(id);
            talentv talentv = new talentv();
            AutoMapper.Mapper.Map(talent, talentv);
            if (talent == null)
            {
                return HttpNotFound();
            }
            return View(talentv);
        }

        // POST: Talent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tid,ttype")] talentv talentv)
        {
            if (Session["aid"] != null)
            {
                if (ModelState.IsValid)
                {
                    talent talent = new talent();
                    AutoMapper.Mapper.Map(talentv, talent);

                    db.Entry(talent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(talentv);
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Talent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                talent talent= db.talents.Find(id);
                if (talent != null)
                {
                    db.talents.Remove(talent);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        TempData["talenterr"] = "Talent cannot be deleted because users are available with this talent";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Talent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            talent talent = db.talents.Find(id);
            db.talents.Remove(talent);
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
