using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dogue.EF.DATA;

namespace Dogue.UI.MVC.Controllers
{
    public class OwnerInformationsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: OwnerInformations
        public ActionResult Index()
        {
            var ownerInformations = db.OwnerInformations.Include(o => o.SiteUser);
            return View(ownerInformations.ToList());
        }

        // GET: OwnerInformations/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.SiteUsers, "UserID", "FirstName");
            return View();
        }

        // POST: OwnerInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerID,UserID,FirstName,LastName,MainPhoneNumber,SecondaryPhoneNumber,Email,Address,City,State,ZipCode,TransactionFileUpToDate")] OwnerInformation ownerInformation)
        {
            if (ModelState.IsValid)
            {
                db.OwnerInformations.Add(ownerInformation);
                db.SaveChanges();
                TempData["assetMessage"] = "You have successfully entered your Owner Information.  Please fill in the information below for the Animal Client.";
                return RedirectToAction("Create", "OwnerAssets");
            }

            ViewBag.UserID = new SelectList(db.SiteUsers, "UserID", "FirstName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // GET: OwnerInformations/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.UserID = new SelectList(db.SiteUsers, "UserID", "FirstName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // POST: OwnerInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerID,UserID,FirstName,LastName,MainPhoneNumber,SecondaryPhoneNumber,Email,Address,City,State,ZipCode,TransactionFileUpToDate")] OwnerInformation ownerInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.SiteUsers, "UserID", "FirstName", ownerInformation.UserID);
            return View(ownerInformation);
        }

        // GET: OwnerInformations/Delete/5
        public ActionResult Delete(int? id)
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
