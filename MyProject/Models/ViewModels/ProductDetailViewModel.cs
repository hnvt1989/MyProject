using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels.ContentManagement;

namespace MyProject.Models.ViewModels
{
    public class ProductDetailViewModel : ResourceModel
    {
        private string _resourceContext = "StoreProductDetails";
        private string _resourceSet = "SalesPortal";

        public ProductDetailViewModel()
        {
            base.ResourceContext = _resourceContext;
            base.InitializeResources();

        }
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public string ReviewVideoUrl { get; set; }

        public decimal ShippingCost { get; set; }

        [Display(Name = "Quantity on hand")]
        public int QuantityOnHand { get; set; }

        public byte[] Image { get; set; }

        public byte[] ImageAlt0 { get; set; }

        public byte[] ImageAlt1 { get; set; }

    }
}