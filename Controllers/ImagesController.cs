using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentHunt.Models;
using TalentHunt.ModelView;

namespace TalentHunt.Controllers
{
    public class ImagesController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Images
        public ActionResult Index()
        {
            var images = db.images.Include(i => i.talent).Include(i => i.user);
            return View(images.ToList());
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create(int? id)
        {

            ViewBag.uid = id;
            int usrid = Convert.ToInt32(id);
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype");
            var talents = db.talents.ToList();
            var finallist = db.userprofiles.Where(p => p.userid.Equals(usrid));

            TempData["talents"] = finallist;
            ViewBag.userid = new SelectList(db.users, "userid", "fname");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iid,userid,tid,image1,caption,ImageFile")] imagev imagev,int uid)
        {
            if (ModelState.IsValid)
            {
                if (imagev.tid == 0 && imagev.ImageFile == null)
                {
                    ViewBag.tiderr = "*";
                    ViewBag.emptyerr = "*";
                }
                else
                {
                    if (imagev.tid == 0)
                    {
                        ViewBag.tiderr = "*";
                    }
                    else
                    {
                        if (imagev.ImageFile == null)
                        {
                            ViewBag.emptyerr = "*";
                        }
                        else if (imagev.ImageFile.ContentType == "image/jpeg" || imagev.ImageFile.ContentType == "image/png" || imagev.ImageFile.ContentType == "image/jpg")
                        {
                            string fileName = Path.GetFileNameWithoutExtension(imagev.ImageFile.FileName);
                            string extension = Path.GetExtension(imagev.ImageFile.FileName);
                            fileName = fileName + extension;
                            imagev.image1 = "~/Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Images"), fileName);

                            imagev.ImageFile.SaveAs(fileName);
                            imagev.userid = uid;

                            image image = new image();
                            AutoMapper.Mapper.Map(imagev, image);

                            db.images.Add(image);
                            db.SaveChanges();
                            return RedirectToAction("Profile", "User");
                        }
                        else
                        {
                            ViewBag.picformat = "Only jpeg/png/jpg format";
                        }
                    }
                }
            }

            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", imagev.tid);

            ViewBag.uid = uid;
            int usrid = Convert.ToInt32(uid);
            var talents = db.talents.ToList();
            var finallist = db.userprofiles.Where(p => p.userid.Equals(usrid));
            TempData["talents"] = finallist;
            return View(imagev);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.images.Find(id);
            imagev imagev = new imagev();
            AutoMapper.Mapper.Map(image, imagev);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", imagev.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", imagev.userid);
            return View(imagev);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iid,userid,tid,image1,caption")] imagev imagev)
        {
            if (ModelState.IsValid)
            {
                image image = new image();
                AutoMapper.Mapper.Map(imagev, image);
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", imagev.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", imagev.userid);
            return View(imagev);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Profile", "User");
                }
                image image = db.images.Find(id);

                if (image != null)
                {
                    db.images.Remove(image);
                    db.SaveChanges();
                    return RedirectToAction("Profile", "User");
                }
                else
                {
                    return RedirectToAction("Profile", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            image image = db.images.Find(id);
            db.images.Remove(image);
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
