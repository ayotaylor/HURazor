using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;

namespace HURazor.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        [Display(Name = "")]
        public string ImageUrl { get; set; }

        [Display(Name = "Details")]
        public string DetailsUrl { get; set; }

        [Display(Name = "ASIN")]
        public string ASIN { get; set; }

        public string Link
        {
            get
            {
                return "http://localhost:65264?q=" + ASIN;
            }
        }

        public bool isFollowed { get; set; }

        public Product[] RelatedItems { get; set; }

        public virtual ICollection<Follow> Follows { get; set; } 
        public virtual ICollection<Price> Prices { get; set; }
    }
}