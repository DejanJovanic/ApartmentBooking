using ProjekatWeb.Models;
using ProjekatWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace ProjekatWeb.Controllers
{
    [RoutePrefix("Apartment")]
    public class ApartmentController : ApiController
    {
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles ="Host")]
        public async Task<HttpResponseMessage> Post()
        {
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var apartment = MakeApartment(provider);
                Host host = null;
                using (var repo = new DataRepo())
                {
                    host = repo.GetHostWithName(User.Identity.Name);
                }
                if (host != null)
                {
                    if (host.Blocked)
                        return Request.CreateResponse(HttpStatusCode.Forbidden, "User is blocked");
                        
                
                apartment.Images = new List<Image>();
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    string fileName = Path.Combine(HostingEnvironment.MapPath("~/Images"), Guid.NewGuid().ToString() + "." + file.Headers.ContentType.MediaType.Split('/')[1]);
                    var fileInfo = new FileInfo(fileName);
                    if (!Directory.Exists(fileInfo.Directory.FullName))
                    {
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    }


                    File.Move(file.LocalFileName, fileName);

                    apartment.Images.Add(new Image() { Path = fileName });

                }
                using(var repo = new DataRepo())
                {
                        repo.AddApartment(User.Identity.Name, apartment);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unknown host suplemented data."));
                }
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("Search")]
        public List<ApartmentDisplay> Search(SearchParameters search)
        {
           
            if (User.IsInRole("Admin"))
            {
                using (var repo = new DataRepo())
                {
                    var temp = repo.GetDisplaysWithDatesAdmin();
                    return Filter(temp, search);

                }
            }
            else if (User.IsInRole("Host"))
            {
                using (var repo = new DataRepo())
                {
                    var host = repo.GetHostWithName(User.Identity.Name);
                    if (host != null && !host.Blocked)
                    {
                        var temp = repo.GetDisplaysWithDatesHost(User.Identity.Name);
                        var ret = Filter(temp.Where(i => i.Status == "Active"), search);
                        foreach (var a in temp.Where(i => i.Status == "Inactive").ToList())
                        {
                            ret.Add(new ApartmentDisplay() { Address = a.Address, RoomNumber = a.RoomNumber, GuestNumber = a.GuestNumber, ID = a.ID, Price = a.Price, Type = a.Type, Status = a.Status });

                        }
                        return ret;

                    }
                    else return null;

                }
            }
            else
            {
                
                using (var repo = new DataRepo())
                {

                   var temp = repo.GetDisplaysWithDatesGuest();
                    var ret = Filter(temp.Where(i => i.Status == "Active"), search);
                    foreach (var a in temp.Where(i => i.Status == "Inactive").ToList())
                    {
                        ret.Add(new ApartmentDisplay() { Address = a.Address, RoomNumber = a.RoomNumber, GuestNumber = a.GuestNumber, ID = a.ID, Price = a.Price, Type = a.Type, Status = a.Status });

                    }
                    return ret;

                }
            }
           
            
        }

        private List<ApartmentDisplay> Filter(IEnumerable<DisplaysWithDates> temp, SearchParameters search)
        {
            List<ApartmentDisplay> ret = new List<ApartmentDisplay>();
            foreach (var a in temp)
            {
                bool isTimeOk = true;
                if (search.CheckInDate != null && search.CheckOutDate == null)
                {
                    if (!a.AvailableDates.Contains(search.CheckInDate.Value.ToUniversalTime()))
                        continue;
                }
                else if (search.CheckInDate == null && search.CheckOutDate != null)
                {
                    if (!a.AvailableDates.Contains(search.CheckOutDate.Value.ToUniversalTime()))
                        continue;
                }
                else if (search.CheckInDate != null && search.CheckOutDate != null)
                {
                   
                    var maxDate = search.CheckOutDate.Value.ToUniversalTime();
                    for (DateTime date = new DateTime(search.CheckInDate.Value.ToUniversalTime().Ticks); date <= maxDate;)
                    {
                        if (!a.AvailableDates.Contains(date))
                        {
                            isTimeOk = false;
                            break;
                        }

                        date = date.AddDays(1);
                    }
                }
                if (isTimeOk)
                    {
                        var address = a.Address.Split(',');
                        if (search.Town != null && search.Town != "")
                            if (address[1] == null || address[1] != search.Town)
                                continue;
                        if (search.State != null && search.State != "")
                            if (address[3] == null || address[3] != search.State)
                            continue;

                        if (search.GuestNo != null)
                            if (a.GuestNumber != search.GuestNo)
                            continue;
                        if (search.MinPrice != null)
                        {
                            if (a.Price < search.MinPrice)
                            continue;
                        }
                        if (search.MaxPrice != null)
                        {
                            if (a.Price > search.MaxPrice)
                            continue;
                        }
                        if (search.RoomMin != null)
                        {
                            if (a.RoomNumber < search.RoomMin)
                            continue;
                        }
                        if (search.RoomMax != null)
                        {
                            if (a.RoomNumber > search.RoomMax)
                            continue;
                        }

                    }
                    else
                    {
                        continue;
                    }
                    var display = new ApartmentDisplay() { Address = a.Address, RoomNumber = a.RoomNumber, GuestNumber = a.GuestNumber, ID = a.ID, Price = a.Price, Type = a.Type, Status = a.Status };
                    display.Amenities = new List<string>();
                    foreach (var b in a.Amenities)
                        display.Amenities.Add(b);
                    ret.Add(display);
                
            }
            return ret;
        }

        private Apartment MakeApartment(MultipartFormDataStreamProvider item)
        {
            var apartment = new Apartment();
            apartment.Type = Uri.UnescapeDataString(item.FormData.GetValues("Type")[0]);
            if (apartment.Type == "Room")
                apartment.RoomNumber = 1;
            else
                apartment.RoomNumber = int.Parse(Uri.UnescapeDataString(item.FormData.GetValues("RoomNumber")[0]));
            apartment.GuestNumber = int.Parse(Uri.UnescapeDataString(item.FormData.GetValues("GuestNumber")[0]));
            apartment.Price = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Price")[0]));
            var location = new Location();
            location.Address = Uri.UnescapeDataString(item.FormData.GetValues("Address")[0]);
            if (location.Address.Split(',').Length != 4)
                throw new Exception();
            location.Longitude = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Longitude")[0]));
            location.Latitude = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Latitude")[0]));
            apartment.Location = location;
            apartment.Status = "Inactive";
            apartment.CheckInTime = item.FormData.GetValues("CheckInTime")[0];
            apartment.CheckOutTime = item.FormData.GetValues("CheckOutTime")[0];
            if (!Regex.IsMatch(apartment.CheckInTime, "^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"))
                throw new Exception();
            if (!Regex.IsMatch(apartment.CheckOutTime, "^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"))
                throw new Exception();
            apartment.RentDates = new PersistableDateTimeCollection();
            apartment.Amenities = new List<Amenity>();
            if(item.FormData.GetValues("RentDates") != null)
            {
                foreach (var a in item.FormData.GetValues("RentDates"))
                {
                    apartment.RentDates.Add(DateTime.Parse(Uri.UnescapeDataString(a)).ToUniversalTime());
                }
                apartment.AvailableDates = new PersistableDateTimeCollection();
                foreach (var a in apartment.RentDates)
                {
                    apartment.AvailableDates.Add(a);
                }
            }
            
            if(item.FormData.GetValues("Amenities") != null)
            {
                List<string> amenities;
                using (var repo = new DataRepo())
                {
                    amenities = repo.GetAmenities();
                }
                foreach (var a in item.FormData.GetValues("Amenities"))
                {
                    var b = Uri.UnescapeDataString(a);
                    var c = amenities.Where(i => i == b).SingleOrDefault();
                    if (c != null)
                        apartment.Amenities.Add(new Amenity() { Name = c});
                }
            }
           
            return apartment;
        }

        private ApartmentModify MakeApartmentModify(MultipartFormDataStreamProvider item)
        {
            var apartment = new ApartmentModify();
            apartment.Type = Uri.UnescapeDataString(item.FormData.GetValues("Type")[0]);
            apartment.RoomNumber = int.Parse(Uri.UnescapeDataString(item.FormData.GetValues("RoomNumber")[0]));
            if (apartment.Type == "Room")
                apartment.RoomNumber = 1;
            else
                apartment.RoomNumber = int.Parse(Uri.UnescapeDataString(item.FormData.GetValues("RoomNumber")[0]));
            apartment.ID = int.Parse(Uri.UnescapeDataString(item.FormData.GetValues("ID")[0]));
            apartment.Price = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Price")[0]));
            var location = new Location();
            location.Address = Uri.UnescapeDataString(item.FormData.GetValues("Address")[0]);
            if (location.Address.Split(',').Length != 4)
                throw new Exception();
            location.Longitude = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Longitude")[0]));
            location.Latitude = double.Parse(Uri.UnescapeDataString(item.FormData.GetValues("Latitude")[0]));
            apartment.Location = location;
            apartment.DeleteIds = new List<int>();
            if(item.FormData.GetValues("DeletedPictures") != null)
            {
                foreach (var a in item.FormData.GetValues("DeletedPictures"))
                {
                    apartment.DeleteIds.Add(int.Parse(a));
                }
            }
           
            apartment.CheckInTime = (item.FormData.GetValues("CheckInTime")[0]);
            apartment.CheckOutTime = item.FormData.GetValues("CheckOutTime")[0];
            if (!Regex.IsMatch(apartment.CheckInTime, "^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"))
                throw new Exception();
            if (!Regex.IsMatch(apartment.CheckOutTime, "^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"))
                throw new Exception();
            apartment.RentDates = new PersistableDateTimeCollection();
            apartment.Amenities = new List<Amenity>();
            if (item.FormData.GetValues("RentDates") != null)
            {
                foreach (var a in item.FormData.GetValues("RentDates"))
                {
                    var time = Uri.UnescapeDataString(a);
                    apartment.RentDates.Add(DateTime.Parse(time).ToUniversalTime());
                }
                apartment.AvailableDates = new PersistableDateTimeCollection();
                
            }

            if (item.FormData.GetValues("Amenities") != null)
            {
                List<string> amenities;
                using (var repo = new DataRepo())
                {
                    amenities = repo.GetAmenities();
                }
                foreach (var a in item.FormData.GetValues("Amenities"))
                {
                    var b = Uri.UnescapeDataString(a);
                    var c = amenities.Where(i => i == b).SingleOrDefault();
                    if (c != null)
                     apartment.Amenities.Add(new Amenity() { Name = c });
                }
            }

            return apartment;
        }

        [HttpGet]
        [Route("GetDisplays")]
        public ICollection<ApartmentDisplay> GetDisplays()
        {
            using(var repo = new DataRepo())
            {
                if (User.IsInRole("Admin"))
                {
                    return repo.GetDisplaysAdmin();
                }
                else if (User.IsInRole("Host"))
                {
                    return repo.GetDisplaysHost(User.Identity.Name);
                }
                else
                {
                    return repo.GetDisplaysGuest();
                }
                
                
            }
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("ApproveApartment")]
        public void ApproveApartment(int id)
        {
            using(var repo = new DataRepo())
            {
                repo.ApproveApartment(id);
            }
        }

        [HttpGet]
        [Route("DeleteApartment")]
        [Authorize(Roles ="Host,Admin")]
        public IHttpActionResult DeleteApartment(int id)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    using (var repo = new DataRepo())
                    {

                        repo.DeleteApartment(id, User.Identity.Name);
                        return Content(HttpStatusCode.OK, "Sucessfully deleted");
                    }
                }
                else
                {
                    using (var repo = new DataRepo())
                    {
                        var host = repo.GetHostWithName(User.Identity.Name);
                        if (!host.Blocked)
                        {
                            repo.DeleteApartment(id, User.Identity.Name);
                            return Content(HttpStatusCode.OK, "Sucessfully deleted");
                        }
                        else
                        {
                            return Content(HttpStatusCode.Forbidden, "User is blocked");
                        }
                          
                    }

                }
                
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError,"Something went wrong");
            }
            
        }

        [HttpGet]
        [Route("GetApartment")]
        public ApartmentPreview GetApartment(int id)
        {
            Apartment apartment = null;
            ApartmentPreview ret = null;
            using(var repo = new DataRepo())
            {
                apartment = repo.GetApartment(id);
            
            if(apartment != null && !apartment.Deleted)
            {
                ret = new ApartmentPreview();
                ret.ID = apartment.ID;
                ret.CheckInTime = apartment.CheckInTime;
                ret.CheckOutTime = apartment.CheckOutTime;
                foreach (var a in apartment.Comments.Where(i => i.Deleted == false))
                    ret.Comments.Add(new CommentPreview() { ID = a.ID, Text = a.Text, Rate=a.Rate, GuestUsername= a.Guest.Username});
                foreach (var a in apartment.Amenities.Where(i => i.Deleted == false))
                    ret.Amenities.Add(a.Name);
                foreach (var a in apartment.AvailableDates)
                    ret.AvailableDates.Add(a.ToUniversalTime());
                foreach (var a in apartment.Images.Where(i => i.Deleted == false))
                    ret.Images.Add(new Image() { ID = a.ID, Path = "../Images/" + a.Path.Split('\\').Last() });

                ret.GuestNumber = apartment.GuestNumber;
                ret.RoomNumber = apartment.RoomNumber;
                ret.Location = apartment.Location;
                ret.Price = apartment.Price;
                ret.Type = apartment.Type;
            }
            }
            return ret;
        }
        [HttpGet]
        [Route("GetApartmentModify")]
        public ApartmentPreview GetApartmentModify(int id)
        {
            Apartment apartment = null;
            ApartmentPreview ret = null;
            using (var repo = new DataRepo())
            {
                apartment = repo.GetApartment(id);
            
            if (apartment != null)
            {
                ret = new ApartmentPreview();
                ret.ID = apartment.ID;
                ret.CheckInTime = apartment.CheckInTime;
                ret.CheckOutTime = apartment.CheckOutTime;
                foreach (var a in apartment.Comments)
                    ret.Comments.Add(new CommentPreview() { ID = a.ID, GuestUsername = a.Guest.Username, Rate = a.Rate, Text= a.Text});
                foreach (var a in apartment.Amenities)
                    ret.Amenities.Add(a.Name);
                foreach (var a in apartment.RentDates)
                    ret.AvailableDates.Add(a.ToUniversalTime());
                foreach (var a in apartment.Images)
                    ret.Images.Add(new Image() { ID = a.ID, Path = "../Images/" + a.Path.Split('\\').Last() });

                ret.GuestNumber = apartment.GuestNumber;
                ret.RoomNumber = apartment.RoomNumber;
                ret.Location = apartment.Location;
                ret.Price = apartment.Price;
                ret.Type = apartment.Type;
            }
            }
            return ret;
        }   

        [HttpPost]
        [Route("Modify")]
        public async Task<HttpResponseMessage> ModifyApartment()
        {

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var apartment = MakeApartmentModify(provider);
                Host host = null;
                using (var repo = new DataRepo())
                {
                    host = repo.GetHostWithName(User.Identity.Name);
                }
                if (host != null)
                {
                    if (host.Blocked)
                        return new HttpResponseMessage(HttpStatusCode.Forbidden);

                    apartment.Images = new List<Image>();
                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileName = Path.Combine(HostingEnvironment.MapPath("~/Images"), Guid.NewGuid().ToString() + "." + file.Headers.ContentType.MediaType.Split('/')[1]);
                        var fileInfo = new FileInfo(fileName);
                        if (!Directory.Exists(fileInfo.Directory.FullName))
                        {
                            Directory.CreateDirectory(fileInfo.Directory.FullName);
                        }


                        File.Move(file.LocalFileName, fileName);

                        apartment.Images.Add(new Image() { Path = fileName });

                    }
                    using (var repo = new DataRepo())
                    {
                        repo.ModifyApartment(User.Identity.Name, apartment);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unknown host suplemented data."));
                }
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("GetAmenities")]
        public List<string> GetAmenities()
        {
            using(var repo = new DataRepo())
            {
                return repo.GetAmenities();
            }
        }

     

        [HttpGet]
        [Route("GetComments")]
        [Authorize(Roles ="Admin,Host")]
        public IHttpActionResult GetComments(int id)
        {
            try
            {
                using (var repo = new DataRepo())
                {
                    if (User.IsInRole("Admin"))
                    {
                        return Content(HttpStatusCode.OK, repo.GetComments(id));
                    }
                    else
                    {
                        var host = repo.GetHostWithName(User.Identity.Name);
                        if(host != null)
                        {
                            if (host.Blocked)
                                return Content(HttpStatusCode.Forbidden, "User is blocked");
                            var apartment = host.MyApartments.Where(i => i.ID == id && i.Deleted == false).SingleOrDefault();
                            if(apartment != null)
                            {
                               return Content(HttpStatusCode.OK, repo.GetComments(id));
                            }
                            else
                            {
                                return Content(HttpStatusCode.BadGateway, "Host is not owner of apartment");
                            }
                        }
                        else
                        {
                            return Content(HttpStatusCode.BadRequest, "User does not exist");
                        }
                    }
                    
                }
            }
            catch
            {
                return Content(HttpStatusCode.InternalServerError, "Something went wrong...");
            }
           
        }

        [HttpGet]
        [Route("DeleteComment")]
        [Authorize(Roles = "Admin,Host")]
        public HttpResponseMessage DeleteComment(int id)
        {
            try
            {
                using (var repo = new DataRepo())
                {
                    if (User.IsInRole("Admin"))
                    {
                        repo.DeleteComment(id);
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        var host = repo.GetHostWithName(User.Identity.Name);
                        if (host != null)
                        {
                            if (host.Blocked)
                                return new HttpResponseMessage(HttpStatusCode.Forbidden);
                            repo.DeleteComment(id);
                            return new HttpResponseMessage(HttpStatusCode.OK);

                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }
                    }

                }
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("GetFullAmenities")]
        public ICollection<Amenity> GetFullAmenities()
        {
            using(var repo = new DataRepo())
            {
                return repo.GetFullAmenities();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("AddAmenity")]
        public HttpResponseMessage AddAmenity(string name)
        {
            using (var repo = new DataRepo())
            {
                if (repo.AddAmenity(name))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("DeleteAmenity")]
        public HttpResponseMessage DeleteAmenity(int id)
        {
            using (var repo = new DataRepo())
            {
                if (repo.DeleteAmenity(id))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
