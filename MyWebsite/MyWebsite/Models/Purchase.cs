using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyWebsite.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PurchaseId { get; set; }

        
        public ApplicationUser User { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        [Display(Name ="purchased On")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name ="Purchase Method")]
        public Payments Payment { get; set; }

        //[Required]
        //[Range(1,10)]
        //public int Quantity { get; set; }
    }
}

