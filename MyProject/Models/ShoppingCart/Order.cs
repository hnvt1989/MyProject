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

        public string UserName { get; set; }
        public string FullName { get; set; }

        [ForeignKey("Address")]
        public int ShippingAddressId { get; set; }

        [ForeignKey("PaymentTransaction")]
        public int PaymentTransactionId { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<LineOrderDetail> OrderDetails { get; set; }

        public virtual Address Address { get; set; }
        public virtual PaymentTransaction PaymentTransaction { get; set; }
    }
}