using System;

namespace _53_hw_image_sharing_session.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public string Password { get; set; }
        public int Views  {get; set; }
    }
}
