using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        //[ForeignKey("Category")]
        //public int CategoryId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool FeatureProduct { get; set; }

        public byte[] Image { get; set; }

        public ICollection<Category> Categories { get; set; } 
    }

    public class ProductOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        [ForeignKey("PriceType")]
        public int PriceTypeId { get; set; }

        public decimal Price { get; set; }

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


    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public int PromotionLineItemId { get; set; }

        public virtual ICollection<PromotionLineItem> PromotionLineItems { get; set; }

        //the starting date of the promotion
        public DateTime StartDate { get; set; }
        
        //the ending date of the promotion
        public DateTime EndDate { get; set; }

        //can be used with other promotions ?
        public bool Exclusive { get; set; }

        //the order of the cart processing
        public int Order { get; set; }
    }

    public class PromotionLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProductOffer")]
        public int ProductOfferId { get; set; }

        public int Quantity { get; set; }

        public bool FreeShipping { get; set; }

        public int PercentDiscount { get; set; }

        public int AmountDiscount { get; set; }

    }

    public class CartProcessor
    {
        public List<CartLineItem> Process(List<PromotionLineItem> promotions, List<CartLineItem> cartLineItems  )
        {
            return cartLineItems;
        }
        
    }

    public class CartLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("ProductOffer")]
        public int ProductOfferId { get; set; }

        public int Qty { get; set; }

        public System.DateTime DateCreated { get; set; }

        public virtual ProductOffer ProductOffer { get; set; }

        public bool Discountable { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountPrice { get; set; }

        //price after discount
        public decimal FinalPrice { get; set; }

    }
}