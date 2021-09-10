using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public virtual Guest Guest { get; set; }
        public bool Deleted { get; set; }
        public Apartment Apartment { get; set; }
        public string Text { get; set; }

        public int Rate { get; set; }
    }
}