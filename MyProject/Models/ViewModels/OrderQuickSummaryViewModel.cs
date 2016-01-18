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

        [DisplayName("Mã số")]
        public string Code { get; set; }

        [DisplayName("Tên khách hàng")]
        public string CustomerName { get; set; }

        [DisplayName("Ngày đặt hàng")]
        public string OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal PostedAmount { get; set; }

        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
    }
}