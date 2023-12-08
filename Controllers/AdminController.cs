using James_Thew.context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace James_Thew.Controllers
{
    public class AdminController : Controller
    {
        James_ThewEntities1 db=new James_ThewEntities1();
        // GET: Admin
        public ActionResult Index()
        {
        
            return View();
        }

        [ChildActionOnly]
        public ActionResult pv()
        {
            var pro_img = db.Profile_Image.ToList();
            return PartialView("profile_img", pro_img);
        }
      
        public ActionResult ActivateForm()
        {
            // Set a session variable or update a database flag to activate the form
            Session["IsFormActivated"] = true;
            TempData["MessAct"] = "Form is Active Successfully";
            return RedirectToAction("Index","Admin");
        }

        public ActionResult DeactivateForm()
        {
            // Set a session variable or update a database flag to deactivate the form
            Session["IsFormActivated"] = false;
            TempData["MessDe"] = "Form is Detactive Successfully";
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult ActivateWinners()
        {
            // Set a separate session variable to activate winners
            Session["AreWinnersActivated"] = true;
            TempData["MessActWinner"] = "Winners is Active Successfully";
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeactivateWinners()
        {
            // Set a separate session variable to deactivate winners
            Session["AreWinnersActivated"] = false;
            TempData["MessDeWinner"] = "Winners is Detactive Successfully";
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult profileimage(HttpPostedFileBase ImageFile)
        {
            string imagePath;
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var userId = User.Identity.GetUserId();
                var existingProfileImage = db.Profile_Image.FirstOrDefault(p => p.User_Id == userId);

                string filename = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                 imagePath = Path.Combine(Server.MapPath("~/All_Image/"), filename);
                ImageFile.SaveAs(imagePath);

                if (existingProfileImage != null)
                {
                    // If an existing profile image exists, update its Image property
                    existingProfileImage.Image = "~/All_Image/" + filename;
                }
                else
                {
                    // If no existing profile image, create a new record
                    var model = new Profile_Image
                    {
                        User_Id = userId,
                        Image = "~/All_Image/" + filename
                    };
                    db.Profile_Image.Add(model);
                }

                db.SaveChanges(); // Save changes tsssso update or create the record
                return Json("/All_Image/" + filename);
            }

            return View();
        }


    }
}