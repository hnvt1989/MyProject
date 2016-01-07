using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Web;
using System.Web.WebSockets;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using NUnit.Framework;

namespace UnitTest.DataUnitTest
{
    internal class CartProcessingUnitTest
    {
        public ShoppingCartContext CartContext = new ShoppingCartContext();
        private ShoppingCart shoppingCart = new ShoppingCart();
        private Promotion promotion;
        private CartProcessor cartProcessor;
        private List<CartLineItem> cartLineItems;

        //[TestFixtureSetUp]
        public void OneTimeSetUp()
        {
            //Todo better way to mock shopping cart ?
            shoppingCart.ShoppingCartId = Guid.NewGuid().ToString();
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1001"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1002"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1005"));



            promotion = CartContext.Promotions.Single(p => p.Code == "Christmas discount");
            cartProcessor = new CartProcessor();

            foreach (var cart in shoppingCart.GetCartItems())
            {
                var OriginalPrice = CartContext.Products.Single(p => p.Id == cart.ProductId).Price;
                var ProductOffer =
                    CartContext.ProductOffers.Single(po => po.ProductId == cart.ProductId && po.PriceType.Code == "R");
                string put;
                //cartLineItems.Add(new CartLineItem()
                //{
                //    Code = shoppingCart.ShoppingCartId,
                //    DateCreated = DateTime.Now,
                //    OriginalPrice = CartContext.Products.Single(p => p.Id == cart.ProductId).Price,
                //    DiscountPrice = 0m,
                //    Quantity = cart.Count,
                //    ShippingCost = 0m,
                //    FinalPrice = 0m,
                //    ProductOffer =
                //        CartContext.ProductOffers.Single(po => po.ProductId == cart.ProductId && po.PriceType.Code == "R")
                //});
            }

            var list = shoppingCart.GetCartItems().ToList();
            list.ForEach(c => cartLineItems.Add(new CartLineItem()
            {
                Code = shoppingCart.ShoppingCartId,
                DateCreated = DateTime.Now,
                OriginalPrice = CartContext.Products.Single(p => p.Id == c.ProductId).Price,
                DiscountPrice = 0m,
                Quantity = c.Count,
                ShippingCost = 0m,
                FinalPrice = 0m,
                ProductOffer =
                    CartContext.ProductOffers.Single(po => po.ProductId == c.ProductId && po.PriceTypeId == 1)
            }));
        }

        [Test]
        public void BasicCheck()
        {
            Assert.AreEqual("1001", CartContext.Products.Single(p => p.Code == "1001").Code);

        }

        [Test]
        public void CartPrice()
        {
            Assert.AreEqual(37.97m, shoppingCart.GetTotal());

        }

        [Test]
        public void Discount()
        {
            Assert.AreEqual(CartContext.Products.Single(p => p.Code == "1001").Price,
                cartProcessor.Process(promotion.PromotionLineItems, cartLineItems).FirstOrDefault().FinalPrice);
        }

        [Test]
        public void PromoExpression_Parse()
        {
            var exp =
                "Category=1001,1002;PriceType=R,W;ItemCode=1001,1005;FreeShipping=True;BuyItemCategory=1004;BuyItemCount=5;GetItemCategory=1003;GetItemCount=2";
            var promoExp = PromotionLineItemExpression.Parse(exp);
            
            Assert.AreEqual("1001,1002", string.Join(",", promoExp.Category.ToArray()));
            Assert.AreEqual("R,W", string.Join(",", promoExp.PriceType.ToArray()));
            Assert.AreEqual("1001,1005", string.Join(",", promoExp.ItemCode.ToArray()));
            Assert.AreEqual(true, promoExp.FreeShipping);
            Assert.AreEqual("1004", string.Join(",", promoExp.BuyItemCategory.ToArray()));
            Assert.AreEqual(5, promoExp.BuyItemCount);
            Assert.AreEqual("1003", string.Join(",", promoExp.GetItemCategory.ToArray()));
            Assert.AreEqual(2, promoExp.GetItemCount);
        }
    }
}
