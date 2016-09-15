using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Images.Data;
using Homework_6_5.Models;

namespace Homework_6_5.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        public ActionResult Index()
        {
            ImageManager manager = new ImageManager(Properties.Settings.Default.ConnString);
            UserManager um = new UserManager(Properties.Settings.Default.ConnString);
            ImageViewModel vm = new ImageViewModel();
            vm.MostPopular = manager.GetMostPopular();
            vm.MostRecent = manager.GetMostRecent();
            vm.isAuthenticated = User.Identity.IsAuthenticated;
            if (vm.isAuthenticated)
            {
                vm.UserName = um.GetUser(User.Identity.Name).FirstName;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase image, string username, DateTime dateuploaded)
        {
            ImageManager manager = new ImageManager(Properties.Settings.Default.ConnString);
            LinkViewModel vm = new LinkViewModel();
            string filename = Guid.NewGuid() + Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath("~/Images/") + filename);
            Image newImage = new Image
            {
                ImageFileName = filename,
                UserName = username,
                DateUploaded = dateuploaded,
                Hits = 0
            };
            newImage.Id = manager.AddImage(newImage);
            vm.Image = newImage;
            vm.HostName = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            return View(vm);
        }

        public ActionResult ShowImage(int id)
        {
            ImageManager manager = new ImageManager(Properties.Settings.Default.ConnString);
            manager.IncrementViews(id);
            Image image = manager.GetImageById(id);
            ShowImageViewModel vm = new ShowImageViewModel();
            vm.Image = image;
            vm.Likes = manager.GetLikeCount(id);
            vm.isAuthenticated = User.Identity.IsAuthenticated;
            if (User.Identity.IsAuthenticated)
            {
               vm.hasUserLiked = manager.CheckIfUserHasLiked(User.Identity.Name, id);
            }
            return View(vm);
        }

        public ActionResult LikeImage(int imageid)
        {
            ImageManager manager = new ImageManager(Properties.Settings.Default.ConnString);
            UserManager um = new UserManager(Properties.Settings.Default.ConnString);
            User u = um.GetUser(User.Identity.Name);
            manager.AddImageLike(u.Id, imageid);
            return Redirect("/image/showimage?id=" + imageid);
        }

    }
}
