using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Images.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ImageFileName { get; set; }
        public DateTime DateUploaded { get; set; }
        public int Hits { get; set; }
    }
}
