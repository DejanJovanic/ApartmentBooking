using ProjekatWeb.Models;
using ProjekatWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Authentication
{
    public class AuthorisationRepo : IDisposable
    {
        ApartmentContext context = new ApartmentContext();

        public User ValidateUser(string username, string password)
        {
            User ret = null;
            ret = context.Guests.FirstOrDefault(user =>
            user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password == password && !user.Blocked);
            if (ret != null)
                return ret;
            ret = context.Hosts.FirstOrDefault(user =>
            user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password == password && !user.Blocked);
            if (ret != null)
                return ret;
            ret = context.Admins.FirstOrDefault(user =>
            user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password == password);

            return ret;
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}