using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.BuilderProperties;
using MyProject.DAL;
using MyProject.Models.Account;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;
using Address = MyProject.Models.ShoppingCart.Address;

namespace MyProject.Controllers
{
    public class CheckoutController : Controller
    {
        //
        // GET: /Checkout/
        public ActionResult Index(string cartCode, decimal cartTotal)
        {
            if (ModelState.IsValid)
            {
                //if user not logged in, ask them to log in to place an order
                //if (!Request.IsAuthenticated)
                //{
                //    return RedirectToAction("Login", "Account");
                //}


                //using (var context = new ShoppingCartContext())
                //{
                //    context.Carts.Where(c => c.Code == HttpContext.Session["CartCode"] as string).ForEach(cart => cart.Code = HttpContext.User.Identity.Name);
                //    context.SaveChanges();

                //    //update the cart code session
                //    HttpContext.Session["CartCode"] = HttpContext.User.Identity.Name;
                //}

                var user = new ApplicationUser();
                using (IdentityContext _idDb = new IdentityContext())
                {
                    var _currentUserId = User.Identity.GetUserId();
                    user = _idDb.Users.FirstOrDefault(x => x.Id == _currentUserId);

                }

                var addresses = new List<Address>();
                if (user != null)
                {
                    addresses = AddressFlow.GetAccountAddresses(user.UserName);
                }
                    

                var checkoutModel = new CheckoutViewModel()
                {
                    CartTotal = cartTotal,
                    PaymentAmount = cartTotal,
                    PaymentTransaction = new PaymentTransaction()
                    {
                        //todo return billing address from account
                        PaymentType = null
                    },
                    //PaymentTypes = new List<PaymentType>(),
                    //todo: return shipping address from account
                    ShippingAddress = addresses.SingleOrDefault(a=>a.Primary),
                    //Name = user.LastName + " " + user.FirstName,
                    //Email = user.Email,
                    //Phone = user.PhoneNumber
                };

                if (user != null)
                {
                    checkoutModel.Name = user.LastName + " " + user.FirstName;
                    checkoutModel.Email = user.Email;
                    checkoutModel.Phone = user.PhoneNumber;
                }
                TempData["CheckoutInfo"] = checkoutModel;

                //using (var context = new ShoppingCartContext())
                //{
                //    checkoutModel.PaymentTypes.AddRange(context.PaymentTypes.ToList());
                //}


                //checkoutModel.PaymentTypesList = new SelectList(checkoutModel.PaymentTypes, "Code", "Description");
                return View(checkoutModel);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(CheckoutViewModel model)
        {
            string cartCode = "";
            decimal cartTotal = 0m;

            var cart = ShoppingCart.GetCart(this.HttpContext);
            //to redirect if view is not valid
            cartCode = cart.ShoppingCartId;
            cartTotal = cart.GetTotal();


            if (ModelState.IsValid)
            {
                
                
                // Set up our ViewModel
                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = cart.GetCartItems(),
                    CartTotal = cart.GetTotal() + cart.GetTotalShippingCost(),
                    CartTotalShippingCost = cart.GetTotalShippingCost(),
                };



                //update the cart code to the user id if they are logged in
                //viewModel.CartItems.ForEach(c => c.Code = cart.GetCartId(HttpContext));

                string paymentTypeValue = Request.Form["paymentType"].ToString();
                PaymentType paymentType;

                using (var context = new ShoppingCartContext())
                {
                    paymentType  = context.PaymentTypes.Single(t => t.Description == paymentTypeValue);
                }

                var m = new OrderConfirmViewModel()
                {
                    CartViewModel = viewModel,
                    CheckOutInfo = new CheckoutViewModel()
                    {
                        ShippingAddress = model.ShippingAddress,
                        PaymentTransaction = new PaymentTransaction()
                        {
                            //todo: comment out this line, because we disabled the amount text box.
                            //todo: when amount text box is enabled, uncomment this line to get the entered amount value
                            //Amount = model.PaymentTransaction.Amount,


                            Amount = viewModel.CartTotal,
                            Code = Guid.NewGuid().ToString(),
                            PartialPayment = false,
                            //PartialPayment = (model.PaymentTransaction.Amount < viewModel.CartTotal),
                            //todo:
                            PaymentStatusId = 1, //1 = on hold, 2 = processing, 3 = processed, 4 = complete
                            PaymentTypeId = paymentType.Id,
                            PaymentTypeDescription =  paymentType.Description,
                            PostedAmount = 0m

                        },
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone
                    }
                };
                m.CheckOutInfo.ShippingAddress.Primary = false;
                m.CheckOutInfo.ShippingAddress.Code = Guid.NewGuid().ToString();
                m.CheckOutInfo.ShippingAddress.AddressTypeId = 2;


                TempData["OrderInfo"] = m;
                return RedirectToAction("Index", "OrderConfirm");
            }

            var checkoutModel = TempData["CheckoutInfo"];
            return View(checkoutModel);
        }
    }
}