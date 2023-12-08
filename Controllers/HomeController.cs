using James_Thew.context;
using James_Thew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace James_Thew.Controllers
{
    public class HomeController : Controller

    {
       private James_ThewEntities1 dbObj = new James_ThewEntities1();
        private UserManager<ApplicationUser> UserManager;

        public HomeController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        public bool UserHasActiveSubscription(string userId)
        {
            // Retrieve the user's subscriptions from the database based on their userId
            var userSubscriptions = dbObj.Subscription
                .Where(s => s.User_Id == userId && s.SubscriptionStatus == "Active")
                .ToList();

            // Check if there are any active subscriptions for the user
            return userSubscriptions.Any();
        }



        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                
                return RedirectToAction("Index", "Admin");
                
            }
            else
            {
                //var pro_img = dbObj.Profile_Image.ToList();
                var userId = User.Identity.GetUserId(); // Assuming you are using ASP.NET Identity
                var announcements = dbObj.Anouncement.ToList();
                var allRecipes = dbObj.Racipies_james.ToList();
                var tip=dbObj.Tips.ToList();

                var fqas = dbObj.FAQs.ToList();
                // Check if the user has an active subscription
                bool hasActiveSubscription = UserHasActiveSubscription(userId);

                // Filter recipes based on the user's subscription status and whether they are free
                var accessibleRecipes = allRecipes.Where(recipe => recipe.IsFree==true || recipe.IsFree==false);

                var viewModel = new MyViewModel
                {
                    //img=pro_img,
                    Tips=tip,
                    Announcements = announcements,
                    Recipes = accessibleRecipes,
                    HasActiveSubscription = hasActiveSubscription,
                    FAQs=fqas,  
                };

                return View(viewModel);
            }
        }



        [Authorize] // Ensures the user is authenticated
        public ActionResult Edit()
        {
            // Retrieve the current user based on their identity
            var userId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(userId);

            if (user == null)
            {
                // Handle the case where the user doesn't exist
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var currentUser = await UserManager.FindByIdAsync(userId);

                if (currentUser == null)
                {
                    // Handle the case where the user doesn't exist
                    return HttpNotFound();
                }

                // Update the properties of the existing user
                currentUser.UserName = model.UserName.Replace(" ", "");
                currentUser.Email = model.Email;
                currentUser.Name = model.Name;
                currentUser.Phone = model.Phone;
                currentUser.Address = model.Address;

                // Save the changes to the database
                var result = await UserManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var identity = UserManager.CreateIdentity(currentUser, DefaultAuthenticationTypes.ApplicationCookie);
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
                  
                    return RedirectToAction("Index", "Home"); // Redirect to the home page
                
                }
                else
                {
                    // Log the errors for debugging
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    // Handle the case where the update was not successful
                    return View(model);
                }
            }

            // If ModelState is not valid, redisplay the form
            return View(model);
        }

        public ActionResult Details_Recp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racipies_james racipies_james = dbObj.Racipies_james.Find(id);
            if (racipies_james == null)
            {
                return HttpNotFound();
            }
            return View(racipies_james);
        }




        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscription(Subscription model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();

                using (var dbContext = new James_ThewEntities1())
                {
                    // Find the user's existing subscription, if any
                    var existingSubscription = dbContext.Subscription
                        .FirstOrDefault(s => s.User_Id == userId);

                    if (existingSubscription != null)
                    {
                        if (existingSubscription.SubscriptionStatus == "InActive")
                        {
                            // If an existing subscription is found with status "Inactive," update its type and status
                            existingSubscription.SubscriptionType = model.SubscriptionType;
                            existingSubscription.SubscriptionStatus = "Active";
                            existingSubscription.SubscriptionDate = DateTime.Now;

                            TempData["Sub"] = "Your subscription has been updated.";
                        }
                        else
                        {
                            // User already has an active subscription
                            TempData["Sub"] = "You already have an active subscription.";
                        }
                    }
                    else
                    {
                        // If no existing subscription is found, create a new one with status "Active"
                        var newSubscription = new Subscription
                        {
                            User_Id = userId,
                            SubscriptionType = model.SubscriptionType,
                            SubscriptionDate = DateTime.Now,
                            SubscriptionStatus = "Active"
                        };

                        dbContext.Subscription.Add(newSubscription);

                        TempData["Sub"] = "You subscribed successfully.";
                    }

                    dbContext.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

      
        [Authorize]
        
        public ActionResult viewSub()
        {
           
                var res = dbObj.Subscription.ToList();
            return View(res);
        }

        public ActionResult InActive(int id)
        {
            var sub = dbObj.Subscription.Find(id);

            sub.SubscriptionStatus = "InActive"; // Correct the status spelling if needed
            dbObj.SaveChanges();

            return RedirectToAction("viewSub","Home");
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}