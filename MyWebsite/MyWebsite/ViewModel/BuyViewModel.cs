using MyWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebsite.ViewModel
{
    public class BuyViewModel
    {
        public int ProductId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public double Price { get; set; }

        
        public int Quantity { get; set; }
        [Display(Name ="Quantity")]
        public int PurchaseQuantity { get; set; }

        public byte[] ImageDb { get; set; }

        public List<byte[]> ImagesDb { get; set; }

        public List<ProductImage> PImagesDb { get; set; }

        public HttpPostedFileBase Image0 { get; set; }
        public HttpPostedFileBase Image1 { get; set; }
        public HttpPostedFileBase Image2 { get; set; }
        public List<HttpPostedFileBase> Images { get; set; }

        public Boolean IsMain { get; set; }

        public string PurchaseId { get; set; }

        [Display(Name = "Purchase Method")]
        public Payments Payment { get; set; }

        [Required]
        public string address { get; set; }
    }
}