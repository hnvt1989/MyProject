using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        readonly ShoppingCartContext _shoppingCartContext = new ShoppingCartContext();
        //ProductContext _productContext = new ProductContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),           
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            var addedProduct= _shoppingCartContext.Products
                .Single(product => product.Id == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            //Go back to index
            return RedirectToAction("Index");
        }

        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string productDescription = _shoppingCartContext.Carts
                .Single(item => item.Id == id).Product.Description;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productDescription) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        // AJAX: /ShoppingCart/AddOneItemToCart/5
        [HttpPost]
        public ActionResult AddOneItemToCart(int id)
        {

            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the product to display confirmation

            string productDescription = _shoppingCartContext.Carts
                .Single(item => item.Id == id && item.Code == cart.ShoppingCartId).Product.Description;


            int itemCount = cart.AddOneItemToCart(id);

            var updatedCart = _shoppingCartContext.Carts.Single(item => item.Id == id && item.Code == cart.ShoppingCartId);

            // update cart view
            var results = new ShoppingCartAddViewModel()
            {
                Message = Server.HtmlEncode(productDescription) +
                    " has been added to your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                TotalDiscount = _shoppingCartContext.Carts.Single(item => item.Id == id && item.Code == cart.ShoppingCartId).TotalDiscountAmount,
                ShippingCost = _shoppingCartContext.Carts.Single(item => item.Id == id && item.Code == cart.ShoppingCartId).ShippingCost,
                NetBeforeDiscount = _shoppingCartContext.Carts.Single(item => item.Id == id && item.Code == cart.ShoppingCartId).NetBeforeDiscount,
                Sum = updatedCart.Sum,
                AddId = id,
                
            };
            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}