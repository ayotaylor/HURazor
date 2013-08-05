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
            else if (asinquery == null)
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
            }
            else if (asinquery != null)
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
                asinquery = null;
            }
            return View(p.ToList());
        }

        public ActionResult FollowItem()
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

            if (db.Products.FirstOrDefault(i => i.ASIN == p[0].ASIN) == null)
            {
                var product = new Product { ProductName = p[0].ProductName, ASIN = p[0].ASIN, DetailsUrl = p[0].DetailsUrl,
                                            ImageUrl = p[0].ImageUrl, Price = p[0].Price};
                //var follow = new Follow { Product = product, UserProfile = follow. };
                Follow follow = new Follow();
                follow.Product = product;
                //follow.UserProfile = db.UserProfiles.Find(u => u.UserID = (int)Session["UserID"]);
                db.Products.Add(product);
                db.SaveChanges();

            }

            return View();
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