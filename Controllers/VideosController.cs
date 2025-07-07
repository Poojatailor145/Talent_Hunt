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
    public class VideosController : Controller
    {
        private huntdbEntities1 db = new huntdbEntities1();

        // GET: Videos
        public ActionResult Index()
        {
            var videos = db.videos.Include(v => v.talent).Include(v => v.user);
            return View(videos.ToList());
        }

        // GET: Videos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            video video = db.videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Videos/Create
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

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "vid,userid,tid,video1,caption,VideoFile")] videov videov,int uid)
        {
            if (ModelState.IsValid)
            {
                if (videov.tid == 0 && videov.VideoFile == null)
                {
                    ViewBag.tiderr = "*";
                    ViewBag.emptyerr = "*";
                }
                else
                {
                    if (videov.tid == 0)
                    {
                        ViewBag.tiderr = "*";
                    }
                    else
                    {
                        if (videov.VideoFile == null)
                        {
                            ViewBag.emptyerr = "*";
                        }
                        else if (videov.VideoFile.ContentType == "video/mp4")
                        {
                            string fileName = Path.GetFileNameWithoutExtension(videov.VideoFile.FileName);
                            string extension = Path.GetExtension(videov.VideoFile.FileName);
                            fileName = fileName + extension;
                            videov.video1 = "~/Videos/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Videos"), fileName);
                            videov.VideoFile.SaveAs(fileName);

                            videov.userid = uid;

                            video video = new video();
                            AutoMapper.Mapper.Map(videov, video);

                            db.videos.Add(video);
                            db.SaveChanges();
                            return RedirectToAction("Profile", "User");
                        }
                        else
                        {
                            ViewBag.mp4err = "Must be mp4";
                        }

                    }
                }
            }

            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", videov.tid);
            ViewBag.uid = uid;
            int usrid = Convert.ToInt32(uid);
            var talents = db.talents.ToList();
            var finallist = db.userprofiles.Where(p => p.userid.Equals(usrid));
            TempData["talents"] = finallist;
            return View(videov);
        }
    

        // GET: Videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            video video = db.videos.Find(id);
            videov videov = new videov();
            AutoMapper.Mapper.Map(video, videov);
            if (video == null)
            {
                return HttpNotFound();
            }
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", videov.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", videov.userid);
            return View(videov);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vid,userid,tid,video1,caption")] videov videov)
        {
            if (ModelState.IsValid)
            {
                video video = new video();
                AutoMapper.Mapper.Map(videov, video);
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tid = new SelectList(db.talents, "tid", "ttype", videov.tid);
            ViewBag.userid = new SelectList(db.users, "userid", "fname", videov.userid);
            return View(videov);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return RedirectToAction("Profile", "User");
                }
                video video = db.videos.Find(id);
                if (video != null)
                {
                    db.videos.Remove(video);
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

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            video video = db.videos.Find(id);
            db.videos.Remove(video);
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
