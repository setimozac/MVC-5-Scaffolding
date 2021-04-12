using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyWebsite.Models
{
    public class Shipping
    {
        [Key]
        public int ShippingId { get; set; }

        [Display(Name ="Shipped On")]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string address { get; set; }

        
        public Purchase Purchase { get; set; }
    }
}