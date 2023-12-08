using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using James_Thew.context;

namespace James_Thew.Controllers
{
    public class Racipies_jamesController : Controller
    {
        private James_ThewEntities1 db = new James_ThewEntities1();

        // GET: Racipies_james
        public ActionResult Index()
        {
            return View(db.Racipies_james.ToList());
        }

        // GET: Racipies_james/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racipies_james racipies_james = db.Racipies_james.Find(id);
            if (racipies_james == null)
            {
                return HttpNotFound();
            }
            return View(racipies_james);
        }

        // GET: Racipies_james/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Racipies_james/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Ingredient,CookingProcedure,IsFree,Image")] Racipies_james racipies_james, HttpPostedFileBase ImageFile)
        {
           
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                string imagePath = Path.Combine(Server.MapPath("~/All_Image/"), filename);
                ImageFile.SaveAs(imagePath);
                racipies_james.Image = "~/All_Image/" + filename;
                db.Racipies_james.Add(racipies_james);
                db.SaveChanges();
                return RedirectToAction("Index","Admin");
            }

            return View(racipies_james);
        }

        // GET: Racipies_james/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racipies_james racipies_james = db.Racipies_james.Find(id);
            if (racipies_james == null)
            {
                return HttpNotFound();
            }
            return View(racipies_james);
        }

        // POST: Racipies_james/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Ingredient,CookingProcedure,IsFree,Image")] Racipies_james racipies_james, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // A new image file is uploaded, handle it
                    string filename = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    string imagePath = Path.Combine(Server.MapPath("~/All_Image/"), filename);
                    ImageFile.SaveAs(imagePath);
                    racipies_james.Image = "~/All_Image/" + filename;
                }
                else
                {
                    // No new image file is uploaded, retain the existing image
                    Racipies_james existingRecipe = db.Racipies_james.Find(racipies_james.Id);
                    if (existingRecipe != null)
                    {
                        racipies_james.Image = existingRecipe.Image;
                    }
                }

                db.Entry(racipies_james).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(racipies_james);
        }


        // GET: Racipies_james/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racipies_james racipies_james = db.Racipies_james.Find(id);
            if (racipies_james == null)
            {
                return HttpNotFound();
            }
            return View(racipies_james);
        }

        // POST: Racipies_james/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Racipies_james racipies_james = db.Racipies_james.Find(id);
            db.Racipies_james.Remove(racipies_james);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
