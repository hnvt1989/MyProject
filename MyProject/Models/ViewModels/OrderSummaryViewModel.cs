using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class OrderSummaryViewModel
    {
        public long OrderNumber { get; set; }

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