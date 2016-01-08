using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Web;
using System.Web.WebSockets;
using MyProject.AppLogic.CartLogic;
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
        private CartProcessor cartProcessor = new CartProcessor();
        private List<Cart> cartLineItems;

        //[TestFixtureSetUp]
        public void OneTimeSetUp()
        {
            //Todo better way to mock shopping cart ?
            shoppingCart.ShoppingCartId = Guid.NewGuid().ToString();
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1001"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1002"));
            shoppingCart.AddToCart(CartContext.Products.Single(p => p.Code == "1005"));



            promotion = CartContext.Promotions.Single(p => p.Code == "Christmas discount");

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
                //    DiscountAmount = 0m,
                //    Quantity = cart.Count,
                //    ShippingCost = 0m,
                //    DiscountedPrice = 0m,
                //    ProductOffer =
                //        CartContext.ProductOffers.Single(po => po.ProductId == cart.ProductId && po.PriceType.Code == "R")
                //});
            }

            var list = shoppingCart.GetCartItems().ToList();
            list.ForEach(c => cartLineItems.Add(new Cart()
            {
                Code = shoppingCart.ShoppingCartId,
                DateCreated = DateTime.Now,
                OriginalPrice = CartContext.Products.Single(p => p.Id == c.ProductId).Price,
                DiscountAmount = 0m,
                Quantity = c.Quantity,
                ShippingCost = 0m,
                //DiscountedPrice = 0m,
                //ProductCode = "1001",
                PriceType = "R"
            }));
        }

        [Test]
        public void _0_BasicCheck()
        {
            Assert.AreEqual("1001", CartContext.Products.Single(p => p.Code == "1001").Code);

        }

        [Test]
        public void _1_CartPrice()
        {
            Assert.AreEqual(37.97m, shoppingCart.GetTotal());

        }

        [Test]
        public void _2_PromoExpression_Parse()
        {
            var exp =
                "Category=1001,1002;PriceType=R,W;ItemCode=1001,1005;FreeShipping=True;BuyItemCategory=1004;BuyItemCount=5;BuyItemCode=1001,1002;GetItemCategory=1003;GetItemCount=2;GetItemCode=1005;PercentDiscount=0.25;AmountDiscount=10";
            var promoExp = PromotionLineItemExpression.Parse(exp);
            
            Assert.AreEqual("1001,1002", string.Join(",", promoExp.Category.ToArray()));
            Assert.AreEqual("R,W", string.Join(",", promoExp.PriceType.ToArray()));
            Assert.AreEqual("1001,1005", string.Join(",", promoExp.ItemCode.ToArray()));
            Assert.AreEqual(true, promoExp.FreeShipping);
            Assert.AreEqual("1004", string.Join(",", promoExp.BuyItemCategory.ToArray()));
            Assert.AreEqual(5, promoExp.BuyItemCount);
            Assert.AreEqual("1003", string.Join(",", promoExp.GetItemCategory.ToArray()));
            Assert.AreEqual(2, promoExp.GetItemCount);
            Assert.AreEqual("1005", string.Join(",", promoExp.GetItemCode.ToArray()));
            Assert.AreEqual("1001,1002", string.Join(",", promoExp.BuyItemCode.ToArray()));
            Assert.AreEqual(.25m, promoExp.PercentDiscount);
            Assert.AreEqual(10m, promoExp.AmountDiscount);
        }

        [Test]
        public void _3_PercentDiscount_Categories()
        {
            //15 percent off Female-Winter-Collection
            var exp =
                "Category=1001;PriceType=R;PercentDiscount=0.15";
            var promoExp = PromotionLineItemExpression.Parse(exp);

            var promo = new PromotionLineItem {PromotionLineItemExpression = exp};

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Categories = new List<string>(){"1001"}, OriginalPrice = 40m, DiscountAmount = 0m, Quantity = 2,DiscountApplied = false, PriceType = "R"},
                new Cart(){Categories = new List<string>(){"1003", "1004"}.ToList(), OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 1,DiscountApplied = false, PriceType = "R"}
            };

            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(88m, cartlineItems.Sum(c => c.Sum));
            //Assert.AreEqual(CartContext.Products.Single(p => p.Code == "1001").Price,
            //    cartProcessor.Process(promotion.PromotionLineItems, cartLineItems).FirstOrDefault().DiscountedPrice);
        }

        [Test]
        public void _4_AmountDiscount_Categories()
        {
            //1.5 dollar off Female-Casual-Collection
            var exp =
                "Category=1002;PriceType=R;AmountDiscount=1.5";
            var promo = new PromotionLineItem { PromotionLineItemExpression = exp };

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Categories = new List<string>(){"1001"}, OriginalPrice = 40m, DiscountAmount = 0m, Quantity = 1, DiscountApplied = false, PriceType = "R"},
                new Cart(){Categories = new List<string>(){"1002", "1003"}, OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 2, DiscountApplied = false, PriceType = "R"}
            };

            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(77m, cartlineItems.Sum(c => c.Sum));
        }

        [Test]
        public void _5_PercentDiscount_ItemCodes()
        {
            //25 percent off item 1004
            var exp =
                "ItemCode=1004;PriceType=R;PercentDiscount=0.25";
            var promo = new PromotionLineItem { PromotionLineItemExpression = exp };

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1004"), OriginalPrice = 25m, DiscountAmount = 0m, Quantity = 1, DiscountApplied = false, PriceType = "R"},
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1005"), OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 2, DiscountApplied = false, PriceType = "R"}
            };

            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(58.75m, cartlineItems.Sum(c => c.Sum));
        }

        [Test]
        public void _6_AmountDiscount_ItemCodes()
        {
            //5.25 off item item 1005
            var exp =
                "ItemCode=1005;PriceType=R;AmountDiscount=5.25";
            var promo = new PromotionLineItem { PromotionLineItemExpression = exp };

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1004"), OriginalPrice = 25m, DiscountAmount = 0m, Quantity = 1, DiscountApplied = false, PriceType = "R"},
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1005"), OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 2, DiscountApplied = false, PriceType = "R"}
            };

            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(54.5m, cartlineItems.Sum(c => c.Sum));
        }

        [Test]
        public void _7_BuyItemsGetItemFree_SameItem()
        {
            //Buy 5 item 1004 get 1 item 1004 free
            var exp =
                "PriceType=R;BuyItemCode=1004;BuyItemCount=5;GetItemCode=1004;GetItemCount=1";
            var promo = new PromotionLineItem { PromotionLineItemExpression = exp };

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1004"), OriginalPrice = 25m, DiscountAmount = 0m, Quantity = 12, DiscountApplied = false, PriceType = "R"},
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1005"), OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 2, DiscountApplied = false, PriceType = "R"}
            };
            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(340m, cartlineItems.Sum(c => c.Sum));
            Assert.AreEqual(2, cartlineItems.Where(c => c.AddOnItem).ToList().Count);
            Assert.AreEqual("1004", cartlineItems.FirstOrDefault(c=>c.AddOnItem).Product.Code);
        }


        [Test]
        public void _8_BuyItemsGetItemFree_DifferentItem()
        {
            //Buy 5 item 1004 get 1004 free
            var exp =
                "PriceType=R;BuyItemCode=1004;BuyItemCount=5;GetItemCode=1001;GetItemCount=2";
            var promo = new PromotionLineItem { PromotionLineItemExpression = exp };

            var cartlineItems = new List<Cart>()
            {
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1004"), OriginalPrice = 25m, DiscountAmount = 0m, Quantity = 6, DiscountApplied = false, PriceType = "R"},
                new Cart(){Product = CartContext.Products.Single(p=> p.Code == "1005"), OriginalPrice = 20m, DiscountAmount = 0m, Quantity = 2, DiscountApplied = false, PriceType = "R"}
            };
            new CartProcessor().ApplyEach(promo, cartlineItems);
            Assert.AreEqual(190m, cartlineItems.Sum(c => c.Sum));
            Assert.AreEqual("1001", cartlineItems.Single(c => c.AddOnItem).Product.Code);
            Assert.AreEqual(1, cartlineItems.Where(c => c.AddOnItem).ToList().Count);
            Assert.AreEqual(2, cartlineItems.Single(c => c.AddOnItem).Quantity);
        }
    }
}
