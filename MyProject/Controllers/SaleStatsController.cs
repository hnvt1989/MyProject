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
    public class SaleStatsController: Controller
    {
        public ActionResult Index()
        {
            var ret = new SaleStatViewModel();
            return View(ret);
        }

        [HttpPost]
        public ActionResult QuickSummaryByPeriod(string period)
        {
            var date = new DateTime();
            var start = new DateTime();
            var end = new DateTime();

            if (period != "All")
            {
                date = DateTime.Parse(period);
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = DateTime.MinValue;
                end = DateTime.Now.AddDays(1); //time zone different between local and hosted web
            }

            var ret = new SaleQuickSummaryViewModel();

            using (var context = new ShoppingCartContext())
            {
                var orderPlaced = context.Orders.Where(o => o.OrderDate <= end && o.OrderDate >= start && o.OrderStatusId != 7).Select(o => o.Id).ToList();
                ret.NumberOfOrderPlaced = orderPlaced.Count;
                if (ret.NumberOfOrderPlaced > 0)
                {
                    context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                        .ForEach(s => ret.NumberOfItemSold += s.Quantity);
                    //context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                    //    .ForEach(s =>
                    //    {
                    //        ret.EstimatedProfit += s.Profit;
                    //        ret.TotalSale += s.Total;
                    //        ret.Fee += s.ShippingCost;
                    //    });
                    context.Orders.Where(order => orderPlaced.Contains(order.Id)).
                        ForEach(o =>
                    {
                        ret.EstimatedProfit += o.Profit;
                        ret.TotalSale += (o.Total - o.ShippingCost);
                        ret.Fee += o.ShippingCost;
                        ret.Commission += o.Commission;
                    });
                    var currencyConversionRate =
                        Decimal.Parse(
                            context.AppSettings.Where(a => a.Code == "ConversionRate").Select(ab => ab.Value).First());
                    ret.EstimatedProfit = Math.Round(ret.EstimatedProfit/currencyConversionRate, 2,
                        MidpointRounding.AwayFromZero);

                    ret.TotalSale = Math.Round(ret.TotalSale/currencyConversionRate, 2, MidpointRounding.AwayFromZero);
                    ret.Fee = Math.Round(ret.Fee/currencyConversionRate, 2, MidpointRounding.AwayFromZero);

                    var totalPosted =
                        context.Orders.Where(o => o.OrderDate <= end && o.OrderDate >= start)
                            .Select(oa => oa.PostedAmount)
                            .Sum();
                    if (totalPosted > 0)
                        ret.TotalReceived = Math.Round(totalPosted/currencyConversionRate, 2,
                            MidpointRounding.AwayFromZero);
                    ret.ActualProfit = ret.EstimatedProfit + (ret.TotalReceived - ret.TotalSale - ret.Fee - ret.Commission);

                    //top customers

                }
            }
            
            return Json(ret);
        }

        [HttpPost]
        public ActionResult QuickFinanceStatistic(string period)
        {
            var date = new DateTime();
            var start = new DateTime();
            var end = new DateTime();

            if (period != "All")
            {
                date = DateTime.Parse(period);
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = DateTime.MinValue;
                end = DateTime.Now.AddDays(1); //time zone different between local and hosted web
            }
            
            var ret = new QuickFinanceStatisticsViewModel();

            using (var context = new ShoppingCartContext())
            {
                var orderPlaced = context.Orders.Where(o => o.OrderDate <= end && o.OrderDate >= start && o.OrderStatusId != 7).Select(o => o.Id).ToList();
                //ret.NumberOfOrderPlaced = orderPlaced.Count;
                if (orderPlaced.Count > 0)
                {
                    //context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                    //    .ForEach(s => ret.NumberOfItemSold += s.Quantity);
                    //context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                    //    .ForEach(s =>
                    //    {
                    //        //ret.EstimatedProfit += s.Profit;
                    //        ret.TotalSale += s.Total;
                    //        ret.Fee += s.ShippingCost;
                    //    });

                    context.Orders.Where(order => orderPlaced.Contains(order.Id)).
                        ForEach(o =>
                        {
                            //ret.EstimatedProfit += o.Profit;
                            ret.TotalSale += (o.Total - o.ShippingCost - o.Commission);
                            ret.Fee += o.ShippingCost;
                        });

                    var currencyConversionRate =
                        Decimal.Parse(
                            context.AppSettings.Where(a => a.Code == "ConversionRate").Select(ab => ab.Value).First());
                    //ret.EstimatedProfit = Math.Round(ret.EstimatedProfit / currencyConversionRate, 2,
                    //    MidpointRounding.AwayFromZero);

                    ret.Revenue = Math.Round(ret.Revenue / currencyConversionRate, 2, MidpointRounding.AwayFromZero);
                    ret.Fee = Math.Round(ret.Fee / currencyConversionRate, 2, MidpointRounding.AwayFromZero);

                    var totalPosted =
                        context.Orders.Where(o => o.OrderDate <= end && o.OrderDate >= start)
                            .Select(oa => oa.PostedAmount)
                            .Sum();
                    if (totalPosted > 0)
                        ret.Revenue = Math.Round(totalPosted / currencyConversionRate, 2,
                            MidpointRounding.AwayFromZero);
                    
                    
                    if(ret.TotalSale > 0)
                        ret.TotalSale = Math.Round(ret.TotalSale / currencyConversionRate, 2,
                            MidpointRounding.AwayFromZero);
                    if(ret.Expense > 0)
                        ret.Expense = Math.Round(ret.Expense / currencyConversionRate, 2,
                            MidpointRounding.AwayFromZero);
                    ret.Bank = ret.Revenue - ret.Expense;
                }
            }

            return Json(ret);
        }

    }
}