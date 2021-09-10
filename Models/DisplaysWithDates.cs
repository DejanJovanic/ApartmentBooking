using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class DisplaysWithDates
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public int GuestNumber { get; set; }
        public string Type { get; set; }
        public int RoomNumber { get; set; }

        public string Address { get; set; }

        public ICollection<string> Amenities { get; set; }
        public string Status { get; set; }
        public PersistableDateTimeCollection AvailableDates { get; set; }
    }
}