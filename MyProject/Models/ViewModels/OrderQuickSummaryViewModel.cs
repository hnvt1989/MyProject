using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class OrderQuickSummaryViewModel
    {
        public int Id { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Order Date")]
        public string OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal PostedAmount { get; set; }

        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
    }
}