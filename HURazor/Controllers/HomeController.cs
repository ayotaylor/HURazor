using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HURazor.ServiceReference1;
using System.Configuration;

namespace HURazor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string accessKeyId = "AKIAJ4WDNF2MJZUAQDJQ";
            string secretKey = "razor0df-20";
            // Instantiate Amazon ProductAdvertisingAPI client
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();
            //amazonClient.ChannelFactory.Endpoint.Behaviors.Add(new HURazor.AmazonSigningEndpointBehavior
            //    (accessKeyId, secretKey));
            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = "Books";
            request.Title = "WCF";
            request.ResponseGroup = new string[] { "Medium" };

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = accessKeyId;  //ConfigurationManager.AppSettings["accessKeyId"];
            itemSearch.AssociateTag = secretKey;
            

            // send the ItemSearch request
            ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

            ViewBag.Message = "Title - " + response.Items[0].Item[0].ItemAttributes.Title
                + " and Price - " + response.Items[0].Item[0].ItemAttributes.ListPrice.Amount + '\n'
                + response.Items[0].Item[0].ImageSets[0].ThumbnailImage.URL ;

            ViewBag.ImageURLs = response.Items[0].Item[0].ImageSets[0].MediumImage.URL;

            //response.Items[0].Item[0].RelatedItems[0].RelatedItem[0].Item.ASIN;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
