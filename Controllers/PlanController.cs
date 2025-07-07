
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
    public class PlanController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Plan
        public ActionResult Index()
        {
            if(Session["aid"]!=null)
            {
                return View(db.plans.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            
        }

        public ActionResult planselect()
        {
            return View(db.plans.ToList());
        }

        public ActionResult upgradeplan()
        {
            return View(db.plans.ToList());
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
                    return View(db.plans.ToList());
                }
                List<plan> planTS = db.plans.Where(p => p.plantype.Contains(Search)).ToList();
                if (planTS.Count() == 0)
                {
                    try
                    {
                        int s = Convert.ToInt32(Search);
                        List<plan> planLnS = db.plans.Where(p => p.price <= s).ToList();
                        if (planLnS.Count() == 0)
                        {
                            TempData["pNotFound"] = "Plan Not Found";
                        }
                        else
                        {
                            return View(planLnS.ToList());
                        }
                    }
                    catch
                    {
                        TempData["pNotFound"] = "Plan Not Found";

                    }
                }
                else
                {
                    return View(planTS.ToList());
                }

                return View(planTS.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // GET: Plan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            plan plan = db.plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // GET: Plan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Plan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "planid,plantype,duration,price,maxevents,maxbids,description,benefits")] planv planv)
        {
            if (ModelState.IsValid)
            {
                plan plan = new plan();
                AutoMapper.Mapper.Map(planv, plan);
                db.plans.Add(plan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(planv);
        }

        // GET: Plan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            plan plan = db.plans.Find(id);
            planv planv = new planv();
            AutoMapper.Mapper.Map(plan, planv);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(planv);
        }

        // POST: Plan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "planid,plantype,duration,price,maxevents,maxbids,description,benefits")] planv planv)
        {
            if (ModelState.IsValid)
            {
                plan plan = new plan();
                AutoMapper.Mapper.Map(planv, plan);

                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(planv);
        }

        // GET: Plan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["aid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                plan plan= db.plans.Find(id);
                if (plan != null)
                {
                    db.plans.Remove(plan);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        TempData["planerr"] = "Plan cannot be deleted because users are subscribed to this plan";
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

        // POST: Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            plan plan = db.plans.Find(id);
            db.plans.Remove(plan);
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
