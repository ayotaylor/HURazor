using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HURazor.Models;

namespace HURazor.DAL
{
    public class AmazonInitializer : DropCreateDatabaseIfModelChanges<AmazonContext>
    {
        protected override void Seed(AmazonContext context)
        {
            var products = new List<Product>
            {
                new Product { ProductName = "Facing the Intelligence Explosion", Price = 99, 
                    ImageUrl = "http://ecx.images-amazon.com/images/I/51pciWIhIyL._BO2,204,203,200_PIsitb-sticker-arrow-click,TopRight,35,-76_AA278_PIkin4,BottomRight,-61,22_AA300_SH20_OU01_.jpg", 
                    DetailsUrl = "http://www.amazon.com/gp/product/B00C7YOR5Q/ref=s9_simh_gw_p351_d2_i1?pf_rd_m=ATVPDKIKX0DER&pf_rd_s=center-3&pf_rd_r=0QCEP61XQK3D14YCP6F7&pf_rd_t=101&pf_rd_p=470938811&pf_rd_i=507846" },
                 
                    new Product { ProductName = "PlayStation 3 Dualshock 3 Wireless Controller (Black)", Price = 4265, 
                    ImageUrl = "http://ecx.images-amazon.com/images/I/41Nw9erTKSL._SX38_SY50_CR,0,0,38,50_.jpg", 
                    DetailsUrl = "http://www.amazon.com/gp/product/B0015AARJI/ref=s9_psimh_gw_p63_d1_i1?pf_rd_m=ATVPDKIKX0DER&pf_rd_s=center-2&pf_rd_r=1CSRD8VW2NM2NFCF8T37&pf_rd_t=101&pf_rd_p=1389517282&pf_rd_i=507846" }
            };
            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
            /*
            var prices = new List<Price>
            {
                new Price { PriceOne = 99, ProductID = 1, },
                new Price { PriceOne = 4265, ProductID = 2 }
               
            };
            prices.ForEach(s => context.Prices.Add(s));
            context.SaveChanges();
            */
        }
    }
}