using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class UserUpdate
    {
        public string OldUsername { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        
    }
}