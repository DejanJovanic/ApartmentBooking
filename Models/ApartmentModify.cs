using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class ApartmentModify
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public bool Deleted { get; set; }
        public int RoomNumber { get; set; }
        public int GuestNumber { get; set; }
        public Location Location { get; set; }
        public virtual PersistableDateTimeCollection RentDates { get; set; }
        public virtual PersistableDateTimeCollection AvailableDates { get; set; }
     
        public virtual ICollection<Image> Images { get; set; }

        public double Price { get; set; }

        public virtual ICollection<Amenity> Amenities { get; set; }

        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }

        public ICollection<int> DeleteIds { get; set; }
    }
}