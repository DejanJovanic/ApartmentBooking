
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models.Data
{
    public class DataRepo : IDisposable
    {
        private ApartmentContext context = new ApartmentContext();

        public ICollection<DisplaysWithDates> GetDisplaysWithDatesAdmin()
        {
            var ret = new List<DisplaysWithDates>();
            foreach (var apartment in context.Apartments.Include("Location").Where(i => i.Deleted == false))
            {
                ret.Add(new DisplaysWithDates() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type, Address = apartment.Location.Address, AvailableDates = apartment.AvailableDates }) ;
            }

            return ret;
        }
        public ICollection<DisplaysWithDates> GetDisplaysWithDatesHost(string username)
        {
            var ret = new List<DisplaysWithDates>();
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
            if (host != null)
            {
                foreach (var apartment in host.MyApartments.Where(i => i.Deleted == false))
                {
                    ret.Add(new DisplaysWithDates() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type, Address = apartment.Location.Address, AvailableDates = apartment.AvailableDates });
                }
            }

            return ret;
        }
        public ICollection<DisplaysWithDates> GetDisplaysWithDatesGuest()
        {
            var ret = new List<DisplaysWithDates>();
            foreach (var apartment in context.Apartments.Include("Location").Where(i => i.Status == "Active" && i.Deleted == false))
            {
                ret.Add(new DisplaysWithDates() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type, Address = apartment.Location.Address, AvailableDates = apartment.AvailableDates });
            }

            return ret;
        }

        public ICollection<ApartmentDisplay> GetDisplaysGuest()
        {
            var ret = new List<ApartmentDisplay>();
            foreach (var apartment in context.Apartments.Include("Location").Where(i => i.Status == "Active" && i.Deleted == false))
            {
                ret.Add(new ApartmentDisplay() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type,Address = apartment.Location.Address,Status = apartment.Status});
            }

            return ret;
        }
        public ICollection<ApartmentDisplay> GetDisplaysAdmin()
        {
            var ret = new List<ApartmentDisplay>();
            foreach (var apartment in context.Apartments.Include("Location").Where(i => i.Deleted == false ))
            {
                ret.Add(new ApartmentDisplay() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type, Address = apartment.Location.Address, Status = apartment.Status });
            }

            return ret;
        }
        public ICollection<ApartmentDisplay> GetDisplaysHost(string username)
        {
            var ret = new List<ApartmentDisplay>();
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
            if(host != null)
            {
                foreach(var apartment in host.MyApartments.Where(i => i.Deleted == false))
                {
                    ret.Add(new ApartmentDisplay() { ID = apartment.ID, GuestNumber = apartment.GuestNumber, Price = apartment.Price, RoomNumber = apartment.RoomNumber, Type = apartment.Type, Address = apartment.Location.Address, Status = apartment.Status });
                }
            }
           

            return ret;
        }

        public Apartment GetApartment(int id)
        {
           return context.Apartments.Where(i => i.ID == id).SingleOrDefault();
        }

        public void ApproveApartment(int id)
        {
            var apartment = context.Apartments.Where(i => i.ID == id).SingleOrDefault();
            if (apartment != null)
            {
                apartment.Status = "Active";
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void AddReservation(string userName,Reservation reservation,ICollection<DateTime> dates,int apartmentID)
        {
            var guest = context.Guests.Where(i => i.Username == userName).SingleOrDefault();
            if(guest != null)
            {
                var apartment = context.Apartments.Where(i => i.ID == apartmentID).SingleOrDefault();
                if(apartment != null)
                {
                    reservation.Guest = guest;
                    reservation.Apartment = apartment;
                    
                    guest.RentedApartments.Add(apartment);
                    apartment.Reservations.Add(reservation);
                    foreach(var a in dates)
                    {
                        apartment.AvailableDates.Remove(a);
                    }

                    context.SaveChanges();
                    
                }
                else
                {
                    throw new ArgumentException("Apartment not found");
                }
            }
            else
            {
                throw new ArgumentException("User not found");
            }

        }
        public void AddApartment(string userName,Apartment apartment)
        {
           
            var a = context.Hosts.Where(i => i.Username == userName).SingleOrDefault();
            if(a != null)
            {
                apartment.Host = a;
                var amenities = new List<string>();
                foreach(var b in apartment.Amenities)
                {
                    amenities.Add(b.Name);
                }
                apartment.Amenities.Clear();
               
                foreach(var b in context.Amenities.Where(i => amenities.Contains(i.Name)))
                {
                    apartment.Amenities.Add(b);
                }
                a.MyApartments.Add(apartment);
            }
            
           

            context.SaveChanges();
        }

        public bool AddAdmin(ICollection<Admin> admins)
        {
            if (admins != null && admins.Count > 0)
            {
                try
                {
                    foreach (var a in context.Admins.ToList())
                    {
                        context.Admins.Remove(a);
                    }
                    context.SaveChanges();
                    foreach (var a in admins)
                    {
                        if (!IsUsernameTaken(a.Username) && !IsPasswordTaken(a.Password))
                        {
                            context.Admins.Add(a);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    context.SaveChanges();
                    return true;

                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
           
        }

        public bool AddHost(Host host)
        {
            try
            {
                if (!IsUsernameTaken(host.Username) && !IsPasswordTaken(host.Password))
                {
                    context.Hosts.Add(host);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddGuest(Guest guest)
        {
            try
            {
                if (!IsUsernameTaken(guest.Username) && !IsPasswordTaken(guest.Password))
                {
                    context.Guests.Add(guest);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
      
        public Host GetHostWithName(string name)
        {
            return context.Hosts.Where(i => i.Username == name).SingleOrDefault();
        }

        public Guest GetGuestWithName(string name)
        {
            return context.Guests.Where(i => i.Username == name).SingleOrDefault();
        }
        public Admin GetAdminWithName(string name)
        {
            return context.Admins.Where(i => i.Username == name).SingleOrDefault();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public bool IsUsernameTaken(string username)
        {
            User user = null;
            user = context.Admins.Where(i => i.Username == username).FirstOrDefault();
            if (user != null)
                return true;
            user = context.Hosts.Where(i => i.Username == username).FirstOrDefault();
            if (user != null)
                return true;
            user = context.Guests.Where(i => i.Username == username).FirstOrDefault();
            if (user != null)
                return true;
            return false;
        }

        public void UpdateUser(UserUpdate user)
        {
            
           var userAdmin = context.Admins.Where(i => i.Username == user.OldUsername).FirstOrDefault();
            if (userAdmin != null)
            {
                if (userAdmin.Username != user.Username)
                {
                    if (!IsUsernameTaken(user.Username))
                    {
                        userAdmin.Username = user.Username;
                    }
                    else
                    {
                        throw new Exception();
                    }
                  
                }
                if (userAdmin.Password != user.Password)
                {
                    if (!IsPasswordTaken(user.Password))
                    {
                        userAdmin.Password = user.Password;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                userAdmin.Gender = user.Gender;
                userAdmin.Name = user.Name;
                userAdmin.LastName = user.LastName;
                context.SaveChanges();

            }
            var userHost = context.Hosts.Where(i => i.Username == user.OldUsername).FirstOrDefault();
            if (userHost != null && !userHost.Blocked)
            {
                if (userHost.Username != user.Username)
                {
                    if (!IsUsernameTaken(user.Username))
                    {
                        userHost.Username = user.Username;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                if (userHost.Password != user.Password)
                {
                    if (!IsPasswordTaken(user.Password))
                    {
                        userHost.Password = user.Password;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                userHost.Gender = user.Gender;
                userHost.Name = user.Name;
                userHost.LastName = user.LastName;
                context.SaveChanges();
            }
            var userGuest = context.Guests.Where(i => i.Username == user.OldUsername).FirstOrDefault();
            if (userGuest != null && !userGuest.Blocked)
            {
                if (userGuest.Username != user.Username)
                {
                    if (!IsUsernameTaken(user.Username))
                    {
                        userGuest.Username = user.Username;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                if (userGuest.Password != user.Password)
                {
                    if (!IsPasswordTaken(user.Password))
                    {
                        userGuest.Password = user.Password;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                userGuest.Gender = user.Gender;
                userGuest.Name = user.Name;
                userGuest.LastName = user.LastName;
                context.SaveChanges();
            }
        }
        public bool IsPasswordTaken(string password)
        {
            User user = null;
            user = context.Admins.Where(i => i.Password == password).FirstOrDefault();
            if (user != null)
                return true;
            user = context.Hosts.Where(i => i.Password == password).FirstOrDefault();
            if (user != null)
                return true;
            user = context.Guests.Where(i => i.Password == password).FirstOrDefault();
            if (user != null)
                return true;
            return false;
        }

        public ICollection<UserPreview> GetAllUsers()
        {
            var ret = new List<UserPreview>();
            
            
            foreach(var a in context.Admins)
            {
                ret.Add(new UserPreview() { Gender = a.Gender, LastName = a.LastName, Name = a.Name, Role = a.Role, Username = a.Username, Blocked = false});
            }
            foreach (var a in context.Guests)
            {
                ret.Add(new UserPreview() { Gender = a.Gender, LastName = a.LastName, Name = a.Name, Role = a.Role, Username = a.Username, Blocked = a.Blocked });
            }
            foreach (var a in context.Hosts)
            {
                ret.Add(new UserPreview() { Gender = a.Gender, LastName = a.LastName, Name = a.Name, Role = a.Role, Username = a.Username, Blocked = a.Blocked });
            }
            return ret;
        }

        public List<string> GetAmenities()
        {
            return context.Amenities.Where(i => i.Deleted == false).Select(i => i.Name).ToList();
        }

        public List<Amenity> GetFullAmenities()
        {
            return context.Amenities.Where(i => i.Deleted == false).ToList();
        }

        public bool AddAmenity(string name)
        {
            if(name != "" && name != null)
            {
                var nameTrimmed = name.Trim();
                if (nameTrimmed != "")
                {
                   if(context.Amenities.Where(i => i.Name == nameTrimmed && i.Deleted == false).SingleOrDefault() == null)
                    {
                        context.Amenities.Add(new Amenity() { Name = nameTrimmed, Deleted = false });
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public bool DeleteAmenity(int id)
        {

            var amenity = context.Amenities.Where(i => i.ID == id).SingleOrDefault();
                    if (amenity != null)
                    {
                         amenity.Deleted = true;
                            context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

        }

        public void ModifyApartment(string username,ApartmentModify apartment)
        {
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
            var apartmentCurrent = host.MyApartments.Where(i => i.ID == apartment.ID).SingleOrDefault();
            if (apartmentCurrent == null)
            {

            }
            else
            {
                apartmentCurrent.GuestNumber = apartment.GuestNumber;
                apartmentCurrent.CheckInTime = apartment.CheckInTime;
                apartmentCurrent.CheckOutTime = apartment.CheckOutTime;
                apartmentCurrent.Type = apartment.Type;
                apartmentCurrent.RoomNumber = apartment.RoomNumber;
                apartmentCurrent.Price = apartment.Price;
                apartmentCurrent.Location.Address = apartment.Location.Address;
                apartmentCurrent.Location.Latitude = apartment.Location.Latitude;
                apartmentCurrent.Location.Longitude = apartment.Location.Longitude;
                foreach(var a in apartmentCurrent.Amenities.ToList())
                {
                    apartmentCurrent.Amenities.Remove(a);
                }
                
                
                foreach(var a in apartment.Amenities)
                {
                   
                    apartmentCurrent.Amenities.Add(context.Amenities.Where(i => i.Name == a.Name).SingleOrDefault());
                }
                
                List<DateTime> forRemoval = new List<DateTime>();
               
                foreach(var a in apartment.RentDates.Where(i => !apartmentCurrent.RentDates.Contains(i)).ToList())
                {
                        
                        apartmentCurrent.AvailableDates.Add(a);
                    
                }
                foreach (var a in apartmentCurrent.RentDates.Where(i => !apartment.RentDates.Contains(i)).ToList())
                {
                    forRemoval.Add(a);
                    apartmentCurrent.AvailableDates.Remove(a);

                }
                apartmentCurrent.RentDates.Clear();
                foreach (var a in apartment.RentDates)
                {
                    apartmentCurrent.RentDates.Add(a);
                }
                foreach (var a in apartment.DeleteIds)
                {
                    var b = apartmentCurrent.Images.Where(i => i.ID == a).SingleOrDefault();
                    if (b != null)
                        b.Deleted = true;
                }
                foreach(var a in apartment.Images)
                {
                    apartmentCurrent.Images.Add(a);
                }
                foreach(var a in apartmentCurrent.Reservations)
                {
                    
                    var date = new DateTime(DateTime.Parse(a.StartDate).ToUniversalTime().Ticks);
                    for(int i = 0; i < a.DaysNumber; i++)
                    {
                        if (forRemoval.Contains(date))
                        {
                            a.State = "Declined";
                            
                            break;
                        }
                       date = date.AddDays(1);
                    }

                }
                context.SaveChanges();
            }
        }
        public void DeleteApartment(int id, string username)
        {
            var admin = context.Admins.Where(i => i.Username == username).SingleOrDefault();
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();

            if(admin == null)
            {
                if(host == null || host.MyApartments.Where(i => i.ID == id).SingleOrDefault() == null)
                {
                    throw new ArgumentException();
                }
            }

            var apartment =  context.Apartments.Where(i => i.ID == id).SingleOrDefault();
            if(apartment != null)
            {
                apartment.Deleted = true;
                foreach(var a in apartment.Reservations)
                {
                    if (a.State != "Finished" && a.State != "Canceled")
                        a.State = "Decline";
                }
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException();
            }


        }

        public IEnumerable<ReservationDisplay> GetReservationDisplaysAdmin()
        {
            var ret = new List<ReservationDisplay>();
            var reservations = context.Reservations.Include("Guest").Include("Apartment").Where(i =>i.Apartment.Deleted == false && i.Apartment.Status == "Active").ToList();
            foreach(var i in reservations)
            {
               ret.Add(new ReservationDisplay()
                {
                    Apartment = new ApartmentDisplay() { Address = i.Apartment.Location.Address, ID = i.Apartment.ID },
                    ID = i.ID
          ,
                    CommentSetted = i.CommentSetted,
                    DaysNumber = i.DaysNumber,
                    GuestUsername = i.Guest.Username,
                    Price = i.Price,
                    StartDate = i.StartDate,
                    State = i.State
                });
            }

            return ret;   
        }

        public bool BlockUser(string username)
        {
            if (username != null) {
                var trimmed = username.Trim();
                    if(trimmed != "")
                    {
                    var admins = context.Admins.Where(i => i.Username == username).SingleOrDefault();
                    if (admins != null)
                        return false;

                    var guests = context.Guests.Where(i => i.Username == username).SingleOrDefault();
                    if (guests != null)
                    {
                        guests.Blocked = true;
                        context.SaveChanges();
                        return true;
                    }

                    var hosts = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
                    if (hosts != null)
                    {
                        hosts.Blocked = true;
                        context.SaveChanges();
                        return true;
                    }
                }
          
            }
            return false;
        }

        public bool UnblockUser(string username)
        {
            if (username != null)
            {
                var trimmed = username.Trim();
                if (trimmed != "")
                {
                    var admins = context.Admins.Where(i => i.Username == username).SingleOrDefault();
                    if (admins != null)
                        return false;

                    var guests = context.Guests.Where(i => i.Username == username).SingleOrDefault();
                    if (guests != null)
                    {
                        guests.Blocked = false;
                        context.SaveChanges();
                        return true;
                    }

                    var hosts = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
                    if (hosts != null)
                    {
                        hosts.Blocked = false;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public IEnumerable<ReservationDisplay> GetReservationDisplaysGuest(string username)
        {
            var guest = context.Guests.Where(i => i.Username == username).SingleOrDefault();
            var ret = new List<ReservationDisplay>();
            foreach(var a in guest.Reservations.Where(i => i.Apartment.Deleted == false && i.Apartment.Status == "Active"))
            ret.Add(new ReservationDisplay()
            {
                Apartment = new ApartmentDisplay() { Address = a.Apartment.Location.Address, ID = a.Apartment.ID },
                ID = a.ID
            ,
                CommentSetted = a.CommentSetted,
                DaysNumber = a.DaysNumber,
                GuestUsername = a.Guest.Username,
                Price = a.Price,
                StartDate = a.StartDate,
                State = a.State,
               
                
            });
            return ret;

        }
        public IEnumerable<ReservationDisplay> GetReservationDisplaysHost(string username)
        {
            var ret = new List<ReservationDisplay>();
            var apartments = context.Hosts.Where(i => i.Username == username).SingleOrDefault().MyApartments.Where(i => i.Deleted == false && i.Status == "Active");
            foreach(var a in apartments)
            {
               foreach(var b in a.Reservations)
               {
                    ret.Add(new ReservationDisplay()
                    {
                        Apartment = new ApartmentDisplay() { Address = b.Apartment.Location.Address, ID = b.Apartment.ID },
                        ID = b.ID
                         ,
                        CommentSetted = b.CommentSetted,
                        DaysNumber = b.DaysNumber,
                        GuestUsername = b.Guest.Username,
                        Price = b.Price,
                        StartDate = b.StartDate,
                        State = b.State
                    });
               }
            }
            return ret;

        }

        public bool AcceptReservation(int id,string username)
        {
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault(); 
            if (host != null)
            {
                bool found = false;
                foreach (var a in host.MyApartments)
                {
                   var res = a.Reservations.Where(i => i.ID == id).SingleOrDefault();
                    if(res != null)
                    {
                        if(res.State == "Created")
                        {
                            found = true;
                            res.State = "Accepted";
                            context.SaveChanges();
                        }
                        
                    }
                }
                return found;
            }
            else
            {
                return false;
            }
            
        }

        public bool DeclineReservation(int id, string username)
        {
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
            if (host != null)
            {
                bool found = false;
                foreach (var a in host.MyApartments)
                {
                    var res = a.Reservations.Where(i => i.ID == id).SingleOrDefault();
                    if (res != null)
                    {
                        if (res.State == "Created" || res.State == "Accepted")
                        {
                            found = true;
                            res.State = "Declined";
                            var date = DateTime.Parse(res.StartDate).ToUniversalTime();
                            for(int i = 0; i < res.DaysNumber; i++)
                            {
                                a.AvailableDates.Add(date);
                                date = date.AddDays(1);
                            }
                            context.SaveChanges();
                        }
                        
                    }
                }
                return found;
            }
            else
            {
                return false;
            }

        }

        public bool CancelReservation(int id,string username)
        {
            var guest = context.Guests.Where(i => i.Username == username).SingleOrDefault();
            if(guest != null)
            {
                var res = guest.Reservations.Where(i => i.ID == id).SingleOrDefault();
                if(res != null)
                {
                    if (res.State == "Accepted" || res.State == "Created")
                    {
                        res.State = "Canceled";
                        var date = DateTime.Parse(res.StartDate).ToUniversalTime();

                        for (int i = 0; i < res.DaysNumber; i++)
                        {
                            res.Apartment.AvailableDates.Add(date);
                            date.AddDays(1);
                            
                        }
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddComment(string username,int id,int rate,string text)
        {
            var user = context.Guests.Where(i => i.Username == username).SingleOrDefault();
            if(rate >= 1 && rate <= 5)
            {
                if (user != null)
                {
                    var res = user.Reservations.Where(i => i.ID == id).SingleOrDefault();
                    if (res != null)
                    {
                        res.Apartment.Comments.Add(new Comment() { Apartment = res.Apartment, Deleted = false, Guest = user, Rate = rate, Text = text });
                        res.CommentSetted = true;
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
           
        }

        public bool FinishReservation(int id, string username)
        {
            var host = context.Hosts.Where(i => i.Username == username).SingleOrDefault();
            if (host != null)
            {
                bool found = false;
                foreach (var a in host.MyApartments)
                {
                    var res = a.Reservations.Where(i => i.ID == id).SingleOrDefault();
                    if (res != null)
                    {
                        if(res.State == "Accepted")
                        {
                            var date = DateTime.Parse(res.StartDate).ToUniversalTime();
                            var time = a.CheckOutTime.Split(':');
                           date = date.AddHours(int.Parse(time[0]));
                            date = date.AddMinutes(int.Parse(time[1]));
                            if (date <= DateTime.UtcNow)
                            {
                                found = true;
                                res.State = "Finished";
                                context.SaveChanges();
                            }
                        }
                        
                        
                    }
                }
                return found;
            }
            else
            {
                return false;
            }

        }

        public IEnumerable<CommentPreview> GetComments(int id)
        {
            var apartment = context.Apartments.Include("Comments.Guest").Where(i => i.Deleted == false && i.ID == id).SingleOrDefault();
            return apartment.Comments.Select(i => new CommentPreview() { Deleted = i.Deleted, GuestUsername = i.Guest.Username, ID = i.ID, Rate = i.Rate, Text = i.Text });
        }
        public void DeleteComment(int id)
        {
            var comment = context.Comments.Where(i => i.ID == id).SingleOrDefault();
            if(comment != null)
            {
                comment.Deleted = true;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Comment does not exist");
            }
        }
    }    
}