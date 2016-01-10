using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class ProductViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Enter Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Enter description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Enter detail description")]
        public string DetailDescription { get; set; }

        public decimal Price { get; set; }

        public string PriceType { get; set; }

        public int QuantityOnHand { get; set; }

        public bool FeatureProduct { get; set; }

        public decimal Weight { get; set; }

        public byte[] Image { get; set; }

        public HttpPostedFileBase ProductImage { get; set; }
    }
}