using James_Thew.context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace James_Thew.Controllers
{
    public class ContestController : Controller
    {
        James_ThewEntities1 db=new James_ThewEntities1();
        [Authorize]
        // GET: Contest
        public ActionResult Contest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contest(User_recepie model)
        {
            string userId = User.Identity.GetUserId();

            // Check if the user has an active recipe
            bool hasActiveRecipe = db.User_recepie.Any(r => r.User_Id == userId && r.IsActive);

            if (hasActiveRecipe)
            {
                // User has an active recipe, prevent them from posting a new one
                TempData["ErrorMessage"] = "You have already posted a recepies.";
                return RedirectToAction("Index", "Home"); // Redirect to home or appropriate page
            }

            // User can post a new recipe since they don't have an active one
            if (ModelState.IsValid)
            {
                User_recepie recepies = new User_recepie()
                {
                    User_Id = userId,
                    Title = model.Title,
                    Ingrdient = model.Ingrdient,
                    CookingProcedure = model.CookingProcedure,
                    Post=model.Post,
                    IsActive = true // Set the new recipe as active
                };

                db.User_recepie.Add(recepies);
                db.SaveChanges();
                TempData["Error"] = "Thank You for posting a recepies.";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        public ActionResult viewContest()
        {
            var list=db.User_recepie.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult winner(int id, winnerstable model)
        {
            
           
            if (ModelState.IsValid)
            {

                winnerstable win = new winnerstable()
                {
                    User_Id =model.User_Id,
                    User_rec_id = id
                };
                bool hasWinner = db.winnerstable.Any(r => r.User_Id ==model.User_Id);
                if (hasWinner)
                {
                    // User has an active recipe, prevent them from posting a new one
                   
                    return RedirectToAction("Index", "Admin"); // Redirect to home or appropriate page
                }
                else {
                    db.winnerstable.Add(win);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
               
            };
            return View(model);
          
        }
        public ActionResult Result()
        {
            var res = db.winnerstable.ToList();
            return View(res);
        }
        public ActionResult showWinners()
        {
            var res = db.winnerstable.ToList();
            return View(res);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           User_recepie user_rece = db.User_recepie.Find(id);
            if (user_rece == null)
            {
                return HttpNotFound();
            }
            return View(user_rece);
        }
        public ActionResult Delete(int id)
        {
            var res = db.winnerstable.Where(x => x.Id == id).First();
            db.winnerstable.Remove(res);
            db.SaveChanges();

            return RedirectToAction("showWinners", "Contest");
        }

    }
}