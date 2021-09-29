using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeDevelop.Models
{
    [Table("Karta")]
    public class Karta
    {
        [Key]
        public int Id { get; set; }
        public int sherbimId { get; set; }
        public string userId { get; set; }
        public virtual Sherbime Sherbime { get; set; }
    }
}

