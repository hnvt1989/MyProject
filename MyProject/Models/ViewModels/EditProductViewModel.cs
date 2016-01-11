using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class EditProductViewModel
    {
        public EditProductViewModel ()
        {
            this.Offers = new List<ProductOffer>();
            this.Categories = new List<CategoryViewModel>();
        }
        public ProductViewModel ProductView { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public List<PriceType> PriceTypes { get; set; }

        public List<ProductOffer> Offers { get; set; } 
    }
}