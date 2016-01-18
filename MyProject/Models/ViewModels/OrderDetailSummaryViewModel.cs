using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class OrderDetailSummaryViewModel
    {
        public OrderDetailSummaryViewModel()
        {
            this.OrderDetails = new List<LineOrderDetail>();
        }
        public long OrderNumber { get; set; }

        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }

        [AllowHtml]
        public string OrderStatusNote { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public decimal Total { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal PostedAmount { get; set; }
        //public decimal ActualSoldAmount { get; set; }


        public System.DateTime OrderDate { get; set; }
        public List<LineOrderDetail> OrderDetails { get; set; }

        public Address ShippingAddress { get; set; }
        public PaymentTransaction PaymentTransaction { get; set; }

        public SelectList PaymentTypesList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    return new SelectList(context.PaymentTypes.ToList().Select(c => c.Description), "Bank");
                };

            }
        }

        public SelectList PaymentStatusList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    return new SelectList(context.PaymentStatuses.ToList().Select(c => c.Description), "NotPaid");
                };

            }
        }

        [DisplayName("Chọn tình trạng đơn đặt hàng:")]
        public SelectList OrderStatusList
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    return new SelectList(context.OrderStatuses.ToList().Select(c => c.Description), "Processing");
                };

            }
        }

    }
}