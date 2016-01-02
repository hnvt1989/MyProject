using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;

namespace MyProject.Models.ShoppingCart
{
    public partial class ShoppingCart
    {
        ShoppingCartContext soContext = new ShoppingCartContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "Code";

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
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                soContext.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            soContext.SaveChanges();
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
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    soContext.Carts.Remove(cartItem);
                }
                // Save changes
                soContext.SaveChanges();
            }
            return itemCount;
        }

        public int AddOneItemToCart(int id)
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
                cartItem.Count++;
                itemCount = cartItem.Count;
            }
            // Save changes
            soContext.SaveChanges();
            return itemCount;
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
                          select (int?)cartItems.Count).Sum();
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
                              select (int?)cartItems.Count *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var lineOrderDetail = new LineOrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Product.Price);

                soContext.LineOrderDetails.Add(lineOrderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            soContext.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the Id as the confirmation number
            return order.Id;
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