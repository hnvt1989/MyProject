using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using WebGrease.Css.Extensions;

namespace MyProject.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.Info = new HomePageInfo();
            this.BestSellingProductsThisMonths = new List<ProductViewModel>();
            this.NewProducts = new List<ProductViewModel>();
        }

        [DisplayName("Danh muc sản phẩm")]
        public SelectList CategoryList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {

                    var cat = new List<Category>();
                    cat.Add(new Category()
                    {
                        Id = 0,
                        Code = "All",
                        Description = "Tất cả"
                    });
                    cat.AddRange(context.Categories.Where(c => c.Active).ToList());
                    return new SelectList(cat, "Code", "Description", "All");
                };

            }
        }

        public List<CategoryViewModel> CategoryViews
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var ret = new List<CategoryViewModel>();
                    context.Categories.Where(c => c.Active).ForEach( c => ret.Add(new CategoryViewModel()
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Description = c.Description,
                        Icon = c.Icon,
                        ParentId = c.ParentId
                    }));

                    return ret;
                };
            }
        } 

        public string SelectedCategory { get; set; }

        //list of feature products
        public List<MyProject.Models.ViewModels.ProductViewModel> NewProducts { get; set; }

        public List<MyProject.Models.ViewModels.ProductViewModel> BestSellingProductsThisMonths { get; set; }

        public HeaderAdvertisementViewModel Advertisement { get; set; }

        public HomePageInfo Info { get; set; }

    }

    public class HeaderAdvertisementViewModel
    {
        public HeaderAdvertisementViewModel()
        {
            this.Ads = new List<HeaderAd>();
        }
        public List<HeaderAd> Ads { get; set; } 
    }

    public class HeaderAd
    {
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public int DisplayOrder { get; set; }
        public string AdText { get; set; }
        public string AdTextStyle { get; set; }
    }

    public class HomePageInfo
    {
        public string ContactInfo { get;set; }
        public string Annoucment { get; set; }
    }

    public class PurchaseInstruction
    {
        public string Header { get; set; }
        public string Content { get; set; }
    }
}