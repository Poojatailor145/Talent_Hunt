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
    public class ProductionController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Production
        public ActionResult Index()
        {
            
            return View(db.productions.ToList());
        }

        [HttpPost]

        public ActionResult Index(string Search)
        {
            if (Session["uid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productions.ToList());
                }


                List<production> eventNS = db.productions.Where(p => p.pname.Contains(Search) || Search == null).ToList();
                if (eventNS.Count() == 0)
                {
                    List<production> eventDS = db.productions.Where(p => p.description.Contains(Search) || Search == null).ToList();
                    if (eventDS.Count() == 0)
                    {
                        List<production> eventHS = db.productions.Where(p => p.phead.Contains(Search) || Search == null).ToList();
                        if (eventHS.Count() == 0)
                        {
                            TempData["NotFound"] = "Data Not Found";
                        }
                        else
                        {
                            return View(eventHS.ToList());
                        }
                    }
                    else
                    {
                        return View(eventDS.ToList());
                    }
                }
                else
                {
                    return View(eventNS.ToList());
                }

                return View(eventNS.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult EditStatus(int? id, string status)
        {
            if (id == null || status == null)
            {
                return RedirectToAction("UserView", "User");
            }
            production production= db.productions.Find(id);

            if (status == "blocked")
            {
                production.status = status;
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductionView", "Production");
            }
            else if (status == "active")
            {
                production.status = status;
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductionView", "Production");
            }
            else
            {
                return RedirectToAction("ProductionView", "Production");
            }
        }

        public ActionResult Proinfo()
        {
            if (Session["pid"] != null)
            {
                int pid = Convert.ToInt32(HttpContext.Session["pid"]);
                var result = db.productions.Where(x => x.pid.Equals(pid));
                TempData["pdata"] = result;

                //PLAN VALIDITY
                var prplan = db.subproductions.Where(p => p.pid.Equals(pid));
                var evcount = db.productionevents.Where(p => p.pid.Equals(pid));
                var datenow = DateTime.Today;
                if(prplan.Count()==0)
                {
                    TempData["max"] = "Yes";
                }
                else
                {
                    TempData["max"] = "No";
                }
                foreach (var pl in prplan)
                {
                    if (evcount.Count() >= pl.plan.maxevents && datenow > pl.enddate)
                    {
                        TempData["max"] = "Yes";
                    }
                    else
                    {
                        TempData["max"] = "No";
                    }
                }
                return View(db.productions.ToList());
            }
            else
            {
                return RedirectToAction("Login","User");
            }
        }

        public ActionResult PrShortView(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(Session["aid"]!=null)
            {
                production production = db.productions.Find(id);
                if (production == null)
                {
                    return HttpNotFound();
                }
                return View(production);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }




        // GET: Production/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            int pid = Convert.ToInt32(id);
            var result = db.productionevents.Where(p => p.pid.Equals(pid) && p.status.Equals("active"));
            if(result.Count()==0)
            {
                TempData["NotFound"] = "No events registered from this production";
            }
            TempData["events"] = result;
            
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: Production/Create
        public ActionResult Create(int id)
        {
            if(id==0)
            {
                return RedirectToAction("planselect", "Plan");
            }
            else
            {
                string name = db.plans.Where(x => x.planid == id).SingleOrDefault()?.plantype;
                string duration = db.plans.Where(x => x.planid == id).SingleOrDefault()?.duration;
                int price = db.plans.Where(x => x.planid == id).SingleOrDefault().price;

                ViewBag.id = id;
                ViewBag.name = name;
                ViewBag.duration = duration;
                ViewBag.price= price;
            }
            return View();
        }

        public ActionResult ProductionView()
        {
            if (Session["aid"] != null)
            {
                return View(db.productions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult ProductionView(string Search)
        {
            if (Session["aid"] != null)
            {
                if (Search == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Search.ToLower() == "all" || Search == "")
                {
                    return View(db.productions.ToList());
                }
                List<production> productions= db.productions.Where(p => p.pname.Contains(Search) || Search == null).ToList();
                if (productions.Count() == 0)
                {
                    List<production> eventHS = db.productions.Where(p => p.phead.Contains(Search)).ToList();
                    if (eventHS.Count() == 0)
                    {
                        TempData["NotFound"] = "Data Not Found";
                    }
                    else
                    {
                        return View(eventHS.ToList());
                    }
                }
                else
                {
                    return View(productions.ToList());
                }

                return View(productions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "AdminLogin");
            }
        }

        // POST: Production/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pid,pname,pimage,phead,address,contactno,email,username,password,description,ImageFile,status")] productionv productionv,subproductionv subproductionv,int planid)
        {
            if (ModelState.IsValid)
            {
                if (productionv.ImageFile == null)
                {
                    ViewBag.emptyerr = "*";
                }
                else if (productionv.ImageFile.ContentType == "image/jpeg" || productionv.ImageFile.ContentType == "image/png" || productionv.ImageFile.ContentType == "image/jpg")
                {
                    string fileName = Path.GetFileNameWithoutExtension(productionv.ImageFile.FileName);
                    string extension = Path.GetExtension(productionv.ImageFile.FileName);
                    fileName = fileName + extension;
                    productionv.pimage = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    productionv.ImageFile.SaveAs(fileName);
                    productionv.status = "active";

                    int Duration = Convert.ToInt32(db.plans.Where(x => x.planid == planid).SingleOrDefault()?.duration);

                    subproductionv.planid = planid;
                    subproductionv.pid = productionv.pid;
                    subproductionv.startdate = DateTime.Now;
                    subproductionv.enddate = DateTime.Now.AddMonths(Duration);

                    subproduction subproduction = new subproduction();
                    production production = new production();

                    AutoMapper.Mapper.Map(subproductionv, subproduction);
                    AutoMapper.Mapper.Map(productionv, production);

                    db.subproductions.Add(subproduction);
                    db.productions.Add(production);
                    db.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.picformat = "Invalid Format";
                }
            }
            string name = db.plans.Where(x => x.planid == planid).SingleOrDefault()?.plantype;
            string duration = db.plans.Where(x => x.planid == planid).SingleOrDefault()?.duration;
            int price = db.plans.Where(x => x.planid == planid).SingleOrDefault().price;

            ViewBag.id = planid;
            ViewBag.name = name;
            ViewBag.duration = duration;
            ViewBag.price = price;
            return View(productionv);
        }

        // GET: Production/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            productionv productionv = new productionv();
            AutoMapper.Mapper.Map(production, productionv);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(productionv);
        }

        // POST: Production/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,pname,pimage,phead,address,contactno,email,username,password,description,status")] productionv productionv)
        {
            if (ModelState.IsValid)
            {
                production production = new production();
                production.pid = productionv.pid;
                production.pname = productionv.pname;
                production.pimage = productionv.pimage;
                production.phead = productionv.phead;
                production.address = productionv.address;
                production.contactno = productionv.contactno;
                production.email = productionv.email;
                production.username = productionv.username;
                production.password = productionv.password;
                production.description = productionv.description;
                production.status = productionv.status;
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Proinfo");
            }
            return View(productionv);
        }

        // GET: Production/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Production/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            production production = db.productions.Find(id);
            db.productions.Remove(production);
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
