using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebsite.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public Product product { get; set; }

        public byte[] Image { get; set; }

        public Boolean IsMain { get; set; }
    }
}