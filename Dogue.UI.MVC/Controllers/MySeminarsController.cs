using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dogue.EF.DATA;
using Microsoft.AspNet.Identity;

namespace Dogue.UI.MVC.Controllers
{
    public class MySeminarsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: MySeminars
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Admin") || User.IsInRole("Admin") || User.IsInRole("Photographer"))
            {
                var mySeminars = db.MySeminars.Include(m => m.Seminar);
                return View(mySeminars.ToList());
            }
            else
            {
                var currentUser = User.Identity.GetUserId();
                var userMySem = from m in db.MySeminars
                            where m.UserID == currentUser
                            select m;

                return View(userMySem);
            }

        }

        // GET: MySeminars/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MySeminar mySeminar = db.MySeminars.Find(id);
            if (mySeminar == null)
            {
                return HttpNotFound();
            }
            return View(mySeminar);
        }

        // GET: MySeminars/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.SeminarID = new SelectList(db.Seminars, "SeminarID", "SeminarName");
            return View();
        }

        // POST: MySeminars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "MySeminarID,SeminarID")] MySeminar mySeminar)
        {
            if (ModelState.IsValid)
            {
                db.MySeminars.Add(mySeminar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SeminarID = new SelectList(db.Seminars, "SeminarID", "SeminarName", mySeminar.SeminarID);
            return View(mySeminar);
        }

        // GET: MySeminars/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MySeminar mySeminar = db.MySeminars.Find(id);
            if (mySeminar == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeminarID = new SelectList(db.Seminars, "SeminarID", "SeminarName", mySeminar.SeminarID);
            return View(mySeminar);
        }

        // POST: MySeminars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "MySeminarID,SeminarID")] MySeminar mySeminar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mySeminar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SeminarID = new SelectList(db.Seminars, "SeminarID", "SeminarName", mySeminar.SeminarID);
            return View(mySeminar);
        }

        // GET: MySeminars/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MySeminar mySeminar = db.MySeminars.Find(id);
            if (mySeminar == null)
            {
                return HttpNotFound();
            }
            return View(mySeminar);
        }

        // POST: MySeminars/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MySeminar mySeminar = db.MySeminars.Find(id);
            db.MySeminars.Remove(mySeminar);
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
