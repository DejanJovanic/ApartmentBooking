using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models.Database
{
    public class Context : DbContext
    {
        public Context() : base() { }
        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Host> Hosts { get; set; }
        public virtual DbSet<Amenity> Amenities { get; set; }
    }
}