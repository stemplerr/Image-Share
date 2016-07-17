using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Images.Data;

namespace Homework_6_5.Models
{
    public class ShowImageViewModel
    {
        public Image Image { get; set; }
        public int Likes { get; set; }
        public bool isAuthenticated { get; set; }
        public bool hasUserLiked { get; set; }
    }
}