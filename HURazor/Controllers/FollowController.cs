using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HURazor.Models;
using HURazor.ServiceReference1;
using WebMatrix.WebData;

namespace HURazor.Controllers
{
    public class FollowController : Controller
    {
        private AmazonContext db = new AmazonContext();

        //
        // GET: /Follow/

        public ActionResult Index()
        {
            var follows = db.Follows.Include(f => f.UserProfile).Include(f => f.Product);
            return View(follows.ToList());
        }

        //
        // GET: /Follow/Details/5

        public ActionResult Details(int id = 0)
        {
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        //
        // GET: /Follow/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        //
        // POST: /Follow/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Follow follow)
        {
            if (ModelState.IsValid)
            {
                db.Follows.Add(follow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", follow.UserId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", follow.ProductID);
            return View(follow);
        }

        //
        // GET: /Follow/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", follow.UserId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", follow.ProductID);
            return View(follow);
        }

        //
        // POST: /Follow/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Follow follow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(follow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", follow.UserId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", follow.ProductID);
            return View(follow);
        }

        //
        // GET: /Follow/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Follow follow = db.Follows.Find(id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        //
        // POST: /Follow/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Follow follow = db.Follows.Find(id);
            db.Follows.Remove(follow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UnfollowItem()
        {
            List<Product> p = new List<Product>();
            //ViewBag.Search = SearchString;
            string asinquery = Request.QueryString["q"];
            string accessKeyId = "AKIAJ4WDNF2MJZUAQDJQ";
            string secretKey = "razor0df-20";
            // Instantiate Amazon ProductAdvertisingAPI client
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();
            // prepare an ItemSearch request
            ItemLookup lookup = new ItemLookup();
            ItemLookupRequest request = new ItemLookupRequest();

            lookup.AssociateTag = secretKey;
            lookup.AWSAccessKeyId = accessKeyId;

            request.IdType = ItemLookupRequestIdType.ASIN;
            request.ItemId = new string[] { asinquery };

            request.ResponseGroup = new string[] { "Large" };

            lookup.Request = new ItemLookupRequest[] { request };
            ItemLookupResponse response = amazonClient.ItemLookup(lookup);
            for (int i = 0; i < response.Items[0].Item.Length; i++)
            {
                if (response.Items[0].Item[i].ItemAttributes.Title != null &&
                    response.Items[0].Item[i].ItemAttributes.ListPrice != null &&
                    response.Items[0].Item[i].ImageSets[0].SmallImage.URL != null &&
                    response.Items[0].Item[i].DetailPageURL != null)
                {
                    p.Add(new Product()
                    {
                        ProductName = response.Items[0].Item[i].ItemAttributes.Title,
                        Price = Convert.ToDecimal(response.Items[0].Item[i].ItemAttributes.ListPrice.Amount) / 100,
                        ImageUrl = response.Items[0].Item[i].ImageSets[0].MediumImage.URL,
                        DetailsUrl = response.Items[0].Item[i].DetailPageURL,
                        ASIN = response.Items[0].Item[i].ASIN,
                        //RelatedItems = response.Items[0].Item[i].RelatedItems[0].RelatedItem//foreach(RelatedItem r in response.Items[0].Item[i].RelatedItems[0].RelatedItem)

                    });
                }
            }
            asinquery = null;

            string asin = p[0].ASIN;
            var item = db.Products.SingleOrDefault(i => i.ASIN == asin);    // find product
            var followedItem = db.Follows.SingleOrDefault(f => f.UserId == WebSecurity.CurrentUserId && f.ProductID == item.ProductID);
            db.Follows.Remove(followedItem);
            db.SaveChanges();
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}