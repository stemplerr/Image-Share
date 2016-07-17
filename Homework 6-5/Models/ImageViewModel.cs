using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Images.Data;

namespace Homework_6_5.Models
{
    public class ImageViewModel
    {
        public bool isAuthenticated { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ImageWithLikes> MostPopular { get; set; }
        public IEnumerable<ImageWithLikes> MostRecent { get; set; }
        
    }
}