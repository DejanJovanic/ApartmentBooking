using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Host : User
    {
        public virtual ICollection<Apartment> MyApartments { get; set; }
        public bool Blocked { get; set; }
        public Host()
        {
            //MyApartments = new List<Apartment>();
            Role = "Host";
            Blocked = false;
        }
    }
}