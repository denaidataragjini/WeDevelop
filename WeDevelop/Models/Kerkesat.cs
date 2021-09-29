using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeDevelop.Models
{
    [Table("Kerkesat")]
    public class Kerkesat
    {
        [Key]
        public int Id { get; set; }
       
        public bool Pranuar { get; set; }
        public string Progresi { get; set; }
        
        [Display(Name ="Data e fillimit")]
        public DateTime DataEFillimit { get; set; }

        [Display(Name = "Data e mbarimit")]
        public DateTime DataEMbarimit { get; set; }

        public int Id_Sherbimi { get; set; }
        [ForeignKey("Id_Sherbimi")]
        public virtual Sherbime Sherbimet { get; set; }

        [MaxLength(1000)]
        public string Pershkrimi { get; set; }
        public string Dokumenti { get; set; }
        public bool Seen { get; set; }
       
        public string Id_Perdoruesi { get; set; }
        [ForeignKey("Id_Perdoruesi")]
        public virtual ApplicationUser User { get; set; }
    }



}


