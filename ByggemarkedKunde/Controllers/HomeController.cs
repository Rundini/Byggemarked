using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ByggemarkedEFClassLibrary;

namespace ByggemarkedKunde.Controllers
{
    public class HomeController : Controller
    {

        private ByggemarkedEntities db = new ByggemarkedEntities();

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        public class Parameters
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        //          Login metoder til Home/Index
        //
        //  Sendes til Kunders/Details/x ved genkendt bruger (email og password),
        //  ellers sendes man videre til Kunders/Create til opretning af en ny
        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        public ActionResult Index()
        {
            Parameters p = new Parameters { Email = null, Password = null };
            return View(p);
        }

        [HttpPost]
        public ActionResult Index(Parameters p)
        {

            foreach (Kunder k in db.Kunder)
            {
                if (p.Email == k.Email && p.Password == k.Password)
                {
                    return RedirectToAction("Details", "Kunders", new { id = k.KundeId });
                }

            }

            return RedirectToAction("Create", "Kunders", new { email = p.Email, password = p.Password });
        }
    }
}