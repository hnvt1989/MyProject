using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.Models.ShoppingCart;
using Address = MyProject.Models.ShoppingCart.Address;

namespace MyProject.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public decimal CartTotal { get; set; }

        public decimal PaymentAmount { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public PaymentTransaction PaymentTransaction { get; set; }
    }
}