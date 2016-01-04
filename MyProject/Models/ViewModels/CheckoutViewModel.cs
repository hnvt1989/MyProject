using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.Models.ShoppingCart;
using Address = MyProject.Models.ShoppingCart.Address;

namespace MyProject.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [DisplayName("Full Name")]
        public string Name { get; set; }

        public string CartCode { get; set; }

        public decimal CartTotal { get; set; }

        public decimal PaymentAmount { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public PaymentTransaction PaymentTransaction { get; set; }

        public List<PaymentType> PaymentTypes { get; set; } 

        public SelectList PaymentTypesList { get; set; }
    }
}