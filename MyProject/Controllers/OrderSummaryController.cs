using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.AppLogic.Communication;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class OrderSummaryController : Controller
    {
        ShoppingCartContext _soCartContext = new ShoppingCartContext();

        public async Task<ActionResult> Index(long orderNumber)
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
                    ShippingCost = order.ShippingCost,
                    FullName = order.FullName,
                    ShippingAddress = order.Address,
                    Phone = order.Phone,
                    Total = order.Total
                };
                model.OrderDetails = (from lineItems in _soCartContext.LineOrderDetails
                                      where lineItems.OrderId == order.Id
                                      select lineItems).ToList();


                ////payment transaction detail:
                //using (var context = new ShoppingCartContext())
                //{
                //    model.PaymentTransaction.PaymentType =  context.PaymentTypes.SingleOrDefault(t => t.Id == model.PaymentTransaction.PaymentTypeId);
                //}
                await EmailSender.Send(model);

                return View(model);
            }
            return View("OrderNotFound");
        }
    }
}