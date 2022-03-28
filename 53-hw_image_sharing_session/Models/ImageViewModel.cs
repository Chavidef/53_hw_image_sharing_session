using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _53_hw_image_sharing_session.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public bool CorrectPassword { get; set; }
        public string ImagePath { get; set; }
        public string Message { get; set; }
        public bool HasAccess { get; set; }
        public int Views { get; set; }
    }
}
