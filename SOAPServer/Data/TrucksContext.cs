using SOAPServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace SOAPServer.Data
{
    public class TrucksContext : DbContext
    {
        public TrucksContext() : base("name=CarsConnectionString")
        {
        }

        public DbSet<Models.Truck> Trucks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Truck>().HasKey(t => t.TruckID);

            
        }
    }
}