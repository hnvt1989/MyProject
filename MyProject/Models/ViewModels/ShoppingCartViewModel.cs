using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels.ContentManagement;

namespace MyProject.Models.ViewModels
{
    public class ShoppingCartViewModel : ResourceModel
    {
        private string _resourceContext = "ShoppingCart";
        private string _resourceSet = "SalesPortal";

        public ShoppingCartViewModel()
        {
            base.ResourceContext = _resourceContext;
            base.InitializeResources();

        }


        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal CartTotalShippingCost { get; set; }
    }
}