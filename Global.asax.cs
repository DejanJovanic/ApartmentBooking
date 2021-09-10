using ProjekatWeb.Models;
using ProjekatWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;

namespace ProjekatWeb
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            using(var repo = new DataRepo())
            {
                repo.AddAdmin(AddAdmins());
            }
        }

        public static ICollection<Admin> AddAdmins()
        {
         
            List<Admin> ret = null;
            try
            {
                var admins = XElement.Load(HostingEnvironment.MapPath("~/App_Data/Admins.txt"));
                ret = new List<Admin>();
                foreach(var a in admins.Elements("Admin"))
                {
                    ret.Add(new Admin() { Gender = a.Element("Gender").Value, LastName = a.Element("LastName").Value, Name = a.Element("Name").Value, Password = a.Element("Password").Value, Username = a.Element("Username").Value, Role = "Admin" });
                }
                return ret;
            }
            catch
            {
                ret = new List<Admin>();
                return ret;
            }
        }
    }
}
