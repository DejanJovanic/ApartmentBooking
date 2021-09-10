using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class ReservationPricing
    {
        public int ID { get; set; }
        public ICollection<DateTime> Times { get; set; }
    }
}