using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HURazor.Models
{
    public class Price
    {
        public int PriceID { get; set; }
        public int PriceOne { get; set; }
        public int ProductID { get; set; } 
        /*
        public decimal PriceTwo { get; set; }
        public decimal PriceThree { get; set; }
        public decimal PriceFour { get; set; }
        public decimal PriceFive { get; set; }
        public decimal PriceSix { get; set; }
        public string ProductID { get; set; }
         * */
        public virtual Product Product { get; set; }
    }
}