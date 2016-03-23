using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Core;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class ShipmentController : Controller
    {
        //
        // GET: /Shipment/
        public ActionResult Index()
        {
            using (var context = new ShoppingCartContext())
            {
                var shipments = new List<ShipmentViewModel>();
                context.Shipments.ForEach(s => shipments.Add(new ShipmentViewModel()
                {
                    Id = s.Id,
                    Code = s.Code,
                    ShipDate = s.ShipDate.ToShortDateString(),
                    Recipient = s.Recipient,
                }));

                return View(shipments);
            }
            
        }

        public ActionResult ViewShipment(string code)
        {
            using (var context = new ShoppingCartContext())
            {
                var shipment = context.Shipments.SingleOrDefault(s => s.Code.Equals(code));

                var ret = new ShipmentViewModel();
                ret.Orders = new List<ShipmentOrderViewModel>();
                ret.ShipDate = "";
                ret.Recipient = "";
               
                if (shipment != null)
                {
                    ret.ShipDate = shipment.ShipDate.ToShortDateString();
                    ret.Recipient = shipment.Recipient;
                    ret.Code = shipment.Code;

                    context.Orders.Where(o => o.ShipmentId == shipment.Id).ForEach(or =>
                        ret.Orders.Add(new ShipmentOrderViewModel()
                        {
                            CustomerName = or.FullName,
                            OrderNumber = or.OrderNumber,
                            OrderTotal = or.Total,
                            Id = or.Id
                        }));

                }
                return View(ret);
            }
            
        }

        [HttpGet]
        public ActionResult EditShipment(int id)
        {
            var ret = new ShipmentViewModel();
            if (id == 0)
            {
                ret.Id = id;
                ret.Code = "";
                ret.Recipient = "";
                ret.OrdersId = new List<int>();
                ret.OrderIdsString = "";
                ret.ShipDate = "0/0/0000";
            }
            return View(ret);
        }

        [HttpPost]
        public async Task<ActionResult> EditShipment(ShipmentViewModel model)
        {
            using (var context = new ShoppingCartContext())
            {
                //new item
                if (model.Id == 0)
                {
                    var shipment = new Shipment
                    {
                        Code = model.Code,
                        Recipient = model.Recipient,
                        ShipDate = DateTime.Parse(model.ShipDate),
                    };

                    context.Shipments.Add(shipment);
                    await context.SaveChangesAsync();

                    var orderLists = model.OrderIdsString.Split(',');

                    context.Orders.Where(o => orderLists.Contains(o.OrderNumber.ToString())).ForEach(order =>
                    {
                        order.ShipmentId = shipment.Id;
                    });

                    
                }

                await context.SaveChangesAsync();
            }
            return View();
        }
	}
}