using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
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

        public string SelectedCategory { get; set; }

        [DisplayName("Sort by Category")]
        public SelectList CategoryList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var cat = new List<Category>();
                    cat.Add(new Category(){ Code = "All", Description = "All Categories"});
                    cat.AddRange(context.Categories.ToList());
                    return new SelectList(cat, "Code", "Description");
                };

            }
        }
    }
}