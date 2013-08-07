using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HURazor.Models
{
    public class Follow
    {
        public int FollowID { get; set; }
        public int UserId { get; set; }
        public int ProductID { get; set; }
        public virtual UserProfile UserProfile { get; set; } 
        public virtual Product Product { get; set; }
    }
}