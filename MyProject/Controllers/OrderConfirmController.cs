using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyProject.AppLogic.Communication;
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
        public async Task<ActionResult> Index(OrderConfirmViewModel model)
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
                    Phone = m.CheckOutInfo.Phone,
                    Email = m.CheckOutInfo.Email,
                    OrderDate = DateTime.Now,                
                    OrderDetails = new List<LineOrderDetail>()
                };

                //user who placed the order
                using (IdentityContext _idDb = new IdentityContext())
                {
                    var _currentUserId = User.Identity.GetUserId();
                    var _currentUser = _idDb.Users.FirstOrDefault(x => x.Id == _currentUserId);

                    if(_currentUser != null)
                        order.UserName = _currentUser.UserName;
                }

                if(User != null)
                    order.UserName = User.Identity.GetUserName();
                order.Guid = Guid.NewGuid().ToString();
                var orderNumber = ShoppingCart.GetCart(this).CreateOrder(order);
                m.OrderGuid = order.Guid;
                int i = await EmailSender.SendMail(orderNumber.ToString(), m);
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Consultant"))
                    {
                        return RedirectToAction("Index", "OrderSummary", new {orderNumber = orderNumber, guid = "", firstTime = true});
                    }
                }
                return RedirectToAction("Index", "OrderSummary", new { orderNumber = orderNumber, guid = order.Guid, firstTime = true });
            }

            //TempData["OrderInfo"] = m;
            return View();
        }
    }
}