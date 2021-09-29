using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeDevelop.Models;
using static WeDevelop.Controllers.AdminController;

namespace WeDevelop.ViewModels
{
    public class AdminViewModel
    {
        public int NrPerdoruesve { get; set; }
        public int NrMarreveshjeve { get; set; }
        public int NrSherbimeve { get; set; }
        public int NrKerkesave { get; set; }
        public List<SherbimetMeTeKerkuara> Sherbimet = new List<SherbimetMeTeKerkuara>();
    }
}