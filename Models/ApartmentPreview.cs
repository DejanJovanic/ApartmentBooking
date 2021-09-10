using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class ApartmentPreview
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int RoomNumber { get; set; }
        public int GuestNumber { get; set; }
        public Location Location { get; set; }
        
        public  List<DateTime> AvailableDates { get; set; }
        public Host Host { get; set; }
        public List<Image> Images { get; set; }

        public List<CommentPreview> Comments { get; set; }
        public double Price { get; set; }

        public virtual List<string> Amenities { get; set; }

        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
   

        public ApartmentPreview()
        {
            AvailableDates = new List<DateTime>();
            Images = new List<Image>();
            Comments = new List<CommentPreview>();
            Amenities = new List<string>();
        }
    }
}