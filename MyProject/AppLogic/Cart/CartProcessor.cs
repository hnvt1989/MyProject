using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace MyProject.AppLogic.Cart
{
    public class CartProcessor
    {
        public IEnumerable<CartLineItem> Process(IEnumerable<PromotionLineItem> promotions, IEnumerable<CartLineItem> cartLineItems)
        {
            foreach (var promo in promotions)
            {

            }
            return cartLineItems;
        }

        public void ApplyEach(PromotionLineItem promo, List<CartLineItem> cartLineItems)
        {
            var tempLineItems = new List<CartLineItem>();

            foreach (var lineItem in cartLineItems)
            {
                var promoExp = PromotionLineItemExpression.Parse(promo.PromotionLineItemExpression);

                if (promoExp.Category.Count > 0 && !lineItem.DiscountApplied && 
                    promoExp.PriceType.Contains(lineItem.PriceType))
                {
                    var qualifiedCategories = promoExp.Category.Intersect(lineItem.Categories);

                    if (qualifiedCategories.Count() > 0)
                    {
                        lineItem.DiscountAmount = promoExp.AmountDiscount +
                                                 (lineItem.OriginalPrice*promoExp.PercentDiscount);

                        lineItem.DiscountedPrice = lineItem.OriginalPrice - lineItem.DiscountAmount;
                    }
                    lineItem.DiscountApplied = true;
                }

                if (promoExp.ItemCode.Count > 0 && !lineItem.DiscountApplied && 
                    promoExp.PriceType.Contains(lineItem.PriceType))
                {
                    var qualified = promoExp.ItemCode.Contains(lineItem.ProductCode);
                    if (qualified)
                    {
                        lineItem.DiscountAmount = promoExp.AmountDiscount +
                                                 (lineItem.OriginalPrice * promoExp.PercentDiscount);

                        lineItem.DiscountedPrice = lineItem.OriginalPrice - lineItem.DiscountAmount;
                    }
                    lineItem.DiscountApplied = true;
                }

                if (promoExp.BuyItemCode.Count > 0 && !lineItem.DiscountApplied &&
                    promoExp.PriceType.Contains(lineItem.PriceType))
                {
                    var qualified = promoExp.BuyItemCode.Contains(lineItem.ProductCode) && lineItem.Quantity >= promoExp.BuyItemCount;
                    if (qualified)
                    {
                        //how many free items ?
                        var freeItemCount = lineItem.Quantity/promoExp.BuyItemCount;
                        //slow !!!!!!!!!!!!!!!
                        using (var context = new ShoppingCartContext())
                        {
                            var product = context.Products.Single(p => p.Code == promoExp.GetItemCode.FirstOrDefault()); //only support single get product
                            for (int i = 0; i < freeItemCount; i++)
                            {
                                tempLineItems.Add(new CartLineItem()
                                {
                                    OriginalPrice = product.Price,
                                    DiscountAmount = product.Price,
                                    ShippingCost = lineItem.ShippingCost,
                                    DiscountedPrice = 0m,
                                    DiscountApplied = true,
                                    AddOnItem = true,
                                    Quantity = promoExp.GetItemCount,
                                    Code = Guid.NewGuid().ToString(),
                                    Categories = lineItem.Categories,
                                    DateCreated = lineItem.DateCreated,
                                    PriceType = lineItem.PriceType,
                                    ProductCode = product.Code
                                });
                            }
                        }
                    }
                }

                //if the cart line item applies for this promo
                //if (promo.ProductOffers.Any(po => po.Code == lineItem.ProductOffer.Code) && lineItem.ProductOffer.Discountable && promo.Quantity >= lineItem.Quantity)
                //{
                //    if (promo.FreeShipping)
                //        lineItem.ShippingCost = 0m;
                //    lineItem.DiscountAmount = (lineItem.OriginalPrice * (1 - promo.PercentDiscount)) - promo.AmountDiscount;
                //    lineItem.DiscountedPrice = lineItem.OriginalPrice - lineItem.DiscountAmount + lineItem.ShippingCost;
                //}
            }

            cartLineItems.AddRange(tempLineItems);
            //return ret;
        }
    }
}