using ProjekatWeb.Models;
using ProjekatWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Xml.Linq;

namespace ProjekatWeb.Controllers
{
    [RoutePrefix("Reservation")]
    public class ReservationController : ApiController
    {
        [Route("GetPrice")]
        [HttpPost]
        public double GetPrice(ReservationPricing userData)
        {
            return CalculatePrice(userData.ID, userData.Times);
        }

        [Route("SetReservation")]
        [HttpPost]
        [Authorize(Roles ="Guest")]
        public HttpResponseMessage SetReservation(ReservationPricing data)
        {
            try
            {

                var price = CalculatePrice(data.ID, data.Times);
                if (price <= 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
               
                var reservation = new Reservation();
                reservation.StartDate = data.Times.Min().ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture);
                reservation.DaysNumber = data.Times.Count;
                reservation.Price = price;
                reservation.State = "Created";
                var times = new List<DateTime>();
                foreach(var a in data.Times)
                {
                    times.Add(a.ToUniversalTime());
                }

                using(var repo = new DataRepo())
                {
                    var guest = repo.GetGuestWithName(User.Identity.Name);
                    if (guest.Blocked)
                        return new HttpResponseMessage(HttpStatusCode.Forbidden);
                    repo.AddReservation(User.Identity.Name, reservation, times,data.ID);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        private double CalculatePrice(int ID,ICollection<DateTime> times)
        {
            double total = 0;
            bool isOk = true;
            if (times != null && times.Count > 0)
            {
                int id;
                List<DateTime> available = new List<DateTime>();
                var holidays = GetHolidays();
                double price = 0;
                
                List<DateTime> timesUTC = new List<DateTime>();
                foreach (var a in times)
                {
                    timesUTC.Add(a.ToUniversalTime());
                }
                
                using (var repo = new DataRepo())
                {
                    var item = repo.GetApartment(ID);
                    if (item != null)
                    {
                        id = item.ID;
                        foreach (var a in item.AvailableDates)
                        {
                            available.Add(a.ToUniversalTime());
                        }
                        price = item.Price;
                    }
                    else isOk = false;
                }

                if (isOk)
                {
                    var minDate = timesUTC.Min();
                    var maxDate = timesUTC.Max();

                    for (DateTime date = new DateTime(minDate.Ticks); date <= maxDate;)
                    {
                        if (!available.Contains(date))
                        {
                            isOk = false;
                            break;
                        }
                        else
                        {
                            var date2 = date.AddHours(2);
                            var holiday = holidays.Where(i => i.Day == date2.Day && i.Month == date2.Month).SingleOrDefault();
                            if (holiday != null)
                            {
                                total += price * 1.05;
                            }
                            else if (date2.DayOfWeek == DayOfWeek.Saturday || date2.DayOfWeek == DayOfWeek.Sunday || date2.DayOfWeek == DayOfWeek.Friday)
                            {

                                total += price * 0.9;
                            } else total += price;
                        }
                        date = date.AddDays(1);
                    }
                   
                }

            }
            else
            {
                isOk = false;
            }
           

            return (isOk) ? total : -1;
        }

        public static ICollection<Holiday> GetHolidays()
      
        {

            List<Holiday> ret = null;
            try
            {
                var holidays = XElement.Load(HostingEnvironment.MapPath("~/App_Data/Holidays.txt"));
                ret = new List<Holiday>();
                foreach (var a in holidays.Elements("Holiday"))
                {
                    ret.Add(new Holiday() { Day = int.Parse(a.Element("Day").Value), Month = int.Parse(a.Element("Month").Value) });
                }
                return ret;
            }
            catch
            {
                ret = new List<Holiday>();
                return ret;
            }
        }
        [HttpGet]
        [Route("GetReservationsDisplays")]
        [Authorize(Roles = "Admin,Guest,Host")]
        public IHttpActionResult GetReservationsDisplay()
        {
            try { 
            if (User.IsInRole("Admin"))
            {
                using (var repo = new DataRepo())
                {
                    var user = repo.GetAdminWithName(User.Identity.Name);
                    if (user != null)
                        return Content(HttpStatusCode.OK, repo.GetReservationDisplaysAdmin());
                }
            }
            else if (User.IsInRole("Guest"))
            {
                using (var repo = new DataRepo())
                {
                    var user = repo.GetGuestWithName(User.Identity.Name);
                    if(user != null)
                        return Content(HttpStatusCode.OK, repo.GetReservationDisplaysGuest(User.Identity.Name));
                }
            }
            else
            {
                using (var repo = new DataRepo())
                {
                    var user = repo.GetHostWithName(User.Identity.Name);
                    if (user != null)
                        return Content(HttpStatusCode.OK, repo.GetReservationDisplaysHost(User.Identity.Name)); 
                }
            }
            return Content(HttpStatusCode.BadRequest, "Bad input parameters");
        }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
    }
}

        [HttpGet]
        [Route("AcceptReservation")]
        [Authorize(Roles ="Host")]
        public IHttpActionResult AcceptReservation(int id)
        {
            try { 
            using(var repo = new DataRepo())
            {
                    var guest = repo.GetHostWithName(User.Identity.Name);
                    if (guest.Blocked)
                        return Content(HttpStatusCode.Forbidden,"User is blocked");
                    var ret = repo.AcceptReservation(id, User.Identity.Name);
                if (ret)
                {
                    return Content(HttpStatusCode.OK, "All good");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Invalid info provided");
                }
            }
        }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
    }
}

        [HttpGet]
        [Route("DeclineReservation")]
        [Authorize(Roles = "Host")]
        public IHttpActionResult DeclineReservation(int id)
        {
            try { 
            using (var repo = new DataRepo())
            {
                    var guest = repo.GetHostWithName(User.Identity.Name);
                    if (guest.Blocked)
                        return Content(HttpStatusCode.Forbidden, "User is blocked");
                    var ret = repo.DeclineReservation(id, User.Identity.Name);
                if (ret)
                {
                    return Content(HttpStatusCode.OK, "All good");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Invalid info provided");
                }
            }
        }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
    }
}
        [HttpGet]
        [Route("FinishReservation")]
        [Authorize(Roles = "Host")]
        public IHttpActionResult FinishReservation(int id)
        {
            try { 
            using (var repo = new DataRepo())
            {
                    var guest = repo.GetHostWithName(User.Identity.Name);
                    if (guest.Blocked)
                        return Content(HttpStatusCode.Forbidden, "User is blocked");
                    var ret = repo.FinishReservation(id, User.Identity.Name);
                if (ret)
                {
                    return Content(HttpStatusCode.OK, "All good");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Invalid info provided");
                }
            }
        }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
    }
}

