using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class ShoppingCartAddViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public string CartTotalToString { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int AddId { get; set; }
        public decimal NetBeforeDiscount { get; set; }
        public decimal Sum { get; set; }
        public string SumToString { get; set; }
        public decimal TotalDiscount { get; set; }
        public  decimal ShippingCost { get; set; }
        public decimal TotalCartShippingCost { get; set; }
        public string TotalCartShippingCostToString { get; set; }
    }
}