using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeDevelop.Models
{
    [Table("Sherbimet")]
    public class Sherbime
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Emri i sherbimit")]
        public string Emri { get; set; }
        [Display(Name = "Pershkrimi i sherbimit")]
        [MaxLength(400, ErrorMessage = "Pershkrimi nuk lejohet me shume se 400 karaktere")]
        public string Pershkrimi { get; set; }
        [Display(Name = "Foto")]
        public string Imazh { get; set; }
        [Display(Name = "Cmimi i sherbimit")]
        [DataType(DataType.Currency)]
        public decimal Cmimi { get; set; }
        [Display(Name ="Kohezgjatja")]
        public int Ditet { get; set; }
        [Display(Name = "Cfare ne kerkojme nga ju")]
        public string CfareKerkojme { get; set; }
        [Display(Name = "Cfare ju merrni nga ne")]
        public string CfareJapim { get; set; }
        public int KategoriId { get; set; }
        public virtual List<Kategoria> Kategorite { get; set; }
    }

}