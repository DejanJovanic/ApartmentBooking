using ProjekatWeb.Models;
using ProjekatWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjekatWeb.Controllers
{
    [RoutePrefix("Account")]
    public class UserController : ApiController
    {
        [Route("AddHost")]
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public bool AddHost(Host host)
        {
            using(var repo = new DataRepo())
            {
                return repo.AddHost(host);
            }
        }
        [Route("AddGuest")]
        [HttpPost]
        public bool AddGuest(Guest guest)
        {
            using (var repo = new DataRepo())
            {
                return repo.AddGuest(guest);
            }
        }

        [Route("UpdateAccount")]
        [HttpPost]
        public IHttpActionResult UpdateAccount(UserUpdate user)
        {
            try
            {
                using (var repo = new DataRepo())
                {
                   repo.UpdateUser(user);
                }
                return Content(HttpStatusCode.OK,"All good..");
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }

        }

        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize(Roles ="Admin,Guest,Host")]
        public User GetUserInfo()
        {
            using(var repo = new DataRepo())
            {
                if (User.IsInRole("Host"))
                {
                   
                    var host = repo.GetHostWithName(User.Identity.Name);
                    User ret = new User() { Gender = host.Gender, LastName = host.LastName, Name = host.Name, Password = host.Password, Username = host.Username, Role = host.Role };
                    return ret;
                }else if (User.IsInRole("Guest"))
                {
                    var host = repo.GetGuestWithName(User.Identity.Name);
                    User ret = new User() { Gender = host.Gender, LastName = host.LastName, Name = host.Name, Password = host.Password, Username = host.Username, Role = host.Role };
                    return ret;
                }
                return repo.GetAdminWithName(User.Identity.Name);
            }
        }

        [HttpGet]
        [Route("IsUsernameTaken")]       
        public bool CheckUsername(string username)
        {
            using (var repo = new DataRepo())
            {
                return repo.IsUsernameTaken(username);
            }
        }


        [HttpGet]
        [Route("IsPasswordTaken")]     
        public bool CheckPassword(string password)
        {
            using (var repo = new DataRepo())
            {
                return repo.IsPasswordTaken(password);
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles ="Admin")]
        public ICollection<UserPreview> GetAllUsers()
        {
            using(var repo = new DataRepo())
            {
                return repo.GetAllUsers();
            }
        }

        [HttpGet]
        [Route("BlockUser")]
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage BlockUser(string username)
        {
            using (var repo = new DataRepo())
            {
                if (repo.BlockUser(username))
                    return new HttpResponseMessage(HttpStatusCode.OK);
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("UnblockUser")]
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage UnblockUser(string username)
        {
            using (var repo = new DataRepo())
            {
                if (repo.UnblockUser(username))
                    return new HttpResponseMessage(HttpStatusCode.OK);
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("Search")]
        [Authorize(Roles ="Admin")]
        public ICollection<UserPreview> Search(UserPreview search)
        {
            ICollection<UserPreview> users;
            using(var repo = new DataRepo())
            {
                users = repo.GetAllUsers();
            }

            return Filter(users, search);
        }


        [HttpGet]
        [Route("GetPreview")]
        [Authorize(Roles = "Admin,Guest,Host")]
        public UserPreview GetPreview()
        {
            ICollection<UserPreview> users;
            using (var repo = new DataRepo())
            {
                users = repo.GetAllUsers();
            }

            return users.Where(i => i.Username == User.Identity.Name).SingleOrDefault();
        }

        private ICollection<UserPreview> Filter(ICollection<UserPreview> displays, UserPreview search)
        {
            List<UserPreview> ret = new List<UserPreview>();
            foreach (var a in displays)
            {
   
                if (search.Username != "" && search.Username != null)
                {
                    if (a.Username != search.Username)
                        continue;
                }
               
                if (search.Gender != "" && search.Gender != null && search.Gender != "All")
                {
                    if (a.Gender != search.Gender)
                        continue;
                }
                if (search.Role!= "" && search.Role != null && search.Role != "All")
                {
                    if (a.Role != search.Role)
                        continue;
                }

                ret.Add(a);
            }
            return ret;
        }
    }


}
