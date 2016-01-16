﻿using System;
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
                context.Orders.ForEach(o => ret.Add(new OrderQuickSummaryViewModel()
                {
                    Code = o.OrderNumber.ToString(),
                    OrderDate = o.OrderDate.ToString("dd-MM-yyyy"),
                    CustomerName = o.FullName,
                    OrderTotal = o.Total,
                    Id = o.Id,
                }));

            }

            using (var context = new ShoppingCartContext())
            {
                foreach (var r in ret)
                {
                    r.PostedAmount = context.Orders.Single(o => o.Id == r.Id).PaymentTransaction.Amount;
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
            TempData["OrderId"] = order.Id;

            return View(ret);
        }

        [HttpPost]
        public async Task<ActionResult> EditOrder(OrderDetailSummaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string paymentStatus = Request.Form["paymentStatus"].ToString();
                string paymentType = Request.Form["paymentType"].ToString();

                var order = _context.Orders.Single(o => o.OrderNumber == model.OrderNumber);
                var id = TempData["OrderId"];

                order.PaymentTransaction.Amount = model.PaymentTransaction.Amount;
                order.PaymentTransaction.PaymentStatusId = _context.PaymentStatuses.Single(p => p.Description == paymentStatus).Id;

                order.PaymentTransaction.PaymentTypeId =
                    _context.PaymentTypes.Single(p => p.Description == paymentType).Id;

                await _context.SaveChangesAsync();
                return RedirectToAction("ViewOrder", new { id });
            }

            return View();

        }
    }
}