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


    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public virtual ICollection<PromotionLineItem> PromotionLineItems { get; set; }

    }

    public class PromotionLineItem
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //public string Code { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public bool FreeShipping { get; set; }

        public decimal PercentDiscount { get; set; }

        public decimal AmountDiscount { get; set; }

        public ICollection<ProductOffer> ProductOffers { get; set; }

        //the starting date of the promotion
        public DateTime StartDate { get; set; }

        //the ending date of the promotion
        public DateTime EndDate { get; set; }

        public bool Active { get; set; }

        //can be used with other promotions ?
        public bool Exclusive { get; set; }

        //the order of the cart processing
        public int Order { get; set; }

        public ICollection<Category> Categories { get; set; } 



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string PromotionLineItemExpression { get; set; }
    }

    public class PromotionLineItemExpression
    {
        public List<string> Category { get; set; }
        public List<string> PriceType { get; set; }
        public List<string> ItemCode { get; set; }
        public bool FreeShipping { get; set; }
        public List<string> BuyItemCategory { get; set; }
        public int BuyItemCount { get; set; }
        public List<string> GetItemCategory { get; set; }
        public int GetItemCount { get; set; }

        public static PromotionLineItemExpression Parse(string expression)
        {
            var ret = new PromotionLineItemExpression()
            {
                Category = new List<string>(),
                PriceType = new List<string>(),
                ItemCode = new List<string>(),
                FreeShipping = false,
                BuyItemCategory = new List<string>(),
                BuyItemCount = 0,
                GetItemCategory = new List<string>(),
                GetItemCount = 0
            };

            ret.Category.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.Category.ToString())).Split('=')[1].Split(','));
            ret.PriceType.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.PriceType.ToString())).Split('=')[1].Split(','));
            ret.ItemCode.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.ItemCode.ToString())).Split('=')[1].Split(','));
            ret.FreeShipping =
                expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.FreeShipping.ToString())).Split('=')[1] == "True";
            ret.BuyItemCategory.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCategory.ToString())).Split('=')[1].Split(','));
            ret.BuyItemCount = int.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCount.ToString())).Split('=')[1]);
            ret.GetItemCategory.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCategory.ToString())).Split('=')[1].Split(','));
            ret.GetItemCount = int.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCount.ToString())).Split('=')[1]);
            return ret;
        }
    }


    //public class Criteria
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }

    //    public string Code { get; set; }

    //    public string Category { get; set; }
    //    public string PriceType { get; set; }
    //    public string ItemCode { get; set; }
    //    public bool FreeShipping { get; set; }
    //    public string BuyItemCategory { get; set; }
    //    public string BuyItemCount { get; set; }
    //    public string GetItemCategory { get; set; }
    //    public string GetItemCount { get; set; }

    //}

    public enum Condition { Equal = 1, LessThan = 2, GreaterThan = 3, True = 4, False = 5}
    public enum Comparator {Category, PriceType, ItemCode, FreeShipping, BuyItemCategory, BuyItemCount, GetItemCategory, GetItemCount}



    public class CartProcessor
    {
        public IEnumerable<CartLineItem> Process(IEnumerable<PromotionLineItem> promotions, IEnumerable<CartLineItem> cartLineItems  )
        {
            foreach (var promo in promotions)
            {
                
            }
            return cartLineItems;
        }

        public List<CartLineItem> ApplyEach(PromotionLineItem promo, List<CartLineItem> cartLineItems)
        {
            foreach (var lineItem in cartLineItems)
            {
                //if the cart line item applies for this promo
                if (promo.ProductOffers.Any(po=> po.Code == lineItem.ProductOffer.Code) && lineItem.ProductOffer.Discountable && promo.Quantity >= lineItem.Quantity)
                {
                    if (promo.FreeShipping)
                        lineItem.ShippingCost = 0m;
                    lineItem.DiscountPrice = (lineItem.OriginalPrice*(1 - promo.PercentDiscount)) - promo.AmountDiscount;
                    lineItem.FinalPrice = lineItem.OriginalPrice - lineItem.DiscountPrice + lineItem.ShippingCost;
                }
            }
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

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public virtual ProductOffer ProductOffer { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountPrice { get; set; }

        public decimal ShippingCost { get; set; }

        //price after discount
        public decimal FinalPrice { get; set; }

    }
}