using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCImageGallery.Controllers
{
    public class ImageGalleryController : Controller
    {
        //
        // GET: /ImageGallery/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            List<ImageGallery> all = new List<ImageGallery>();
            
            // Here MyDatabaseEntities is our datacontext
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                all = dc.ImageGalleries.ToList();
            }
            return View(all);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(ImageGallery IG)
        {
            // Apply Validation Here

            
            if (IG.File.ContentLength > (2*1024*1024))
            {
                ModelState.AddModelError("CustomError", "File size must be less than 2 MB");
                return View();
            }
            if (!(IG.File.ContentType == "image/jpeg" || IG.File.ContentType == "image/gif"))
            {
                ModelState.AddModelError("CustomError", "File type allowed : jpeg and gif");
                return View();
            }

            IG.FileName = IG.File.FileName;
            IG.ImageSize = IG.File.ContentLength;

            byte[] data = new byte[IG.File.ContentLength];
            IG.File.InputStream.Read(data, 0, IG.File.ContentLength);

            IG.ImageData = data;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                dc.ImageGalleries.Add(IG);
                dc.SaveChanges();
            }
            return RedirectToAction("Gallery");
        }
    }
}
