using James_Thew.context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace James_Thew.Controllers
{
   

    public class FeedbackController : Controller
    {
        James_ThewEntities1 dbObj = new James_ThewEntities1();
        // GET: Feedback
        [Authorize]
        public ActionResult Feedback(Feedback model)
        {
            string user = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {

            
            
            Feedback feedback = new Feedback()
            {
                 User_Id = user,
                 Contents= model.Contents,
                 DatePosted=DateTime.Now,
                 Rating=model.Rating,
            };
                dbObj.Feedback.Add(feedback);
                dbObj.SaveChanges();

                TempData["Subs"] = "Submit Feebback successfully.";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        public ActionResult Feedbacks()
        {
            var feed=dbObj.Feedback.ToList();
            return View(feed);  
        }
       
    }
}