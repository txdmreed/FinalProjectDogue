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
    public class ReservationsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: Reservations
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Agent"))
            {
                var reservations = db.Reservations.Include(r => r.Location).Include(r => r.OwnerAsset).Include(r => r.Service);
                return View(reservations.ToList());
            }
            else if (User.IsInRole("Photographer"))
            {
                //var photoReserve = db.Reservations.Include(r => r.Location).Include(r => r.OwnerAsset).Include(r => r.ServiceID).Where(r => r.ServiceID == 1);
                var photoReserve = from r in db.Reservations
                              where r.ServiceID == 1
                              select r;

                return View(photoReserve.ToList());

            }
            else if (User.IsInRole("Client"))
            {
                var currentUser = User.Identity.GetUserId();
                var userReserve = from r in db.Reservations
                                  where r.OwnerAsset.UserID == currentUser
                                  select r;

                return View(userReserve.ToList());
            }
            else
            {
                ViewBag.RedirectMessage = "The page you were trying to access is only available to registered users.";
                return RedirectToAction("Home", "Index");
            }


        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            var currentUser = User.Identity.GetUserId();
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            if (User.IsInRole("Admin") || User.IsInRole("Agent"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName");
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName");
            }
            else if (User.IsInRole("Photographer"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName");
                ViewBag.ServiceID = new SelectList(db.Services.Where(r => r.ServiceID == 1), "ServiceID", "ServiceName");
            }
            else
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName");
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName");
            }
       
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,OwnerAssetID,LocationID,ReservationDate,ServiceID")] Reservation reservation)
        {
            var currentUser = User.Identity.GetUserId();
            //if (User.IsInRole("Client") || User.IsInRole("Agent"))

            if (ModelState.IsValid)
            {
                

                    var phoShoots = (from l in db.Locations
                                     where l.LocationID == reservation.LocationID
                                     select l).FirstOrDefault();

                    var phoResCount = from r in db.Reservations
                                      where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 1
                                      select r;

                    var trainResCount = from r in db.Reservations
                                        where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 2
                                        select r;

                    var groomResCount = from r in db.Reservations
                                        where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 3
                                        select r;
                    var phoResCountCheck = phoShoots.PhotoReservationLimit - phoResCount.Count();
                    var trainResCountCheck = phoShoots.TrainerReservationLimit - trainResCount.Count();
                    var groomResCountCheck = phoShoots.GroomerReservationLimit - groomResCount.Count();
                    if (reservation.ServiceID == 1 && phoResCountCheck > 0)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else if (reservation.ServiceID == 2 && trainResCountCheck > 0)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else if (reservation.ServiceID == 3 && groomResCountCheck > 0)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (User.IsInRole("Admin"))
                    {
                        ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
                        ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                        ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
                        return PartialView("OverbookedCreate", reservation);
                        
                    }
                        ViewBag.ReservationMessage = "We're sorry but the reservation limit for that day and/or service has been reached. Please, make new selections.";
                        ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
                        ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                        ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);

                        return View(reservation);
                    }
                
            }

            ViewBag.ReservationMessage = "We're sorry, but an error has occured.  Please, try again.";
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            if (User.IsInRole("Admin") || User.IsInRole("Agent"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else if (User.IsInRole("Photographer"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services.Where(r => r.ServiceID == 1), "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            return View(reservation);

        }
     
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OverbookedCreate([Bind(Include = "ReservationID,OwnerAssetID,LocationID,ReservationDate,ServiceID")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            return PartialView("OverbookedCreate",reservation);
        }
    


        // GET: Reservations/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin, Agent, Photographer")]
        public ActionResult Edit(int? id)
        {
            var currentUser = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            if (User.IsInRole("Admin") || User.IsInRole("Agent"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else if (User.IsInRole("Photographer"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services.Where(r => r.ServiceID == 1), "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Agent, Photographer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,OwnerAssetID,LocationID,ReservationDate,ServiceID")] Reservation reservation)
        {
            var currentUser = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var phoShoots = (from l in db.Locations
                                 where l.LocationID == reservation.LocationID
                                 select l).FirstOrDefault();

                var phoResCount = from r in db.Reservations
                                  where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 1
                                  select r;

                var trainResCount = from r in db.Reservations
                                    where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 2
                                    select r;

                var groomResCount = from r in db.Reservations
                                    where reservation.ReservationDate == r.ReservationDate && reservation.ServiceID == 3
                                    select r;
                var phoResCountCheck = phoShoots.PhotoReservationLimit - phoResCount.Count();
                var trainResCountCheck = phoShoots.TrainerReservationLimit - trainResCount.Count();
                var groomResCountCheck = phoShoots.GroomerReservationLimit - groomResCount.Count();
                if (reservation.ServiceID == 1 && phoResCountCheck > 0)
                {
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (reservation.ServiceID == 2 && trainResCountCheck > 0)
                {
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (reservation.ServiceID == 3 && groomResCountCheck > 0)
                {
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
                        ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                        ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
                        return PartialView("OverbookedEdit", reservation);

                    }
                    ViewBag.ReservationMessage = "We're sorry but the reservation limit for that day and/or service has been reached. Please, make new selections.";
                    ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
                    ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                    ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);

                    return View(reservation);
                }
            }
            ViewBag.ReservationMessage = "We're sorry, but an error has occured.  Please, try again.";
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            if (User.IsInRole("Admin") || User.IsInRole("Agent"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else if (User.IsInRole("Photographer"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services.Where(r => r.ServiceID == 1), "ServiceID", "ServiceName", reservation.ServiceID);
            }
            else
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(r => r.UserID == currentUser), "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            }
            return View(reservation);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OverbookedEdit([Bind(Include = "ReservationID,OwnerAssetID,LocationID,ReservationDate,ServiceID")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", reservation.OwnerAssetID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "ServiceName", reservation.ServiceID);
            return PartialView("OverbookedCreate", reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
