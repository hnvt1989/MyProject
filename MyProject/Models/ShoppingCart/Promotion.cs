using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyProject.Models.ShoppingCart
{

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

        //public string Description { get; set; }

        //public int Quantity { get; set; }

        //public bool FreeShipping { get; set; }

        //public decimal PercentDiscount { get; set; }

        //public decimal AmountDiscount { get; set; }

        //public ICollection<ProductOffer> ProductOffers { get; set; }

        ////the starting date of the promotion
        //public DateTime StartDate { get; set; }

        ////the ending date of the promotion
        //public DateTime EndDate { get; set; }

        //public bool Active { get; set; }

        ////can be used with other promotions ?
        //public bool Exclusive { get; set; }

        ////the order of the cart processing
        //public int Order { get; set; }

        //public ICollection<Category> Categories { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        //the starting date of the promotion
        public DateTime StartDate { get; set; }

        //the ending date of the promotion
        public DateTime EndDate { get; set; }

        public bool Active { get; set; }

        public string PromotionLineItemExpression { get; set; }
    }

    public class PromotionLineItemExpression
    {
        public List<string> Category { get; set; }
        public List<string> PriceType { get; set; }
        public List<string> ItemCode { get; set; }
        public bool FreeShipping { get; set; }
        public List<string> BuyItemCategory { get; set; }
        public List<string> BuyItemCode { get; set; }
        public int BuyItemCount { get; set; }
        public List<string> GetItemCategory { get; set; }
        public List<string> GetItemCode { get; set; } 
        public int GetItemCount { get; set; }

        //discount amount
        public decimal PercentDiscount { get; set; }
        public decimal AmountDiscount { get; set; }


        public static PromotionLineItemExpression Parse(string expression)
        {
            var ret = new PromotionLineItemExpression()
            {
                Category = new List<string>(),
                PriceType = new List<string>(),
                ItemCode = new List<string>(),
                FreeShipping = false,
                BuyItemCategory = new List<string>(),
                BuyItemCode =  new List<string>(),
                BuyItemCount = 0,
                GetItemCategory = new List<string>(),
                GetItemCode = new List<string>(),
                GetItemCount = 0,

                PercentDiscount = 0,
                AmountDiscount = 0
            };

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.Category.ToString()))))
            {
                ret.Category.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.Category.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.PriceType.ToString()))))
            {
                ret.PriceType.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.PriceType.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.ItemCode.ToString()))))
            {
                ret.ItemCode.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.ItemCode.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.FreeShipping.ToString()))))
            {
                ret.FreeShipping =
                    expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.FreeShipping.ToString())).Split('=')[1] == "True";
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCategory.ToString()))))
            {
                ret.BuyItemCategory.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCategory.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCode.ToString()))))
            {
                ret.BuyItemCode.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCode.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCount.ToString()))))
            {
                ret.BuyItemCount = int.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.BuyItemCount.ToString())).Split('=')[1]);
            }


            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCategory.ToString()))))
            {
                ret.GetItemCategory.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCategory.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCode.ToString()))))
            {
                ret.GetItemCode.AddRange(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCode.ToString())).Split('=')[1].Split(','));
            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCount.ToString()))))
            {
                ret.GetItemCount = int.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.GetItemCount.ToString())).Split('=')[1]);
            }

            if(!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.PercentDiscount.ToString()))))
            {
                ret.PercentDiscount = decimal.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.PercentDiscount.ToString())).Split('=')[1]);

            }

            if (!string.IsNullOrEmpty(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.AmountDiscount.ToString()))))
            {
                ret.AmountDiscount = decimal.Parse(expression.Split(';').FirstOrDefault(e => e.StartsWith(Comparator.AmountDiscount.ToString())).Split('=')[1]);

            }
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

    public enum Condition { Equal = 1, LessThan = 2, GreaterThan = 3, True = 4, False = 5 }
    public enum Comparator { Category, PriceType, ItemCode, FreeShipping, BuyItemCategory, BuyItemCode, BuyItemCount, GetItemCategory, GetItemCount , GetItemCode, PercentDiscount, AmountDiscount}
}