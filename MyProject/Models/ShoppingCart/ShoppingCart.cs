using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using MyProject.AppLogic.CartLogic;
using MyProject.DAL;
using NUnit.Framework.Internal;
using WebGrease.Css.Extensions;

namespace MyProject.Models.ShoppingCart
{
    public partial class ShoppingCart
    {
        ShoppingCartContext soContext = new ShoppingCartContext();
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartCode";
        public CartProcessor CartProcessor = new CartProcessor();

        public List<Cart> CartItems
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    return context.Carts.Where(cart => cart.Code == ShoppingCartId).ToList();
                }
            }
        }

        public void UpdateCart()
        {
            using (var context = new ShoppingCartContext())
            {
                var shippingRateObj = context.AppSettings.Single(a => a.Code == "ShippingRate");
                var shippingRate = 0m;
                if (shippingRateObj.ValueType == "decimal")
                    shippingRate = Convert.ToDecimal(shippingRateObj.Value);

                var cartItems = context.Carts.Where(cart => cart.Code == ShoppingCartId).ToList();

                foreach (var c in cartItems)
                {
                    c.Categories = context.Products.Where(p => p.Id == c.ProductId).SelectMany(x => x.Categories).Select(y => y.Code).ToList();
                }

                var newCarts = CartProcessor.Process(context.Promotions.Single(p => p.Code == "Christmas discount").PromotionLineItems.ToList(), cartItems);

                foreach (var cart in context.Carts)
                {
                    foreach (var nCart in newCarts)
                    {
                        if (cart.Code == nCart.Code && cart.ProductId == nCart.ProductId)
                        {
                            cart.Quantity = nCart.Quantity;
                            cart.DiscountAmount = nCart.DiscountAmount;
                            cart.ShippingCost = nCart.ProductWeight * nCart.Quantity * shippingRate; //fixed rate shipping
                            cart.DiscountAmount = nCart.DiscountAmount;
                            cart.OriginalPrice = nCart.OriginalPrice;
                            cart.AddOnItem = nCart.AddOnItem;
                            //cart.DiscountedPrice = nCart.DiscountedPrice;
                            cart.NetBeforeDiscount = nCart.NetBeforeDiscount;
                            cart.Sum = nCart.Sum + (nCart.ProductWeight*nCart.Quantity*shippingRate);
                            cart.TotalDiscountAmount = nCart.TotalDiscountAmount;

                            //context.Entry(cart).State = EntityState.Modified;
                        }
                    }

                }
                context.SaveChanges();
            }
        }


        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            using (var context = new ShoppingCartContext())
            {
                // Get the matching cart and product instances
                var cartItem = context.Carts.SingleOrDefault(
                    c => c.Code == ShoppingCartId
                    && c.ProductId == product.Id);

                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        ProductId = product.Id,
                        Code = ShoppingCartId,
                        Quantity = 1,
                        DateCreated = DateTime.Now,
                        PriceType = "R",
                        Categories = new List<string>() { "1001", "1002" },
                        AddOnItem = false,
                        ShippingCost = 0m,
                        DiscountAmount = 0m,
                        OriginalPrice = context.Products.Where(p => p.Id == product.Id).SelectMany(x => x.ProductOffers).ToList().Single(o => o.ProductId == product.Id && o.PriceTypeId == 1).Price,
                        //OriginalPrice = context.Products.Single(p => p.Id == product.Id).Price,
                        Product = context.Products.Single(p => p.Id == product.Id),
                        //DiscountedPrice = 0m,
                        DiscountApplied = false,
                        ProductWeight = product.Weight
                    };
                    context.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, 
                    // then add one to the quantity
                    cartItem.Quantity++;
                }
                // Save changes
                context.SaveChanges();


                //update the cart (promotion)
                UpdateCart();

            }
        }

        public Cart RemoveFromCart(int id)
        {
            using (var context = new ShoppingCartContext())
            {


                // Get the cart
                var cartItem = context.Carts.Single(
                    cart => cart.Code == ShoppingCartId
                    && cart.Id == id);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        // Save changes
                        context.SaveChanges();
                        UpdateCart();
                    }
                    else
                    {
                        context.Carts.Remove(cartItem);
                        // Save changes
                        context.SaveChanges();
                        UpdateCart();
                        return null;
                    }

                }
                return context.Carts.Single(c => c.Code == ShoppingCartId && c.Id == id);
            }
        }

        //public Cart GetCartLine(string cartId, int id)
        //{
        //    using (var context = new ShoppingCartContext())
        //    {
        //        return context.Carts.Single(c => c.Code == cartId && c.Id == id);
        //    }
        //}

        public Cart AddOneItemToCart(int id)
        {
            using (var context = new ShoppingCartContext())
            {
                // Get the matching cart and product instances
                // Get the cart
                var cartItem = context.Carts.Single(
                    cart => cart.Code == ShoppingCartId
                    && cart.Id == id);

                if (cartItem != null)
                {
                    //Add 1 more item
                    cartItem.Quantity++;
                    // Save changes
                    context.SaveChanges();

                    UpdateCart();
                }
                var ret = context.Carts.Single(
                    cart => cart.Code == ShoppingCartId
                            && cart.Id == id);
                return ret;
            }
        }

        public void EmptyCart()
        {
            using (var context = new ShoppingCartContext())
            {
                var cartItems = context.Carts.Where(
                    cart => cart.Code == ShoppingCartId);

                foreach (var cartItem in cartItems)
                {
                    context.Carts.Remove(cartItem);
                }
                // Save changes
                context.SaveChanges();

                UpdateCart();
            }
        }

        public List<Cart> GetCartItems()
        {

            return soContext.Carts.Where(cart => cart.Code == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            using (var context = new ShoppingCartContext())
            {
                int? count = (from cartItems in context.Carts
                              where cartItems.Code == ShoppingCartId
                              select (int?)cartItems.Quantity).Sum();
                // Return 0 if all entries are null
                return count ?? 0;
            }
        }

        public decimal GetTotal()
        {
            using (var context = new ShoppingCartContext())
            {
                var carts = context.Carts.Where(c => c.Code == ShoppingCartId).ToList();
                if (carts.Count() > 0)
                    return carts.Select(c => Convert.ToDecimal(c.Sum)).Sum();
                return 0m;

                //decimal? total = (from cartItems in context.Carts
                //                  where cartItems.Code == ShoppingCartId
                //                  select (decimal?)cartItems.Sum).Sum();

                //return (total != null) ? decimal.Round((decimal)total, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
            }
        }

        public decimal GetTotalShippingCost()
        {
            using (var context = new ShoppingCartContext())
            {
                var carts = context.Carts.Where(c => c.Code == ShoppingCartId).ToList();
                if (carts.Count() > 0)
                    return carts.Select(c => Convert.ToDecimal(c.ShippingCost)).Sum();
                return 0m;

                //decimal? total = (from cartItems in context.Carts
                //                  where cartItems.Code == ShoppingCartId
                //                  select (decimal?)cartItems.Sum).Sum();

                //return (total != null) ? decimal.Round((decimal)total, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
            }
        }

        public long CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            decimal shippingCost = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each

            //todo: sequence number ?
            //var nextOrderId = soContext.Orders.Max(o => o.Id);

            //order.Id = nextOrderId;

            foreach (var item in cartItems)
            {
                var lineOrderDetail = new LineOrderDetail
                {
                    ProductId = item.ProductId,

                    UnitPrice = item.OriginalPrice,
                    Quantity = item.Quantity,
                    ShippingCost = item.ShippingCost,
                    Net = item.NetBeforeDiscount,
                    TotalDiscount = item.TotalDiscountAmount,
                    Total = item.Sum,
                };
                // Set the order total of the shopping cart
                orderTotal += item.Sum;
                shippingCost += item.ShippingCost;

                order.OrderDetails.Add(lineOrderDetail);

            }

            //determine if this order is going to be back ordered or not
            using (var context = new ShoppingCartContext())
            {
                var backOrderStatusId = context.OrderStatuses.Single(s => s.Code == "BackOrder").Id;
                var processingOrderStatusId = context.OrderStatuses.Single(s => s.Code == "Processing").Id;

                foreach (var prod in order.OrderDetails)
                {
                    var product = context.Products.SingleOrDefault(p => p.Id == prod.ProductId);
                    if (product != null)
                    {
                        if (product.QuantityOnHand < prod.Quantity)
                            order.OrderStatusId = backOrderStatusId;
                        prod.Profit = prod.Total - (product.BuyInPrice * prod.Quantity);
                        order.Profit += prod.Profit;
                    }

                }

                if (order.OrderStatusId != backOrderStatusId && order.OrderStatusId != processingOrderStatusId)
                {
                    order.OrderStatusId = processingOrderStatusId;
                }
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal + shippingCost;
            order.ShippingCost = shippingCost;

            order.PostedAmount = 0m;
            //order.ActualSoldAmount = order.Total;

            soContext.Orders.Add(order);
            // Save the order
            soContext.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the Id as the confirmation number
            return order.OrderNumber;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = soContext.Carts.Where(
                c => c.Code == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.Code = userName;
            }
            soContext.SaveChanges();
        }


    }
}