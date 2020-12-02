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
    public class OwnerInformationsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: OwnerInformations
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Admin") || User.IsInRole("Admin") || User.IsInRole("Photographer"))
            {
                var ownerInformations = db.OwnerInformations;
                return View(ownerInformations.ToList());
            }
            else
            {
                var currentUser = User.Identity.GetUserId();
                var user = (from o in db.OwnerInformations
                                where o.UserID == currentUser
                                select o).FirstOrDefault();

                return View(user);
            }

        }

        // GET: OwnerInformations/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerInformation ownerInformation = db.OwnerInformations.Find(id);
            if (ownerInformation == null)
            {
                return HttpNotFound();
            }
            return View(ownerInformation);
        }

        // GET: OwnerInformations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName");
            return View();
        }

        // POST: OwnerInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,MainPhoneNumber,SecondaryPhoneNumber,Email,Address,City,State,ZipCode")] OwnerInformation ownerInformation)
        {
            if (ModelState.IsValid)
            {
                db.OwnerInformations.Add(ownerInformation);
                db.SaveChanges();
                //TempData["assetMessage"] = "You have successfully entered your Owner Information.  Please fill in the information below for the Animal Client.";
                return RedirectToAction("Create", "OwnerAssets");
            }

            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // GET: OwnerInformations/Edit/5
        [HttpGet]
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerInformation ownerInformation = db.OwnerInformations.Find(id);
            if (ownerInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // POST: OwnerInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,MainPhoneNumber,SecondaryPhoneNumber,Email,Address,City,State,ZipCode")] OwnerInformation ownerInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // GET: OwnerInformations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerInformation ownerInformation = db.OwnerInformations.Find(id);
            if (ownerInformation == null)
            {
                return HttpNotFound();
            }
            return View(ownerInformation);
        }

        // POST: OwnerInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OwnerInformation ownerInformation = db.OwnerInformations.Find(id);
            db.OwnerInformations.Remove(ownerInformation);
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
