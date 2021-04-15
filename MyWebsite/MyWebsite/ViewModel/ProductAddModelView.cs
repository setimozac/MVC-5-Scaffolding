using MyWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebsite.ViewModel
{
    public class ProductAddModelView
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public byte[] ImageDb { get; set; }

        public List<byte[]> ImagesDb { get; set; }

        public List<ProductImage> PImagesDb { get; set; }

        public HttpPostedFileBase Image0 { get; set; }
        public HttpPostedFileBase Image1 { get; set; }
        public HttpPostedFileBase Image2 { get; set; }
        public List<HttpPostedFileBase> Images { get; set; }

        public Boolean IsMain { get; set; }

        
    }
}