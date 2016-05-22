using MyProject.Models.ViewModels.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class OrderConfirmViewModel : ResourceModel
    {

    private string _resourceContext = "OrderConfirm";
    private string _resourceSet = "SalesPortal";

    public OrderConfirmViewModel()
    {
        ResourceContext = _resourceContext;
        base.InitializeResources();

    }
    public CheckoutViewModel CheckOutInfo { get; set; }

        public ShoppingCartViewModel CartViewModel { get; set; }

        public string OrderGuid { get; set; }
    }
}