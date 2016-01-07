using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace MyProject.Models.ShoppingCart
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        //stock price
        public decimal Price { get; set; }

        public bool FeatureProduct { get; set; }

        public decimal Weight { get; set; }

        public int QuantityOnHand { get; set; }

        public byte[] Image { get; set; }

        public ICollection<Category> Categories { get; set; } 
    }

    public class ProductOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        [ForeignKey("PriceType")]
        public int PriceTypeId { get; set; }

        public decimal Price { get; set; }

        public bool Discountable { get; set; }

        public virtual Product Product { get; set; }

        public virtual PriceType PriceType { get; set; }
    }

    public class PriceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CartLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string ProductCode { get; set; }

        public string PriceType { get; set; }

        public List<string> Categories { get; set; }

        //[ForeignKey("ProductOffer")]
        //public int ProductOfferId { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        //public virtual ProductOffer ProductOffer { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountPrice { get; set; }

        public decimal ShippingCost { get; set; }

        //price after discount
        public decimal FinalPrice { get; set; }

    }
}