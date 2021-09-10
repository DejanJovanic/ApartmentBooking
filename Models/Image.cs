using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class Image
    {
        public int ID { get; set; }

        public string Path { get; set; }
        public bool Deleted { get; set; }
    }
}