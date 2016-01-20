using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MyProject.AppLogic.Communication;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class OrderSummaryController : Controller
    {
        ShoppingCartContext _soCartContext = new ShoppingCartContext();

        //[Authorize(Roles = "Consultant, Admin")]
        public ActionResult Index(long orderNumber, string guid, bool firstTime = false)
        {
            Order order = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Consultant"))
                {
                    if (guid.IsNullOrWhiteSpace())
                    {
                        order = _soCartContext.Orders.SingleOrDefault(o => o.OrderNumber == orderNumber);
                    }
                    else
                    {
                        order = _soCartContext.Orders.SingleOrDefault(o => o.OrderNumber == orderNumber && o.Guid == guid);
                    }
                }
            }
            else
            {
                order = _soCartContext.Orders.SingleOrDefault(o => o.OrderNumber == orderNumber && o.Guid == guid);
            }
            

            if (order != null)
            {
                var model = new OrderSummaryViewModel()
                {
                    OrderNumber = order.OrderNumber,
                    Email = order.Email,
                    OrderDate = order.OrderDate,
                    PaymentTransaction = order.PaymentTransaction,
                    ShippingCost = order.ShippingCost,
                    FullName = order.FullName,
                    ShippingAddress = order.Address,
                    Phone = order.Phone,
                    Total = order.Total,
                    FirstTime = firstTime
                };
                model.OrderDetails = (from lineItems in _soCartContext.LineOrderDetails
                                      where lineItems.OrderId == order.Id
                                      select lineItems).ToList();

                var paymentInfoObj =
                    _soCartContext.Contents.FirstOrDefault(c => c.TextLocation == "Summary.Order.PaymentInfo");
                var paymentInfo = "";

                if (paymentInfoObj != null)
                {
                    paymentInfo = paymentInfoObj.TextValue;
                }
                model.PaymentInfo = paymentInfo;
                ////payment transaction detail:
                //using (var context = new ShoppingCartContext())
                //{
                //    model.PaymentTransaction.PaymentType =  context.PaymentTypes.SingleOrDefault(t => t.Id == model.PaymentTransaction.PaymentTypeId);
                //}
                //todo: this shouldnt' be here, should be right after we created an order
                //await EmailSender.Send(model);

                return View(model);
            }
            return View("OrderNotFound");
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> Index(long orderNumber, string guid)
        //{
        //    var order = _soCartContext.Orders.SingleOrDefault(o => o.OrderNumber == orderNumber && o.Guid == guid);

        //    if (order != null)
        //    {
        //        var model = new OrderSummaryViewModel()
        //        {
        //            OrderNumber = order.OrderNumber,
        //            Email = order.Email,
        //            OrderDate = order.OrderDate,
        //            PaymentTransaction = order.PaymentTransaction,
        //            ShippingCost = order.ShippingCost,
        //            FullName = order.FullName,
        //            ShippingAddress = order.Address,
        //            Phone = order.Phone,
        //            Total = order.Total
        //        };
        //        model.OrderDetails = (from lineItems in _soCartContext.LineOrderDetails
        //                              where lineItems.OrderId == order.Id
        //                              select lineItems).ToList();

        //        var paymentInfoObj =
        //            _soCartContext.Contents.FirstOrDefault(c => c.TextLocation == "Summary.Order.PaymentInfo");
        //        var paymentInfo = "";

        //        if (paymentInfoObj != null)
        //        {
        //            paymentInfo = paymentInfoObj.TextValue;
        //        }
        //        model.PaymentInfo = paymentInfo;
        //        ////payment transaction detail:
        //        //using (var context = new ShoppingCartContext())
        //        //{
        //        //    model.PaymentTransaction.PaymentType =  context.PaymentTypes.SingleOrDefault(t => t.Id == model.PaymentTransaction.PaymentTypeId);
        //        //}
        //        //todo: this shouldnt' be here, should be right after we created an order
        //        //await EmailSender.Send(model);

        //        return View(model);
        //    }
        //    return View("OrderNotFound");
        //}
    }
}