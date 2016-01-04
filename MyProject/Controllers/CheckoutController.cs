﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.BuilderProperties;
using MyProject.DAL;
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
            //if user not logged in, ask them to log in to place an order
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }


            //using (var context = new ShoppingCartContext())
            //{
            //    context.Carts.Where(c => c.Code == HttpContext.Session["CartCode"] as string).ForEach(cart => cart.Code = HttpContext.User.Identity.Name);
            //    context.SaveChanges();

            //    //update the cart code session
            //    HttpContext.Session["CartCode"] = HttpContext.User.Identity.Name;
            //}

            var checkoutModel = new CheckoutViewModel()
            {
                CartTotal = cartTotal,
                PaymentAmount = cartTotal,
                PaymentTransaction = new PaymentTransaction()
                {
                    //todo return billing address from account
                    PaymentType = null
                },
                //todo: return shipping address from account
                ShippingAddress = null,

                //todo: load from PaymentTypes table
                PaymentTypes = new List<PaymentType>()
                {
                    new PaymentType()
                    {
                        Id = 1,
                        Code = "Cash",
                        Description = "Cash"
                    },
                    new PaymentType()
                    {
                        Id = 2,
                        Code = "PayPal",
                        Description = "PayPal"
                    }
                }
            };

            checkoutModel.PaymentTypesList = new SelectList(checkoutModel.PaymentTypes, "Code", "Description");
            return View(checkoutModel);
        }

        [HttpPost]
        public ActionResult Index(CheckoutViewModel model)
        {

            if (ModelState.IsValid)
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                // Set up our ViewModel
                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = cart.GetCartItems(),
                    CartTotal = cart.GetTotal()
                };

                //update the cart code to the user id if they are logged in
                //viewModel.CartItems.ForEach(c => c.Code = cart.GetCartId(HttpContext));

                string paymentTypeValue = Request.Form["paymentType"].ToString();
                var paymentType = new PaymentType();
                switch (paymentTypeValue)
                {
                    case "Cash":
                        paymentType.Code = "Cash";
                        paymentType.Id = 1;
                        paymentType.Description = "Cash payment";
                        break;
                    case "PayPal":
                        paymentType.Code = "PayPal";
                        paymentType.Id = 2;
                        paymentType.Description = "Paypal payment";
                        break;
                    default:
                        paymentType.Code = "Default";
                        paymentType.Id = 0;
                        paymentType.Description = "Default payment";
                        break;
                }

                var m = new OrderConfirmViewModel()
                {
                    CartViewModel = viewModel,
                    CheckOutInfo = new CheckoutViewModel()
                    {
                        ShippingAddress = model.ShippingAddress,
                        PaymentTransaction = new PaymentTransaction()
                        {
                            Amount = model.PaymentTransaction.Amount,
                            Code = Guid.NewGuid().ToString(),
                            PartialPayment = (model.PaymentTransaction.Amount < viewModel.CartTotal),
                            //todo:
                            PaymentStatusId = 1, //1 = on hold, 2 = processing, 3 = processed, 4 = complete
                            PaymentType = paymentType,
                            PostedAmount = 0m

                        },
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone
                    }
                    //CartCode = model.CartCode,
                    //CartTotal = model.CartTotal,
                    //Address = model.Address,
                    //PaymentTransaction = model.PaymentTransaction,
                    //FullName = model.FullName,
                    //Email = model.Email,
                    //Phone = model.Phone,
                    //ShippingAddress = model.ShippingAddress
                };

                TempData["OrderInfo"] = m;
                return RedirectToAction("Index", "OrderConfirm");
            }
            return View(model);
        }
    }
}