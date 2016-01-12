using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Categories = new List<Category>();
        }
        public int Id { get; set; }

        [Display(Name = "Product Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Enter description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Enter detail description")]
        public string DetailDescription { get; set; }

        public decimal Price { get; set; }

        public string PriceType { get; set; }

        public decimal ShippingCost { get; set; }

        [Display(Name = "Quantity on hand")]
        public int QuantityOnHand { get; set; }

        public bool FeatureProduct { get; set; }

        [Display(Name = "Pound")]
        public decimal WeightPounds { get; set; }

        [Display(Name = "Ounce")]
        public decimal WeightOunce { get; set; }

        public byte[] Image { get; set; }

        public HttpPostedFileBase ProductImage { get; set; }

        public List<Category> Categories { get; set; }
    }
}