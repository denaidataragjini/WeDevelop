using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WeDevelop.Models
{
    [Table("Kategoria")]
    public class Kategoria
    { 
        [Key]
        public int Id { get; set; }
        public string EmerKat { get; set; }
    
    }
}