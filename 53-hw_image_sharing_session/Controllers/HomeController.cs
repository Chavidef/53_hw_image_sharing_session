using _53_hw_image_sharing_session.Data;
using _53_hw_image_sharing_session.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _53_hw_image_sharing_session.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ImageSharing; Integrated Security=true;";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, Image image)
        {
            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var repo = new ImageRepository(_connectionString);
            image.Id = repo.UploadImage(image.Password, fileName);


            return View(new ImageViewModel { Id = image.Id, Password = image.Password });
        }
        public IActionResult ViewImage(int id, string password)
        {
            var repo = new ImageRepository(_connectionString);
            var image = repo.GetImageById(id);

            var idList = HttpContext.Session.Get<List<int>>("IdList");

            var vm = new ImageViewModel
            {
                Id = id,
                ImagePath = image.ImageURL,
                CorrectPassword = false,
                Message = (string)TempData["Message"],
                Views = image.Views
            };
            if(image.Password == password)
            {
                vm.CorrectPassword = true;
                if(idList == null)
                {
                    idList = new List<int>();
                }
                idList.Add(id);
                vm.HasAccess = true;
                HttpContext.Session.Set("IdList", idList);
                repo.IncrementView(id);

            }
            if (image.Password != password)
            {
                vm.CorrectPassword = false;
                TempData["Message"] = "Incorrect Password";
            }
            //vm.Message = "Invalid Password";//bug is that it's being passed into the view as this message so it shows right when the view shows


            return View(vm);
        }
    }

}

