using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatWeb.Models
{
    public class CommentPreview
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public int Rate { get; set; }

        public bool Deleted { get; set; }
        public string GuestUsername { get; set; }
    }
}