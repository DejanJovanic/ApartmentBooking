using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Admin : User
    {
        public Admin()
        {
            Role = "Admin";
        }
    }
}