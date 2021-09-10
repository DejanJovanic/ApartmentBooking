using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class UserPreview
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }

        public bool Blocked { get; set; }
    }
}