using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.AppLogic.CartLogic;
using MyProject.DAL;
using NUnit.Framework.Internal;

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
                return soContext.Carts.Where(
                cart => cart.Code == ShoppingCartId).ToList();
            }
        }

        public void UpdateCart()
        {
            foreach (var c in CartItems)
            {
                //parsing cat again
                //c.Categories =
                //    soContext.Products.Single(p => p.Id == c.ProductId).Categories.Select(cat => cat.Code).ToList();
                //var prod = soContext.Products.Where(p => p.Id == c.ProductId).SelectMany(x => x.Categories);
                c.Categories = soContext.Products.Where(p => p.Id == c.ProductId).SelectMany(x => x.Categories).Select(y => y.Code).ToList();
            }

            var newCarts = CartProcessor.Process(soContext.Promotions.Single(p => p.Code == "Christmas discount").PromotionLineItems.ToList(), CartItems);

            foreach (var cart in soContext.Carts)
            {
                foreach (var nCart in newCarts)
                {
                    if (cart.Code == nCart.Code && cart.ProductId == nCart.ProductId)
                    {
                        cart.Quantity = nCart.Quantity;
                        cart.DiscountAmount = nCart.DiscountAmount;
                        cart.ShippingCost = nCart.ShippingCost;
                        cart.DiscountAmount = nCart.DiscountAmount;
                        cart.OriginalPrice = nCart.OriginalPrice;
                        cart.AddOnItem = nCart.AddOnItem;
                        //cart.DiscountedPrice = nCart.DiscountedPrice;
                        cart.NetBeforeDiscount = nCart.NetBeforeDiscount;
                        cart.Sum = nCart.Sum;
                        cart.TotalDiscountAmount = nCart.TotalDiscountAmount;

                    }
                }

            }
            soContext.SaveChanges();
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
            // Get the matching cart and product instances
            var cartItem = soContext.Carts.SingleOrDefault(
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
                    OriginalPrice = soContext.Products.Single(p => p.Id == product.Id).Price,
                    Product = soContext.Products.Single(p => p.Id == product.Id),
                    //DiscountedPrice = 0m,
                    DiscountApplied = false,
                };
                soContext.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Quantity++;
            }
            // Save changes
            soContext.SaveChanges();


            //update the cart (promotion)
            UpdateCart();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = soContext.Carts.Single(
                cart => cart.Code == ShoppingCartId
                && cart.Id == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    soContext.Carts.Remove(cartItem);
                }
                // Save changes
                soContext.SaveChanges();
            }

            UpdateCart();

            return itemCount;
        }

        public Cart AddOneItemToCart(int id)
        {
            // Get the matching cart and product instances
            // Get the cart
            var cartItem = soContext.Carts.Single(
                cart => cart.Code == ShoppingCartId
                && cart.Id == id);

            var itemCount = 0;

            if (cartItem != null)
            {
                //Add 1 more item
                cartItem.Quantity++;
                itemCount = cartItem.Quantity;

                // Save changes
                soContext.SaveChanges();

                UpdateCart();
            }

            return soContext.Carts.Single(
                cart => cart.Code == ShoppingCartId
                        && cart.Id == id);
        }

        public void EmptyCart()
        {
            var cartItems = soContext.Carts.Where(
                cart => cart.Code == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                soContext.Carts.Remove(cartItem);
            }
            // Save changes
            soContext.SaveChanges();

            UpdateCart();
        }

        public List<Cart> GetCartItems()
        {
            return soContext.Carts.Where(
                cart => cart.Code == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in soContext.Carts
                          where cartItems.Code == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in soContext.Carts
                              where cartItems.Code == ShoppingCartId
                              select (decimal?)cartItems.Sum).Sum();

            return total ?? decimal.Zero;
        }

        public long CreateOrder(Order order)
        {
            decimal orderTotal = 0;

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

                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Quantity * item.Product.Price);

                order.OrderDetails.Add(lineOrderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

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