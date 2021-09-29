using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WeDevelop.Models;

namespace WeDevelop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //faqja e perdoruesit
        int faqjaAktual;
     
        public ActionResult Dashboard(int? nrFaqes)
        {
            if (Request.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                var useri = db.Users.Where(x => x.Id == userId).FirstOrDefault();
           
                ViewBag.Email = useri.Email;
                ViewBag.NumriTel = useri.PhoneNumber;

                var emri = User.Identity.GetUserName();
                var nr = emri.IndexOf("@");
                emri = emri.Substring(0, nr);
                ViewBag.Emri = emri;

                var kerkesat = db.Kerkesat.Where(x => x.User.Id == userId).ToList();
                if (nrFaqes == null || nrFaqes <= 1)
                {
                    faqjaAktual = 1;
                    ViewBag.FaqjaAktuale = faqjaAktual;
                    return View(kerkesat.Take(6).ToList());
                }
                else
                {
                    faqjaAktual = (int)nrFaqes;
                    int faqja = ((int)nrFaqes - 1) * 6;
                    ViewBag.FaqjaAktuale = faqjaAktual;
                    return View(kerkesat.OrderBy(x => x.Id).Skip(faqja).Take(6).ToList());
                }

            }
            else
                return RedirectToAction("Login", "Account");
        }
       
        //Blerja e sherbimit Get
      
        public ActionResult BliSherbimin( int? id)
        { int idsh = (int)id;
            string userId;

            if (Request.IsAuthenticated)
            {    
                    userId = User.Identity.GetUserId();
                    var username = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                    ViewBag.Username = username.UserName;

                    var sherbime = db.Sherbimet.Where(x => x.Id == idsh).FirstOrDefault();
                    ViewBag.EmriSherbimit = sherbime.Emri;
                    ViewBag.Cmimi = sherbime.Cmimi;
                    ViewBag.Imazhi = sherbime.Imazh;
            
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        //Blerja e sherbimit Post
        [HttpPost]
        public ActionResult BliSherbimin(int id, Kerkesat kerkesat,HttpPostedFileBase file)
        {
            string fileNam;

                   if(file.ContentLength > 0)
                   {
                       
                    fileNam = Path.GetFileName(file.FileName);

                string _path = Path.Combine(Server.MapPath("~/Content/Dokumentat/"), fileNam);
                file.SaveAs(_path);
                ViewBag.Message = "File Uploaded Successfully!!";

                   }
                   else
                   {
                fileNam = null;
                     }

                if (ModelState.IsValid)
                {
                 db.Kerkesat.Add(
                    new Kerkesat
                    {
                        Dokumenti =fileNam,
                        Id_Perdoruesi =User.Identity.GetUserId(),
                        Id_Sherbimi =id,
                        Pershkrimi =kerkesat.Pershkrimi,
                        DataEFillimit = DateTime.Now,
                        DataEMbarimit = DateTime.Now,
                        Seen=false,

                    });
                db.SaveChanges();

                return RedirectToAction("Sherbime", "Home");
                }
            return View();
            
        }
        //paging Sherbime
        int faqjaAktuale;
        [AllowAnonymous]
        public ActionResult Sherbime(int? nrFaqes)
        {
            if (nrFaqes == null || nrFaqes <= 1)
            {
                faqjaAktuale = 1;
                ViewBag.FaqjaAktuale = faqjaAktuale;
                return View(db.Sherbimet.Take(8).ToList());
            }
            else
            {
                faqjaAktuale = (int)nrFaqes;
                int faqja = ((int)nrFaqes - 1) * 8;
                ViewBag.FaqjaAktuale = faqjaAktuale;
                return View(db.Sherbimet.OrderBy(x => x.Id).Skip(faqja).Take(8).ToList());
            }

        }
        //detajet e sherbimeve
        [AllowAnonymous]
        public ActionResult DetajeSherbimesh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Sherbime sherbime = db.Sherbimet.Find(id);
            var KatId = sherbime.KategoriId;
            var TeSugjeruarat = db.Sherbimet.Where(x => x.KategoriId == KatId);
            ViewBag.Sugjerimet = TeSugjeruarat.ToList();
            if (sherbime == null)
            {
                return HttpNotFound();
            }
            return View(sherbime);
        }

        //json i searchit
        [AllowAnonymous]
        public JsonResult Search(string search)
        {
            List<Sherbime> sherbime = db.Sherbimet.Where(x => x.Emri.Contains(search)).ToList();
            return Json(sherbime, JsonRequestBehavior.AllowGet);
        }

        public FileResult Fatura(int? id)
        {
            if (id != null)
            {
               
            
                MemoryStream workStream = new MemoryStream();
                Document doc = new Document(PageSize.A4);
                string strPDFFileName = "Fatura-" + User.Identity.GetUserName() + ".pdf";
                PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Content/Faturat/" + strPDFFileName), FileMode.Create));
                doc.Open();
                PdfPTable tableLayout = new PdfPTable(2);

                doc.Add(Add_Content_To_PDF(tableLayout,(int) id));

                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return File(workStream, "application/pdf", strPDFFileName);
            }
            return null;
        }
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int id)
        {

            float[] headers = { 25, 25 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 50; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            List<Sherbime> sherbimet = db.Sherbimet.ToList();

            tableLayout.AddCell(new PdfPCell(new Phrase("Data:" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year, new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Klienti: " + User.Identity.GetUserName(), new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });

            Sherbime sherbim = db.Sherbimet.Include("Kategorite").Where(x=> x.Id==id).First();

            AddCellToBody(tableLayout, "Emri:");
            AddCellToBody(tableLayout, sherbim.Emri);
            AddCellToBody(tableLayout, "Kohezgjatja: ");
            AddCellToBody(tableLayout, sherbim.Ditet.ToString() + " dite");
          
            AddCellToBody(tableLayout, "Cmimi:");
            AddCellToBody(tableLayout, sherbim.Cmimi.ToString());

            tableLayout.AddCell(new PdfPCell(new Phrase("WeDevelop", new Font(Font.FontFamily.TIMES_ROMAN, 15, 1, new iTextSharp.text.BaseColor(0, 0, 10))))
            {
                Colspan = 12,
                Border = 0,
                PaddingTop = 50,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            return tableLayout;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

        //indexi
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                var karta = db.Kartat.Where(x => x.userId == userId).ToList();
                Session["count"] = karta.Count();
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
           
            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}