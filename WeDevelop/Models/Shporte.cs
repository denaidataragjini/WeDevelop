using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeDevelop.Models
{
    [Table("Shportat")]
    public class Shporte
    {
        [Key]
        public int Id { get; set; }
        public int Id_Sherbimi { get; set; }
        public string Id_Klienti { get; set; }
        public virtual Sherbime Sherbimet { get; set; }
    }

}