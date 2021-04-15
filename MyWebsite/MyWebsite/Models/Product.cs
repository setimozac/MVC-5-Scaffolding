using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebsite.Models
{
    public class Product
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
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}