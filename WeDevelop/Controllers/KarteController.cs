using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeDevelop.Models;

namespace WeDevelop.Controllers
{
    public class KarteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Karte
        public ActionResult Index()
        {
            return View();
        }

        private string UserId()
        {
            if (Request.IsAuthenticated)
            {
                return User.Identity.GetUserId();
            }
            else
            {
                return null;
            }
        }

        int _count;
        public ActionResult ShtoKarte(int? id)
        {
            string userId=UserId();
            if (userId != null)
            {
                var karta = db.Kartat.Where(x => x.userId == userId).ToList();
                Session["count"] = karta.Count();

                Karta Kartaperdorues = db.Kartat.Where(k => k.userId == userId && k.sherbimId == id).FirstOrDefault();
                if (Kartaperdorues == null)
                {  
                    db.Kartat.Add( new Karta()
                    { userId = userId, sherbimId = (int)id });
                    db.SaveChanges();
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                else
                {
                    Session["Errori"] = "Sherbimi qe kerkoni gjendet ne listen e te preferuarave";
                   // int Id=(int)id;
                   //return RedirectToAction("DetajeSherbimesh", "Home", new { id= Id});
                }
                int Id = (int)id;
                return RedirectToAction("DetajeSherbimesh", "Home", new { id = Id });

                //return RedirectToAction("Sherbime", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
        public ActionResult Porosite()
        {
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                var kartat = db.Kartat.Where(x => x.userId == UserId).ToList();
                
                  
                    return View(kartat);
                
            }
            else
            {
                ViewBag.Error = "Ska info";
                return View();
            }
            
        }

        public ActionResult FshiKart(int ? id)
        {
            var Ekziston = db.Kartat.Where(x => x.sherbimId == id).FirstOrDefault(); ;
            
                db.Kartat.Remove(Ekziston);
                db.SaveChanges();
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("Porosite", "Karte");
            
        }
        
    }
}