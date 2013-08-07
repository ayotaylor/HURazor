using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HURazor.Models;
using HURazor.ServiceReference1;
using System.Configuration;
using WebMatrix.WebData;
using System.Data.Entity.Validation;

namespace HURazor.Controllers
{
    public class ProductController : Controller
    {
        private AmazonContext db = new AmazonContext();

        //
        // GET: /Product/

        public ActionResult Index(string SearchString)
        {
            List<Product> p = new List<Product>();
            ViewBag.Search = SearchString;
            string asinquery = Request.QueryString["q"];
            if (String.IsNullOrEmpty(SearchString) && asinquery == null)
            {

            }
            else if (asinquery == null)      // no item has been selected
            {
                string accessKeyId = "AKIAJ4WDNF2MJZUAQDJQ";
                string secretKey = "razor0df-20";
                // Instantiate Amazon ProductAdvertisingAPI client
                AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();
                // prepare an ItemSearch request
                ItemSearchRequest request = new ItemSearchRequest();
                request.SearchIndex = "All";
                request.Keywords = SearchString;
                //request.Title = SearchString;
                request.ResponseGroup = new string[] { "Large" };

                ItemSearch itemSearch = new ItemSearch();
                itemSearch.Request = new ItemSearchRequest[] { request };
                itemSearch.AWSAccessKeyId = accessKeyId;  //ConfigurationManager.AppSettings["accessKeyId"];
                itemSearch.AssociateTag = secretKey;

                // send the ItemSearch request
                ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

                for (int i = 0; i < response.Items[0].Item.Length - 1; i++)
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
                            ASIN = response.Items[0].Item[i].ASIN
                        });
                    }
                }
                foreach (var item in p)
                {
                    var pro = db.Products.SingleOrDefault(x => x.ASIN == item.ASIN);
                    if (pro != null)
                    {
                        if (db.Follows.SingleOrDefault(i => i.UserId == WebSecurity.CurrentUserId && i.ProductID == pro.ProductID) == null)
                            item.isFollowed = false;
                        else
                            item.isFollowed = true;
                    }
                }
            }
            else if (asinquery != null)    // item has been selected
            {
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
                            ASIN = response.Items[0].Item[i].ASIN
                        });
                    }
                }
                foreach (var item in p)
                {
                    var pro = db.Products.SingleOrDefault(x => x.ASIN == item.ASIN);
                    if (pro != null)
                    {
                        if (db.Follows.SingleOrDefault(i => i.UserId == WebSecurity.CurrentUserId && i.ProductID == pro.ProductID) == null)
                            item.isFollowed = false;
                        else
                            item.isFollowed = true;
                    }
                }
                asinquery = null;
            }
            return View(p.ToList());
        }

        public ActionResult FollowItem()
        {
            #region makeamazonrequest
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
            foreach (var thing in p)
            {
                var pro = db.Products.SingleOrDefault(x => x.ASIN == thing.ASIN);
                if (pro != null)
                {
                    if (db.Follows.SingleOrDefault(i => i.UserId == WebSecurity.CurrentUserId && i.ProductID == pro.ProductID) == null)
                        thing.isFollowed = false;
                    else
                        thing.isFollowed = true;
                }
            }
            asinquery = null;
            #endregion

            string asin = p[0].ASIN;
            var item = db.Products.SingleOrDefault(i => i.ASIN == asin);    // find product
            if (item == null)   // if product doesnt exist
            {
                // create new product and follow
                var product = new Product { ProductName = p[0].ProductName, ASIN = p[0].ASIN, DetailsUrl = p[0].DetailsUrl,
                                            ImageUrl = p[0].ImageUrl, Price = p[0].Price};
                db.Products.Add(product);
                db.SaveChanges();
                Follow follow = new Follow();
                follow.Product = db.Products.Single(i => i.ASIN == asin);
                follow.UserProfile = db.UserProfiles.FirstOrDefault(u => u.UserId == WebSecurity.CurrentUserId);
                db.Follows.Add(follow);    // add product and user to follow table
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return View(product);
            } 
            else
            {
                // follow item if it exists already and the user is not following the item
                var followed = db.Follows.SingleOrDefault(i => i.UserId == WebSecurity.CurrentUserId && i.ProductID == item.ProductID);
                if (followed == null)
                {
                    Follow follow = new Follow();
                    follow.Product = db.Products.Single(i => i.ASIN == asin);
                    follow.UserProfile = db.UserProfiles.FirstOrDefault(u => u.UserId == WebSecurity.CurrentUserId);
                    db.Follows.Add(follow);
                    db.SaveChanges();
                    return View(item);
                }
                    // the item is already being followed
                else
                {
                    ViewBag.Message = "Item already followed";
                    return View();
                }
            }  
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}