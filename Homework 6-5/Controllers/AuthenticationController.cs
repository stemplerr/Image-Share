using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Images.Data;

namespace Homework_6_5.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/

        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            UserManager manager = new UserManager(Properties.Settings.Default.ConnString);
            User u =  manager.LogIn(username, password);
            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(username, true);
            }
            return Redirect("/");
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string firstname, string lastname, string username, string password1)
        {
            UserManager manager = new UserManager(Properties.Settings.Default.ConnString);
            manager.AddUser(firstname, lastname, username, password1);
            FormsAuthentication.SetAuthCookie(username, true);
            return Redirect("/");
        }

        public ActionResult SignOut ()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
