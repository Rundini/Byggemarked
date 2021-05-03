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
    public class BookingersController : Controller
    {
        private ByggemarkedEntities db = new ByggemarkedEntities();

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Bookingers
        public ActionResult Index()
        {
            var bookinger = db.Bookinger.Include(b => b.Kunder).Include(b => b.Vaerktoej);
            return View(bookinger.ToList());
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Details booking
        //  Her bruges et kundeID, til at finde en bestemt brugers detalje side.
        //  I dette program er det den helt centrale side, da der her kan tilgåes
        //  både ændringer i brugerinformationer, samt opretninger af nye bookninger.
        //  Derudover kan man se alle ens eksisterende bookninger
        // ---------------------------------------------------------------------------------------------------------------------------------------------------


        // GET: Bookingers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookinger bookinger = db.Bookinger.Find(id);
            if (bookinger == null)
            {
                return HttpNotFound();
            }
            return View(bookinger);
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Create booking
        //  Opretter nye bookninger, status på en booking sættes automatisk som "Reserveret",
        //  samtidig er bruger id'en ført med over, så man ikke kan vælge andre brugeres id'er.
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Bookingers/Create
        public ActionResult Create(int id)
        {
            Bookinger booking = new Bookinger();
            booking.KundeId = id;
            booking.Status = "Reserveret";
            booking.TotalPris = 100;

            ViewBag.VaerktoejId = new SelectList(db.Vaerktoej, "VaerktoejId", "Type");

            return View(booking);
        }

        // POST: Bookingers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,Status,TotalPris,PeriodeStart,PeriodeSlut,KundeId,VaerktoejId")] Bookinger bookinger)
        {


            Vaerktoej vk = db.Vaerktoej.Where(v => v.VaerktoejId == v.VaerktoejId).FirstOrDefault();

            if (bookinger.PeriodeStart != null && bookinger.PeriodeSlut != null)
            {
                bookinger.TotalPris = BeregnPris(vk.DoegnPris, bookinger.PeriodeStart, bookinger.PeriodeSlut);
            }

            if (ModelState.IsValid)
            {
                db.Bookinger.Add(bookinger);
                db.SaveChanges();
                return ReturnToDetailsFromOpret(bookinger.KundeId);
            }

            ViewBag.VaerktoejId = new SelectList(db.Vaerktoej, "VaerktoejId", "Type", bookinger.VaerktoejId);

            return View();
        }

        public decimal BeregnPris(decimal doegnPris, DateTime start, DateTime slut)
        {
            return (doegnPris * ((slut - start).Days));
        }

        public ActionResult ReturnToDetailsFromOpret(int id)
        {
            return RedirectToAction("Details", "Kunders", new { id = id });
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Edit booking
        //  Denne funktion vil ikke blive brugt!
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Bookingers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookinger bookinger = db.Bookinger.Find(id);
            if (bookinger == null)
            {
                return HttpNotFound();
            }
            ViewBag.KundeId = new SelectList(db.Kunder, "KundeId", "Navn", bookinger.KundeId);
            ViewBag.VaerktoejId = new SelectList(db.Vaerktoej, "VaerktoejId", "Type", bookinger.VaerktoejId);
            return View(bookinger);
        }

        // POST: Bookingers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,Status,TotalPris,PeriodeStart,PeriodeSlut,KundeId,VaerktoejId")] Bookinger bookinger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookinger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KundeId = new SelectList(db.Kunder, "KundeId", "Navn", bookinger.KundeId);
            ViewBag.VaerktoejId = new SelectList(db.Vaerktoej, "VaerktoejId", "Type", bookinger.VaerktoejId);
            return View(bookinger);
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Delete booking
        //  Denne funktion ikke blive brugt!
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        // GET: Bookingers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookinger bookinger = db.Bookinger.Find(id);
            if (bookinger == null)
            {
                return HttpNotFound();
            }
            return View(bookinger);
        }

        // POST: Bookingers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookinger bookinger = db.Bookinger.Find(id);
            db.Bookinger.Remove(bookinger);
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
