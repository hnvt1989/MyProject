using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            foreach (var lineItem in cartLineItems)
            {
                var promoExp = PromotionLineItemExpression.Parse(promo.PromotionLineItemExpression);

                if (promoExp.Category.Count > 0)
                {
                    var qualifiedCategories = promoExp.Category.Intersect(lineItem.Categories);

                    if (qualifiedCategories.Count() > 0)
                    {
                        lineItem.DiscountPrice = (lineItem.OriginalPrice*promoExp.AmountDiscount) +
                                                 (lineItem.OriginalPrice*promoExp.PercentDiscount);
                        lineItem.FinalPrice = lineItem.OriginalPrice - lineItem.DiscountPrice;
                    }


                }
                //if the cart line item applies for this promo
                //if (promo.ProductOffers.Any(po => po.Code == lineItem.ProductOffer.Code) && lineItem.ProductOffer.Discountable && promo.Quantity >= lineItem.Quantity)
                //{
                //    if (promo.FreeShipping)
                //        lineItem.ShippingCost = 0m;
                //    lineItem.DiscountPrice = (lineItem.OriginalPrice * (1 - promo.PercentDiscount)) - promo.AmountDiscount;
                //    lineItem.FinalPrice = lineItem.OriginalPrice - lineItem.DiscountPrice + lineItem.ShippingCost;
                //}
            }
            //return ret;
        }
    }
}