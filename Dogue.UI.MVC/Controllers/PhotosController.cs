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
using Dogue.UI.MVC.Models;
using Dogue.UI.MVC.Utilities;

namespace Dogue.UI.MVC.Controllers
{
    public class PhotosController : Controller
    {
        private DogueFinalProjectEntities db = new DogueFinalProjectEntities();

        // GET: Photos
        [HttpGet]
        public ActionResult Index()
        {
            
            var photos = db.Photos.Include(p => p.Filter).Include(p => p.OwnerAsset);

            return View(photos.ToList());
        }
        [HttpGet]
        public ActionResult PhotosTileView()
        {
            var photos = db.Photos.Include(p => p.Filter).Include(p => p.OwnerAsset);
            return View(photos.ToList());
        }

        [HttpGet]
        public ActionResult Portfolio()
        {
            List<PortfolioViewModel> pvm = new List<PortfolioViewModel>();
            var allPhotos = (from photo in db.Photos
                             select new { photo.PhotoID, photo.Title, photo.PhotoUrl, photo.FilterID, photo.Filter.FilterName, photo.OwnerAssetID, photo.OwnerAsset.AssetCallName, }).ToList();
            foreach (var item in allPhotos)
            {
                PortfolioViewModel objPVM = new PortfolioViewModel();

                objPVM.PhotoID = item.PhotoID;
                objPVM.FilterName = item.FilterName;
                objPVM.PhotoUrl = item.PhotoUrl;
                objPVM.Title = item.Title;
                objPVM.AssetCallName = item.AssetCallName;
                objPVM.FilterID = item.FilterID;

                pvm.Add(objPVM);
            }
            return View(pvm.ToList());
        }

        // GET: Photos/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        [HttpGet]
        [Authorize(Roles = "Admin, Photographer")]
        public ActionResult Create()
        {
            ViewBag.FilterID = new SelectList(db.Filters, "FilterID", "FilterName");
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetRegisteredName");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Photographer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotoID,Title,PhotoUrl,FilterID,OwnerAssetID")] Photo photo, HttpPostedFileBase myImg)
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
                        string savePath = Server.MapPath("~/Content/assets/img/portfolio/");

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
                    photo.PhotoUrl = imageName;
                }

                #endregion

                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FilterID = new SelectList(db.Filters, "FilterID", "FilterName", photo.FilterID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", photo.OwnerAssetID);
            return View(photo);
        }

        // GET: Photos/Edit/5
        [Authorize(Roles = "Admin, Photographer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.FilterID = new SelectList(db.Filters, "FilterID", "FilterName", photo.FilterID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", photo.OwnerAssetID);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Photographer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotoID,Title,PhotoUrl,FilterID,OwnerAssetID")] Photo photo, HttpPostedFileBase myImg)
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
                        string savePath = Server.MapPath("~/Content/assets/img/portfolio/");
                        if (photo.PhotoUrl != null && photo.PhotoUrl != "noImage.png")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/asset/img/portfolio/S" + Session["currentImage"].ToString()));
                        }
                        photo.PhotoUrl = imageName;
                        Image convertedImage = Image.FromStream(myImg.InputStream);
                        int maxImgSize = 1200;
                        int maxThumbSize = 100;
                        //calling this method will use the variables created above will save the full size image and thumbnail img to your server. 
                        UploadUtility.ResizeImage(savePath, imageName, convertedImage, maxImgSize, maxThumbSize);

                        ViewBag.PhotoMessage = "Photo Upload Complete.";
                    }

                }

                #endregion


                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FilterID = new SelectList(db.Filters, "FilterID", "FilterName", photo.FilterID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetCallName", photo.OwnerAssetID);
            return View(photo);
        }

        // GET: Photos/Delete/5
        [Authorize(Roles = "Admin, Photographer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [Authorize(Roles = "Admin, Photographer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo.PhotoUrl != null && photo.PhotoUrl != "noImage.png")
            {
                System.IO.File.Delete(Server.MapPath("~/Content/asset/img/portfolio/" + Session["currentImage"].ToString()));
            }
            db.Photos.Remove(photo);
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
