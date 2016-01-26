using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class LineOrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineOrderDetailId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        
        public int Promotion { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Net { get; set; }
        public decimal TotalDiscount { get; set; }

        public decimal Profit { get; set; }

        //total amount after discount
        public decimal Total { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}