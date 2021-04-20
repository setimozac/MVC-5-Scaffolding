using MyWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebsite.ViewModel
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        
        public Purchase Purchase { get; set; }

        public ApplicationUser User { get; set; }
        
        [Required]
        [Display(Name = "purchased On")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Purchase Method")]
        public Payments Payment { get; set; }

        public Shipping Shipping { get; set; }

        [Display(Name = "Shipment Date")]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string address { get; set; }


        //product information
        public int ProductId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

    }
}