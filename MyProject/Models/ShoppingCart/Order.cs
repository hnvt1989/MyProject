using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long OrderNumber { get; set; }

        public string Guid { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }

        [ForeignKey("Address")]
        public int ShippingAddressId { get; set; }

        [ForeignKey("PaymentTransaction")]
        public int PaymentTransactionId { get; set; }

        //[ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public decimal Total { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public decimal PostedAmount { get; set; }//deposit
        public decimal Profit { get; set; }
        public string Notes { get; set; }
        //public decimal ActualSoldAmount { get; set; } //the amount we actually sold.

        public System.DateTime OrderDate { get; set; }
        public List<LineOrderDetail> OrderDetails { get; set; }

        public virtual Address Address { get; set; }
        public virtual PaymentTransaction PaymentTransaction { get; set; }
        //public virtual OrderStatus OrderStatus { get; set; }
    }

    public class OrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }


    }
}