        [HttpGet]
        [Route("CancelReservation")]
        [Authorize(Roles = "Guest")]
        public IHttpActionResult CancelReservation(int id)
        {
            try { 
            using (var repo = new DataRepo())
            {
                    var guest = repo.GetGuestWithName(User.Identity.Name);
                    if (guest.Blocked)
                        return Content(HttpStatusCode.Forbidden, "User is blocked");
                    var ret = repo.CancelReservation(id, User.Identity.Name);
                if (ret)
                {
                    return Content(HttpStatusCode.OK, "All good");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Invalid info provided");
                }
            }
        }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
    }
}

        [HttpPost]
        [Route("AddComment")]
        [Authorize(Roles = "Guest")]
        public IHttpActionResult SetComment(CommentBetween comment)
        {
            try
            {
                using (var repo = new DataRepo())
                {
                    var guest = repo.GetGuestWithName(User.Identity.Name);
                    if(guest != null)
                    {
                        if (guest.Blocked)
                            return Content(HttpStatusCode.Forbidden, "User is blocked");

                        var ret = repo.AddComment(User.Identity.Name, comment.ID, comment.Rate, comment.Text);
                        if (ret)
                        {
                            return Content(HttpStatusCode.OK, "All good");
                        }
                        else
                        {
                            return Content(HttpStatusCode.BadRequest, "Invalid info provided");
                        }
                    }
                    else return Content(HttpStatusCode.BadRequest, "Invalid info provided");


                }
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
            }
            
        }

        [Route("Search")]
        [Authorize(Roles ="Host,Admin")]
        public ICollection<ReservationDisplay> Search(UserPreview search)
        {
            if (User.IsInRole("Admin"))
            {
                using (var repo = new DataRepo())
                {
                    var temp = repo.GetReservationDisplaysAdmin();
                    return Filter(temp, search);

                }
            }
            else if (User.IsInRole("Host"))
            {
                using (var repo = new DataRepo())
                {
                    var temp = repo.GetReservationDisplaysHost(User.Identity.Name);
                    return Filter(temp, search);

                }
            }
            return null;
           
        }

        private ICollection<ReservationDisplay> Filter(IEnumerable<ReservationDisplay> displays, UserPreview search)
        {
            List<ReservationDisplay> ret = new List<ReservationDisplay>();
            foreach(var a in displays)
            {
                Guest guest;
                using(var repo = new DataRepo())
                {
                    guest = repo.GetGuestWithName(a.GuestUsername);
                }

                if(search.Name != "" && search.Name != null)
                {
                    if (guest.Name != search.Name)
                        continue;
                }
                  if (search.LastName != "" && search.LastName != null)
                {
                    if (guest.LastName != search.LastName)
                        continue;
                }
                if (search.Username != "" && search.Username != null)
                {
                    if (guest.Username != search.Username)
                        continue;
                }
                if (search.Gender != "" && search.Gender != null && search.Gender != "All")
                {
                    if (guest.Gender != search.Gender)
                        continue;
                }

                ret.Add(a);
            }
            return ret;
        }

    }
}
