using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class SearchParameters
    {
        public  DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? RoomMin { get; set; }
        public int? RoomMax { get; set; }
        public int? GuestNo { get; set; }
    }
}