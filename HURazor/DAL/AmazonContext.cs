using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HURazor.Models
{
    public class AmazonContext : DbContext
    {
        public AmazonContext()
            : base("AmazonContext")
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Follow> Follows { get; set; }
        //public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Product>().HasOptional(x => x.;
        }
    }
}