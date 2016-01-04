using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class OrderSummaryController : Controller
    {
        ShoppingCartContext _soCartContext = new ShoppingCartContext();

        public ActionResult Index(long orderNumber)
        {
            var order = _soCartContext.Orders.SingleOrDefault(o => o.OrderNumber == orderNumber);

            if (order != null)
            {
                var model = new OrderSummaryViewModel()
                {
                    OrderNumber = order.OrderNumber,
                    Email = order.Email,
                    OrderDate = order.OrderDate,
                    PaymentTransaction = order.PaymentTransaction,
                    FullName = order.FullName,
                    ShippingAddress = order.Address,
                    Phone = order.Phone,
                    Total = order.Total
                };
                model.OrderDetails = (from lineItems in _soCartContext.LineOrderDetails
                                      where lineItems.OrderId == order.Id
                                      select lineItems).ToList();
                return View(model);
            }
            return View("OrderNotFound");
        }
    }
}