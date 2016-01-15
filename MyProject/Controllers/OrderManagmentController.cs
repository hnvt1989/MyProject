using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class OrderManagmentController : Controller
    {
        public ActionResult Index()
        {
            var ret = new List<OrderQuickSummaryViewModel>();

            using (var context = new ShoppingCartContext())
            {
                context.Orders.ForEach( o => ret.Add(new OrderQuickSummaryViewModel()
                {
                    Code = o.OrderNumber.ToString(),
                    OrderDate = o.OrderDate.ToString("dd-MM-yyyy"),
                    CustomerName = o.FullName,
                    OrderTotal = o.Total,
                    Id = o.Id
                    //PostedAmount = o.PaymentTransaction.PostedAmount
                }));
            }
            return View(ret);
        }

        [HttpGet]
        public ActionResult ViewOrder(int id)
        {
            var ret = new OrderDetailSummaryViewModel();
            using (var context = new ShoppingCartContext())
            {
                var order = context.Orders.Single(o => o.Id == id);

                ret.ActualSoldAmount = order.ActualSoldAmount;
                ret.Email = order.Email;
                ret.FullName = order.FullName;
                ret.OrderDate = order.OrderDate;

                ret.OrderDetails = context.LineOrderDetails.Where(l => l.OrderId == id);

                ret.OrderNumber = order.OrderNumber;
                ret.PaymentTransaction = order.PaymentTransaction;
                ret.ShippingAddress = context.Addresses.Single(a => a.Id == order.ShippingAddressId);
            }

           

            return View(ret);
        }
    }
}