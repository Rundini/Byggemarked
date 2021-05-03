using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByggemarkedEFClassLibrary;

namespace ByggemarkedKunde.Controllers
{

    public class KundersController : Controller
    {
        private ByggemarkedEntities db = new ByggemarkedEntities();


        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        public ActionResult Index()
        {
            return View(db.Kunder.ToList());
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Navigations metoder
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        public ActionResult GoToOpretBooking(int id)
        {
            return RedirectToAction("Create", "Bookingers", new { id = id });
        }

        public ActionResult Exit()
        {
            return RedirectToAction("Index", "Home");
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Details kunde
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Kunders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunder kunder = db.Kunder.Find(id);
            if (kunder == null)
            {
                return HttpNotFound();
            }

            return View(kunder);
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Create kunde
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Kunders/Create
        public ActionResult Create(string email, string password)
        {

            Kunder kunde = new Kunder();
            kunde.Email = email;
            kunde.Password = password;
            if (db.Kunder.Find(kunde.Email) == null)
            {
                return View(kunde);
            }
            else
            {
                return Exit();
            }
        }

        // POST: Kunders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KundeId,Navn,Adresse,Email,Password")] Kunder kunder)
        {
            if (ModelState.IsValid)
            {
                db.Kunder.Add(kunder);
                db.SaveChanges();
                return ReturnToDetails(kunder.KundeId);
            }

            return View(kunder);
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Edit kunde
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Kunders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunder kunder = db.Kunder.Find(id);
            if (kunder == null)
            {
                return HttpNotFound();
            }
            return View(kunder);
        }

        // POST: Kunders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KundeId,Navn,Adresse,Email,Password")] Kunder kunder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kunder).State = EntityState.Modified;
                db.SaveChanges();
                return ReturnToDetails(kunder.KundeId);
            }
            return View();
        }
        public ActionResult ReturnToDetails(int id)
        {

            return RedirectToAction("Details", "Kunders", new { id = id });
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Delete kunde
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Kunders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunder kunder = db.Kunder.Find(id);
            if (kunder == null)
            {
                return HttpNotFound();
            }
            return View(kunder);
        }

        // POST: Kunders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kunder kunder = db.Kunder.Find(id);
            db.Kunder.Remove(kunder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Dispose
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

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
