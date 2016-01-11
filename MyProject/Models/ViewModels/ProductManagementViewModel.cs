using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class ProductManagementViewModel
    {
        public ProductManagementViewModel()
        {
            Categories = new List<CategoryViewModel>();
            Products = new List<ProductViewModel>();
        }
        public List<CategoryViewModel> Categories;
        public List<ProductViewModel> Products;

        public string SearchProductId { get; set; }
    }
}