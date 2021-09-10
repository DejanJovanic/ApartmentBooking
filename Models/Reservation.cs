using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public virtual Apartment Apartment { get; set; }
        public string StartDate { get; set; }
        public int DaysNumber { get; set; }
        public double Price { get; set; }
        public virtual Guest Guest { get; set; }
        public string State { get; set; }
        public bool CommentSetted { get; set; }

    }

}