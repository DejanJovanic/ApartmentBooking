using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Guest : User
    {
        public  virtual ICollection<Apartment> RentedApartments { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public bool Blocked { get; set; }
        public Guest()
        {
            //RentedApartments = new List<Apartment>();
            //Reservations = new List<Reservation>();
            Role = "Guest";
            Blocked = false;
        }
    }
}