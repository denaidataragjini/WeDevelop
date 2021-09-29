using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WeDevelop.Models;
using WeDevelop.ViewModels;

namespace WeDevelop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        public class SherbimetMeTeKerkuara
        {
            public Sherbime Sherbimi { get; set; }
            public int Count { get; set; }
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult IndexAdmin()
        {
            int nr = db.Kerkesat.Where(x => x.Seen == false).Count();
            if (nr > 0)
            {
                Session["seen"] = nr;
            }
            else
            {
                Session["seen"] = null;
            }

            var renditKerkesat = db.Kerkesat.Where(x => x.Pranuar == true).GroupBy(x => x.Id_Sherbimi).ToList();
            List<SherbimetMeTeKerkuara> sherbimet = new List<SherbimetMeTeKerkuara>();
            foreach (var kerkese in renditKerkesat)
            {
                Sherbime sherbimi = db.Sherbimet.Where(x => x.Id == kerkese.Key).First();
                int count = db.Kerkesat.Where(x => x.Id_Sherbimi == kerkese.Key).Count();
                sherbimet.Add(new SherbimetMeTeKerkuara()
                {
                    Sherbimi = sherbimi,
                    Count = count
                });
            }
            List<SherbimetMeTeKerkuara> sherbimetRendit = sherbimet.OrderBy(x => x.Count).ToList();
            AdminViewModel IndexVM = new AdminViewModel()
            {
                NrPerdoruesve = db.Users.Count(),
                NrMarreveshjeve = db.Kerkesat.Where(x => x.Pranuar == true).Count(),
                NrSherbimeve = db.Sherbimet.Count(),
                NrKerkesave = db.Kerkesat.Where(x => x.Pranuar == false).Count(),
                Sherbimet= sherbimet.OrderByDescending(x => x.Count).ToList()
            };
            return View(IndexVM);
        }
        public AdminController()
        {
        }
        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                UserManager = value;
            }
        }
        
        public async Task<ActionResult> Perdorues()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        public async Task<ActionResult> DetajetPerdorues(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            var kerkesa = db.Kerkesat.Where(x => x.Id_Perdoruesi == user.Id).ToList();
            ViewBag.Kerkesat = kerkesa.Where(x => x.Pranuar == false).ToList();
            ViewBag.Marrevjeshet = kerkesa.Where(x => x.Pranuar == true);
            return View(user);
        }
      

        public JsonResult KonfirmoKerkese(int? id)
        {
            if (id == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kerkesa = db.Kerkesat.Find(id);
                kerkesa.Pranuar = true;
                kerkesa.DataEFillimit = DateTime.Now;
                kerkesa.Progresi = "0";
                var result = db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Kerkesat()
        {
            var list = db.Kerkesat.Where(x => x.Seen == false).ToList();
            foreach (var kerkese in list)
            {
                db.Kerkesat.Where(x => x.Id == kerkese.Id).First().Seen = true;
                db.SaveChanges();
            }
            Session["seen"] = null;
            ViewBag.Users = new SelectList(db.Users, "Id", "UserName");
            ViewBag.Kategorite = new SelectList(db.Kategorite, "Id", "EmerKat");
            return View(db.Kerkesat.Where(x => x.Pranuar == false).ToList());
        }




        [HttpGet]
        public JsonResult KerkoKerkese(string userId)
        {
            var kerkesat = db.Kerkesat.Include(x => x.Sherbimet).Include(x => x.User).Where(x => x.Id_Perdoruesi == userId && x.Pranuar == false).ToList();
            return Json(kerkesat, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Marreveshjet()
        {
            MarreveshjetViewModel vm = new MarreveshjetViewModel();
            vm.MarreveshjeNeProcesim = db.Kerkesat.Include("Sherbimet").Include("User").Where(x => x.Pranuar == true && x.Progresi != "100").ToList();
            vm.MarreveshjeTePerfunduara = db.Kerkesat.Include("Sherbimet").Include("User").Where(x => x.Pranuar == true && x.Progresi == "100").ToList();

            return View(vm);
        }
        [HttpGet]
        public JsonResult NdryshoProgresMarreveshje(int id, string progresi)
        {
            if (progresi.Contains("100"))
            {
                db.Kerkesat.Find(id).DataEMbarimit = DateTime.Now;
            }
            db.Kerkesat.Find(id).Progresi = progresi;

            var result = db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult FaqezimTabele(int? faqja)
        {
            if (faqja == null || faqja <= 1)
            {
                return Json(db.Kerkesat.Include("Sherbimet").Include("User").Where(x => x.Pranuar == true && x.Progresi.Contains("100")).Take(15).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int faqePara = ((int)faqja - 1) * 15;
                return Json(db.Kerkesat.Include("Sherbimet").Include("User").Where(x => x.Pranuar == true && x.Progresi.Contains("100")).OrderBy(x => x.Id).Skip(faqePara).Take(15).ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult KerkoKerkeseKategori(int kategoriId)
        {
            var kerkesat = db.Kerkesat.Include(x => x.Sherbimet).Include(x => x.User).Where(x => x.Sherbimet.KategoriId == kategoriId && x.Pranuar == false).ToList();
            return Json(kerkesat, JsonRequestBehavior.AllowGet);
        }


        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) && allowedFileTypes.Contains(fileExtension))
            {
                return true;

            }
            return false;
        }

        private void RuajFile(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Konstante.ImazhPath + file.FileName);
        }

        public ActionResult ShtoSherbime()
        {
            ViewBag.KategoriId = new SelectList(db.Kategorite, "Id", "EmerKat");

            return View();
        }
        [HttpPost]
        public ActionResult ShtoSherbime(Sherbime sherbime, HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (ValidateFile(file))
                {
                    try
                    {
                        RuajFile(file);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("EmriiImazhit", "Nuk u ruajt file ne disk, provo perseri");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmriiImazhit", "File duhet te jete imazh dhe me pak se 2MB");
                }
            }
            else
            {
                ModelState.AddModelError("EmriiImazhit", "Zgjidhni nje file");
            }
            if (ModelState.IsValid)
            {
                db.Sherbimet.Add(
                    new Sherbime 
                    {
                        Imazh=file.FileName,
                        Emri= sherbime.Emri,
                        Pershkrimi= sherbime.Pershkrimi,
                        Cmimi=sherbime.Cmimi,
                         Ditet= sherbime.Ditet,
                         CfareJapim =sherbime.CfareJapim,
                         CfareKerkojme =sherbime.CfareKerkojme,
                         KategoriId = sherbime.KategoriId

                    });
                db.SaveChanges();

                return RedirectToAction("Sherbime", "Home");
            }
             
            return View();
        }
        public ActionResult ModifikoSherbime(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sherbime sherbimi = db.Sherbimet.Find(id);
            if (sherbimi == null)
            {
                return HttpNotFound();
            }
            return View(sherbimi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifikoSherbime([Bind(Include = "Id, Emri, Cmimi,Ditet, Pershkrimi, CfareJapim, CfareKerkojme")] Sherbime sherbimi)
        {
            if (ModelState.IsValid)
            {
                db.Sherbimet.Find(sherbimi.Id).Emri=sherbimi.Emri;
                db.Sherbimet.Find(sherbimi.Id).Cmimi = sherbimi.Cmimi;
                db.Sherbimet.Find(sherbimi.Id).Ditet = sherbimi.Ditet;
                db.Sherbimet.Find(sherbimi.Id).Pershkrimi = sherbimi.Pershkrimi;
                db.Sherbimet.Find(sherbimi.Id).CfareJapim = sherbimi.CfareJapim;
                db.Sherbimet.Find(sherbimi.Id).CfareKerkojme = sherbimi.CfareKerkojme;
              
                db.SaveChanges();
                
            }
            return RedirectToAction("DetajeSherbimesh", "Home", new { id = sherbimi.Id });
        }
        public ActionResult FshiSherbime(int? id)
        {
            var sherbim = db.Sherbimet.Find(id);
            var kartat = db.Kartat.Where(x => x.Id == id).ToList();
            if (kartat != null)
            {
                ViewBag.Message = "Per kete sherbim ka kliente te interesuar.";
                return View(sherbim);
            }
            ViewBag.Message = "";
            return View(sherbim);
        }
        [HttpPost]
        public ActionResult FshiSherbime(int id)
        {
            var sherbim = db.Sherbimet.Find(id);
            var kartat = db.Kartat.Where(x => x.sherbimId == id).ToList();
            foreach (var kart in kartat)
            {
                db.Kartat.Remove(kart);
                db.SaveChanges();
            }
            db.Sherbimet.Remove(sherbim);
            db.SaveChanges();
            return RedirectToAction("../Home/Sherbime");
        }
        
        public async Task<ActionResult> ModifikoPerdorues(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> ModifikoPerdorues(ApplicationUser perdoruesRi)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(perdoruesRi.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.UserName = perdoruesRi.UserName;
                user.PhoneNumber = perdoruesRi.PhoneNumber;

                var delete = await UserManager.RemoveFromRoleAsync(user.Id, "Users");
                if (!delete.Succeeded)
                {
                    ModelState.AddModelError("", delete.Errors.First());
                    return View(user);
                }
                if (perdoruesRi.UserName.Contains("admin"))
                {
                    var resultAdmin = await UserManager.AddToRoleAsync(user.Id, "Admin");
                    if (!resultAdmin.Succeeded)
                    {
                        ModelState.AddModelError("", resultAdmin.Errors.First());
                        return View(user);
                    }
                }
                else
                {
                    var result = await UserManager.AddToRoleAsync(user.Id, "Users");
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View(perdoruesRi);
                    }
                }

            }
            ModelState.AddModelError("", "Nuk u krye modifikimi!");
            return RedirectToAction("DetajetPerdorues", "Admin", new { id = perdoruesRi.Id });
        }

        [HttpGet]
        public async Task<JsonResult> KerkoPerdorues(string query)
        {
            var users = await UserManager.Users.ToListAsync();
            
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}