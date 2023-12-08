using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using James_Thew.context;

namespace James_Thew.Controllers
{
    public class FAQsController : Controller
    {
        private James_ThewEntities1 db = new James_ThewEntities1();

        // GET: FAQs
        public ActionResult Index()
        {
            return View(db.FAQs.ToList());
        }

        // GET: FAQs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQs fAQs = db.FAQs.Find(id);
            if (fAQs == null)
            {
                return HttpNotFound();
            }
            return View(fAQs);
        }

        // GET: FAQs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,Answer")] FAQs fAQs)
        {
            if (ModelState.IsValid)
            {
                db.FAQs.Add(fAQs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fAQs);
        }

        // GET: FAQs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQs fAQs = db.FAQs.Find(id);
            if (fAQs == null)
            {
                return HttpNotFound();
            }
            return View(fAQs);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,Answer")] FAQs fAQs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fAQs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fAQs);
        }

        // GET: FAQs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQs fAQs = db.FAQs.Find(id);
            if (fAQs == null)
            {
                return HttpNotFound();
            }
            return View(fAQs);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FAQs fAQs = db.FAQs.Find(id);
            db.FAQs.Remove(fAQs);
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
