using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                var count = context.Orders.Count();
                //take last 200 order
                context.Orders.OrderBy(o => o.OrderDate).Skip(Math.Max(0, count - 200)).ForEach(o => ret.Add(new OrderQuickSummaryViewModel()
                {
                    Code = o.OrderNumber.ToString(),
                    OrderDate = o.OrderDate.ToString("dd-MM-yyyy"),
                    CustomerName = o.FullName,
                    OrderTotal = o.Total,
                    Id = o.Id,
                    OrderStatusId =  o.OrderStatusId
                }));

            }

            using (var context = new ShoppingCartContext())
            {
                foreach (var r in ret)
                {
                    r.PostedAmount = context.Orders.Single(o => o.Id == r.Id).PaymentTransaction.Amount;
                    if (r.OrderStatusId > 0)
                    {
                        r.OrderStatus = context.OrderStatuses.Single(s => s.Id == r.OrderStatusId).Description;
                    }
                    else
                    {
                        r.OrderStatus = "Unknown";
                    }

                }
            }

            return View(ret);
        }

        //[HttpPost]
        //public ActionResult Index([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParameters)
        //{
        //    var totalCount = myDbContext.Set<Something>().Count();
        //    var filteredDataSet = myDbContext.Set<Something>().Where(_s => _s.ToLower().Contains(requestParameters.Search.Value));

        //    foreach(var column in requestParameters.Columns.GetFilteredColumns())
        //    {
        //        // Apply individual filters to each column.
        //        // You can try Dynamic Linq to help here or you can use if statements.

        //        // DynamicLinq will be slower but code will be cleaner.
        //    }

        //    var isSorted = false;
        //    IOrderedEnumerable<Something> ordered = null;
        //    foreach(var column in requestParameters.Columns.GetSortedColumns())
        //    {
        //        // If you choose to use Dynamic Linq, you can apply all sorting at once.
        //        // If not, you have to apply each sort manually, as follows.

        //        if (!isSorted)
        //        {
        //            // Apply first sort.
        //            if (column.SortDirection == Column.SortDirection.Ascendant)
        //                ordered.OrderBy(...);
        //            else
        //                ordered.OrderByDescending(...);

        //            isSorted = true;
        //        }
        //        else
        //        {
        //            if (column.SortDirection == Column.SortDirection.Ascendant)
        //                ordered.ThanBy(...);
        //            else
        //                ordered.ThanByDescending(...);
        //        }
        //    }

        //    var pagedData = ordered.Skip(requestParameters.Start).Take(requestParameters.Length);

        //    var dataTablesResult = new DataTablesResult(
        //        requestParameters.Draw,
        //        pagedData,
        //        filteredDataSet.Count(),
        //        totalCount
        //    );

        //    return View(dataTablesResult);
        //}

        [HttpGet]
        public ActionResult ViewOrder(int id)
        {
            var ret = new OrderDetailSummaryViewModel();
            var lineItems = new List<LineOrderDetail>();

            //using (var context = new ShoppingCartContext())
            //{
            var order = _context.Orders.Single(o => o.Id == id);

            //ret.ActualSoldAmount = order.ActualSoldAmount;
            ret.Email = order.Email;
            ret.FullName = order.FullName;
            ret.OrderDate = order.OrderDate;
            ret.OrderStatusId = order.OrderStatusId;
            ret.OrderDetails = _context.LineOrderDetails.Where(p => p.OrderId == id).ToList();
            ret.TotalProfit = order.Profit;
            ret.Commission = order.Commission;
            ret.TrueProfit = order.TrueProfit;
            ret.Discount = order.Discount;

            var shipment = _context.Shipments.SingleOrDefault(s => s.Id == order.ShipmentId);
            ret.ShipmentCode = string.Empty;
            if (shipment != null)
                ret.ShipmentCode = shipment.Code;


            if (order.FullName.Contains("Hiển Nguyễn") || order.FullName.Contains("Hien Nguyen") ||
                order.FullName.Contains("Nguyen Hien") || order.FullName.Contains("Nguyễn Hiển"))
            {
                if(ret.Commission == 0)
                    ret.Commission = Math.Round((ret.TotalProfit / 2) / 1000, 0, MidpointRounding.AwayFromZero) * 1000;
                if(ret.TrueProfit == 0)
                    ret.TrueProfit = ret.TotalProfit - ret.Commission;
            }

            if (ret.OrderStatusId > 0)
            {
                ret.OrderStatus = _context.OrderStatuses.Single(s => s.Id == ret.OrderStatusId).Description;
            }
            else
            {
                ret.OrderStatus = "Unknown";
            }
            ret.OrderStatusNote = order.Notes;

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

        [HttpGet]
        public ActionResult ViewInvoice(long orderNumber)
        {
            var ret = new OrderDetailSummaryViewModel();
            var lineItems = new List<LineOrderDetail>();

            //using (var context = new ShoppingCartContext())
            //{
            var order = _context.Orders.Single(o => o.OrderNumber == orderNumber);

            //ret.ActualSoldAmount = order.ActualSoldAmount;
            ret.Email = order.Email;
            ret.FullName = order.FullName;
            ret.OrderDate = order.OrderDate;
            ret.OrderStatusId = order.OrderStatusId;
            ret.OrderDetails = _context.LineOrderDetails.Where(p => p.OrderId== order.Id).ToList();
            ret.TotalProfit = order.Profit;
            ret.Commission = order.Commission;
            ret.TrueProfit = order.TrueProfit;
            ret.Discount = order.Discount;

            var shipment = _context.Shipments.SingleOrDefault(s => s.Id == order.ShipmentId);
            ret.ShipmentCode = string.Empty;
            if (shipment != null)
                ret.ShipmentCode = shipment.Code;


            if (order.FullName.Contains("Hiển Nguyễn") || order.FullName.Contains("Hien Nguyen") ||
                order.FullName.Contains("Nguyen Hien") || order.FullName.Contains("Nguyễn Hiển"))
            {
                if (ret.Commission == 0)
                    ret.Commission = Math.Round((ret.TotalProfit / 2) / 1000, 0, MidpointRounding.AwayFromZero) * 1000;
                if (ret.TrueProfit == 0)
                    ret.TrueProfit = ret.TotalProfit - ret.Commission;
            }

            if (ret.OrderStatusId > 0)
            {
                ret.OrderStatus = _context.OrderStatuses.Single(s => s.Id == ret.OrderStatusId).Description;
            }
            else
            {
                ret.OrderStatus = "Unknown";
            }
            ret.OrderStatusNote = order.Notes;

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


        [HttpGet]
        public ActionResult EditOrder(long number)
        {
            var ret = new OrderDetailSummaryViewModel();
            var lineItems = new List<LineOrderDetail>();

            //using (var context = new ShoppingCartContext())
            //{
            var order = _context.Orders.Single(o => o.OrderNumber == number);

            //ret.ActualSoldAmount = order.ActualSoldAmount;

            ret.Email = order.Email;
            ret.FullName = order.FullName;
            ret.OrderDate = order.OrderDate;
            ret.OrderDetails = _context.LineOrderDetails.Where(p => p.OrderId == order.Id).ToList();

            ret.OrderStatusId = order.OrderStatusId;

            if (ret.OrderStatusId > 0)
            {
                ret.OrderStatus = _context.OrderStatuses.Single(s => s.Id == ret.OrderStatusId).Description;
            }
            else
            {
                ret.OrderStatus = "Unknown";
            }
            ret.OrderStatusNote = order.Notes;

            
            ret.ShippingCost = order.ShippingCost;
            ret.PaymentTransaction = order.PaymentTransaction;


            ret.TotalProfit = order.Profit;
            ret.Commission = order.Commission;
            ret.TrueProfit = order.TrueProfit;

            ret.Discount = order.Discount;
            ret.Total = order.Total;
            ret.TotalBeforeDiscount = order.Total + order.Discount;

            if (order.FullName.Contains("Hiển Nguyễn") || order.FullName.Contains("Hien Nguyen") ||
                order.FullName.Contains("Nguyen Hien") || order.FullName.Contains("Nguyễn Hiển"))
            {
                if (ret.Commission == 0)
                    ret.Commission = Math.Round((ret.TotalProfit / 2) / 1000, 0, MidpointRounding.AwayFromZero) * 1000;
                if (ret.TrueProfit == 0)
                    ret.TrueProfit = ret.TotalProfit - ret.Commission;
            }

            ret.OrderNumber = order.OrderNumber;
            ret.PaymentTransaction = order.PaymentTransaction;
            ret.ShippingAddress = _context.Addresses.Single(a => a.Id == order.ShippingAddressId);
            //}           
            //TempData["OrderId"] = order.Id;

            return View(ret);
        }

        [HttpPost]
        public async Task<ActionResult> EditOrder(OrderDetailSummaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string paymentStatus = Request.Form["paymentStatus"].ToString();
                string paymentType = Request.Form["paymentType"].ToString();
                string orderStatus = Request.Form["orderStatus"].ToString();

                var order = _context.Orders.Single(o => o.OrderNumber == model.OrderNumber);
                //var id = TempData["OrderId"];
                //var id = model.OrderId;

                order.PaymentTransaction.Amount = model.PaymentTransaction.Amount;
                order.PostedAmount = model.PaymentTransaction.Amount;
                order.PaymentTransaction.PaymentStatusId = _context.PaymentStatuses.Single(p => p.Description == paymentStatus).Id;
                //order.TrueProfit = model.TrueProfit - (model.Discount/2);
                //order.Commission = model.Commission - (model.Discount/2);
                order.Profit -= model.Discount - order.Discount;

                order.Total = order.Total + order.Discount - model.Discount;

                order.Discount = model.Discount;
                

                order.PaymentTransaction.PaymentTypeId =
                    _context.PaymentTypes.Single(p => p.Description == paymentType).Id;

                order.OrderStatusId = _context.OrderStatuses.Single(s => s.Description == orderStatus).Id;
                order.Notes = model.OrderStatusNote;

                await _context.SaveChangesAsync();
                return RedirectToAction("ViewOrder", new { order.Id });
            }

            return View();

        }
    }
}