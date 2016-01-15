using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class OrderManagmentController : Controller
    {
        ShoppingCartContext _context = new ShoppingCartContext();

        public ActionResult Index()
        {
            var ret = new List<OrderQuickSummaryViewModel>();

            using (var context = new ShoppingCartContext())
            {
                context.Orders.ForEach(o => ret.Add(new OrderQuickSummaryViewModel()
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
            var lineItems = new List<LineOrderDetail>();

            //using (var context = new ShoppingCartContext())
            //{
            var order = _context.Orders.Single(o => o.Id == id);

            ret.ActualSoldAmount = order.ActualSoldAmount;
            ret.Email = order.Email;
            ret.FullName = order.FullName;
            ret.OrderDate = order.OrderDate;

            ret.OrderDetails = _context.LineOrderDetails.Where(p => p.OrderId == id).ToList();
            //context.LineOrderDetails.Where(o => o.OrderId == id).ForEach(p => ret.OrderDetails.Add(new LineOrderDetail()
            //{
            //    Product = p.Product,
            //    UnitPrice = p.UnitPrice,
            //    Quantity = p.Quantity,
            //    Total = p.Total
            //}));
            ret.Total = order.Total;
            ret.ShippingCost = order.ShippingCost;
            ret.PaymentTransaction = order.PaymentTransaction;

            ret.OrderNumber = order.OrderNumber;
            ret.PaymentTransaction = order.PaymentTransaction;
            ret.ShippingAddress = _context.Addresses.Single(a => a.Id == order.ShippingAddressId);
            //}           

            return View(ret);
        }
    }
}