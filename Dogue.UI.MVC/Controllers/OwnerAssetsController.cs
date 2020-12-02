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
using Microsoft.AspNet.Identity;

namespace Dogue.UI.MVC.Controllers
{
    public class OwnerAssetsController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: OwnerAssets
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Admin") || User.IsInRole("Admin") || User.IsInRole("Photographer"))
            {
                var ownerAssets = db.OwnerAssets.Include(o => o.OwnerInformation);
                return View(ownerAssets.ToList());
            }else
            {
                var currentUser = User.Identity.GetUserId();
                var userAsset = from o in db.OwnerAssets
                                  where o.UserID == currentUser
                                  select o;

                return View(userAsset.ToList());
            }
        }

        // GET: OwnerAssets/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName");
            return View();
        }

        // POST: OwnerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "OwnerAssetID,AssetRegisteredName,AssetCallName,AssetSpecies,AssetBreed,AssetAge,AssetSize,AssetTrainerCertified,UserID,AssetPhoto,SpecialNotes,IsActive,DateAdded,DescriptiveColorProfile")] OwnerAsset ownerAsset, HttpPostedFileBase myImg)
        {
            if (ModelState.IsValid)
            {
                #region PhotoUpload

                string imageName = "noImage.png";
                if (myImg != null)
                {
                    //Get File Extension - alternative to using a Substring () and Indexof()
                    string imgExt = System.IO.Path.GetExtension(myImg.FileName);

                    //list of allowed extentions
                    string[] allowedExtentions = { ".png", ".gif", ".jpg", ".jpeg" };
                    if (allowedExtentions.Contains(imgExt.ToLower()) && (myImg.ContentLength <= 4194304))
                    {
                        //using GUid for saved file name
                        imageName = Guid.NewGuid() + imgExt;
                        //creat some variables to pass to the resize image method
                        //Signature to resize Image
                        string savePath = Server.MapPath("~/Content/assets/Image/");

                        Image convertedImage = Image.FromStream(myImg.InputStream);
                        int maxImgSize = 1200;
                        int maxThumbSize = 100;
                        //calling this method will use the variables created above will save the full size image and thumbnail img to your server. 
                        UploadUtility.ResizeImage(savePath, imageName, convertedImage, maxImgSize, maxThumbSize);
                        ViewBag.PhotoMessage = "Photo Upload Complete.";
                    }
                    else
                    {
                        imageName = "noImage.png";
                    }
                    ownerAsset.AssetPhoto = imageName; 
                }
                
                #endregion

                    db.OwnerAssets.Add(ownerAsset);
                    db.SaveChanges();
              
                    return RedirectToAction("Index");
                
            }
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerAsset.UserID);
            return View(ownerAsset);

        }

   


        // GET: OwnerAssets/Edit/5
        [Authorize]
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
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerAsset.UserID);
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "OwnerAssetID,AssetRegisteredName,AssetCallName,AssetSpecies,AssetBreed,AssetAge,AssetSize,AssetTrainerCertified,UserID,AssetPhoto,SpecialNotes,IsActive,DateAdded,DescriptiveColorProfile")] OwnerAsset ownerAsset, HttpPostedFileBase myImg)
        {
            if (ModelState.IsValid)
            {
                #region PhotoUpload
                string imageName = "noImage.png";
                if (myImg != null)
                {
                    //Get File Extension - alternative to using a Substring () and Indexof()
                    string imgExt = System.IO.Path.GetExtension(myImg.FileName);

                    //list of allowed extentions
                    string[] allowedExtentions = { ".png", ".gif", ".jpg", ".jpeg" };
                    if (allowedExtentions.Contains(imgExt.ToLower()) && (myImg.ContentLength <= 4194304))
                    {
                        //using GUid for saved file name
                        imageName = Guid.NewGuid() + imgExt;
                        //creat some variables to pass to the resize image method
                        //Signature to resize Image
                        string savePath = Server.MapPath("~/Content/assets/Image/");
                        if (ownerAsset.AssetPhoto != null && ownerAsset.AssetPhoto != "noImage.png")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/asset/Image/" + Session["currentImage"].ToString()));
                        }
                        ownerAsset.AssetPhoto = imageName;
                        Image convertedImage = Image.FromStream(myImg.InputStream);
                        int maxImgSize = 1200;
                        int maxThumbSize = 100;
                        //calling this method will use the variables created above will save the full size image and thumbnail img to your server. 
                        UploadUtility.ResizeImage(savePath, imageName, convertedImage, maxImgSize, maxThumbSize);

                        ViewBag.PhotoMessage = "Photo Upload Complete.";
                    }

                }    
                
                #endregion

                db.Entry(ownerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.OwnerInformations, "UserID", "FullName", ownerAsset.UserID);
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset.AssetPhoto != null && ownerAsset.AssetPhoto != "noImage.png")
            {
                System.IO.File.Delete(Server.MapPath("~/Content/asset/Image/" + Session["currentImage"].ToString()));
            }
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
