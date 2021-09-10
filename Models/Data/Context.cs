using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models.Data
{
    public class ApartmentContext : DbContext
    {
        public ApartmentContext() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjekatWeb.Models.Data.ApartmentContext, ProjekatWeb.Migrations.Configuration>());
        }
        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Host> Hosts { get; set; }
        public virtual DbSet<Amenity> Amenities { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Admin>().HasKey(i => i.UserName);
        //    modelBuilder.Entity<Host>().HasKey(i => i.UserName);

        //    modelBuilder.Entity<Host>().HasMany(i => i.MyApartments);
        //    modelBuilder.Entity<Guest>().HasKey(i => i.UserName);
        //    modelBuilder.Entity<Guest>().HasMany(i => i.RentedApartments);
        //    modelBuilder.Entity<Guest>().HasMany(i => i.Reservations);


        //}
    }
}