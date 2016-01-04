﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyProject.DAL;
using MyProject.Models.Core;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class OrderConfirmController : Controller
    {
        public ActionResult Index()
        {
            OrderConfirmViewModel model = (OrderConfirmViewModel) TempData["OrderInfo"];

            TempData["OrderConfirm"] = model;
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(OrderConfirmViewModel model)
        {
            OrderConfirmViewModel m = (OrderConfirmViewModel)TempData["OrderConfirm"];

            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    OrderNumber = SeqHelper.Next("Order"),
                    FullName = m.CheckOutInfo.Name,
                    Address = m.CheckOutInfo.ShippingAddress,
                    PaymentTransaction = m.CheckOutInfo.PaymentTransaction,
                    Email = m.CheckOutInfo.Email,
                    OrderDate = DateTime.Now,                
                    OrderDetails = new List<LineOrderDetail>()
                };

                //user who placed the order
                using (IdentityContext _idDb = new IdentityContext())
                {
                    var _currentUserId = User.Identity.GetUserId();
                    var _currentUser = _idDb.Users.FirstOrDefault(x => x.Id == _currentUserId);
                    order.UserName = _currentUser.UserName;
                }


                order.UserName = User.Identity.GetUserName();
                var orderNumber = ShoppingCart.GetCart(this).CreateOrder(order);
                return RedirectToAction("Index", "OrderSummary", new {orderNumber = orderNumber});
            }

            //TempData["OrderInfo"] = m;
            return View();
        }
    }
}