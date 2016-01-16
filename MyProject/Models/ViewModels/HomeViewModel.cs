using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;

namespace MyProject.Models.ViewModels
{
    public class HomeViewModel
    {
        [DisplayName("Danh muc sản phẩm")]
        public SelectList CategoryList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var cat = context.Categories.ToList();
                    return new SelectList(cat, "Code", "Description");
                };

            }
        }

        public string SelectedCategory { get; set; }
        public List<MyProject.Models.ViewModels.ProductViewModel> ProductViewModels { get; set; }

        public HeaderAdvertisementViewModel Advertisement { get; set; }
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
    }

}