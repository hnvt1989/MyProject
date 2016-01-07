using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using NUnit.Framework;

namespace UnitTest.DataUnitTest
{
    class CartProcessingUnitTest
    {
        public ShoppingCartContext CartContext = new ShoppingCartContext();
        ShoppingCart shoppingCart = new ShoppingCart();
        private Promotion promotion;
        private CartProcessor cartProcessor;
        private List<CartLineItem> cartLineItems;
            
        [TestFixtureSetUp]
        public void OneTimeSetUp()
        {
            //Todo better way to mock shopping cart ?
            shoppingCart.ShoppingCartId = Guid.NewGuid().ToString();
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1001"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1002"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1005"));



            promotion = CartContext.Promotions.Single(p => p.Code == "Christmas discount");
            cartProcessor = new CartProcessor();

            shoppingCart.GetCartItems().ForEach(c => cartLineItems.Add(new CartLineItem()
            {
                Code = shoppingCart.ShoppingCartId,
                DateCreated = DateTime.Now,
                OriginalPrice = c.Product.Price,
                DiscountPrice = 0m,
                Quantity = c.Count,
                ShippingCost = 0m,
                FinalPrice = 0m,
                ProductOffer = CartContext.ProductOffers.Single(po => po.Product.Id == c.Product.Id && po.PriceType.Code == "R")
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
            Assert.AreEqual(CartContext.Products.Single(p => p.Code == "1001").Price, cartProcessor.Process(promotion.PromotionLineItems, cartLineItems).FirstOrDefault().FinalPrice);
        }
    }
}
