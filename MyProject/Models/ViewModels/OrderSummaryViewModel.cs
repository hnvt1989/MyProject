using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels.ContentManagement;

namespace MyProject.Models.ViewModels
{
    public class OrderSummaryViewModel : ResourceModel
    {

        private string _resourceContext = "OrderSummary";
        private string _resourceSet = "SalesPortal";

        public OrderSummaryViewModel()
        {
            ResourceContext = _resourceContext;
            base.InitializeResources();

        }
        public long OrderNumber { get; set; }
        public bool FirstTime { get; set; }
        public string FullName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public decimal Total { get; set; }
        public decimal ShippingCost { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<LineOrderDetail> OrderDetails { get; set; }

        public Address ShippingAddress { get; set; }
        public PaymentTransaction PaymentTransaction { get; set; }
        
        public string PaymentInfo { get; set; }
    }
}