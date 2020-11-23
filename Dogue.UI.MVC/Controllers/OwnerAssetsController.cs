using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dogue.EF.DATA;
using Dogue.UI.MVC.Utilities;

namespace Dogue.UI.MVC.Controllers
{
    public class OwnerAssetsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: OwnerAssets
        public ActionResult Index()
        {
            var ownerAssets = db.OwnerAssets.Include(o => o.OwnerInformation);
            return View(ownerAssets.ToList());
        }

        // GET: OwnerAssets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.OwnerInformations, "OwnerID", "FirstName");
            return View();
        }

        // POST: OwnerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerAssetID,AssetRegisteredName,AssetCallName,AssetSpecies,AssetBreed,AssetAge,AssetSize,AssetTrainerCertified,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded,DescriptiveColorProfile")] OwnerAsset ownerAsset)
        {
            if (ModelState.IsValid)
            {
                db.OwnerAssets.Add(ownerAsset);
                db.SaveChanges();
                //TempData["ThankMessage"] = "Thank you. You have successfully completed full registration.  You may now take advanatage of all our client privileges.";
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.OwnerInformations, "OwnerID", "FirstName", ownerAsset.OwnerID);
            return View(ownerAsset);
        }

        [HttpPost]
        public ActionResult UploadPost(HttpPostedFileBase myImg)
        {
            string imageName = "NoImage.png";
            if (myImg != null)
            {
                //Get File Extension - alternative to using a Substring () and Indexof()
                string imgExt = System.IO.Path.GetExtension(myImg.FileName);

                //list of allowed extentions
                string[] allowedExtentions = { ".png, .gif, .jpg., .jpeg" };
                if (allowedExtentions.Contains(imgExt))
                {
                    //using GUid for saved file name
                    imageName = Guid.NewGuid() + imgExt;
                    //creat some variables to pass to the resize image method
                    //Signature to resize Image
                    string savePath = Server.MapPath("~/Content/Images/");

                    Image convertedImage = Image.FromStream(myImg.InputStream);
                    int maxImgSize = 2800;
                    int maxThumbSize = 100;
                    //calling this method will use the variables created above will save the full size image and thumbnail img to your server. 
                    UploadUtility.ResizeImage(savePath, imageName, convertedImage, maxImgSize, maxThumbSize);

                }


            }

            TempData["PhotoMessage"] = "Photo Upload Complete.";
            return RedirectToAction("Create")
        ;}
        

        // GET: OwnerAssets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(db.OwnerInformations, "OwnerID", "UserID", ownerAsset.OwnerID);
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerAssetID,AssetRegisteredName,AssetCallName,AssetSpecies,AssetBreed,AssetAge,AssetSize,AssetTrainerCertified,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded,DescriptiveColorProfile")] OwnerAsset ownerAsset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.OwnerInformations, "OwnerID", "UserID", ownerAsset.OwnerID);
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            db.OwnerAssets.Remove(ownerAsset);
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
