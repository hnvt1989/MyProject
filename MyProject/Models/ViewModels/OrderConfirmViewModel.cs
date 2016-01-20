using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class OrderConfirmViewModel
    {
        public CheckoutViewModel CheckOutInfo { get; set; }

        public ShoppingCartViewModel CartViewModel { get; set; }

        public string OrderGuid { get; set; }
    }
}