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
            var date = DateTime.Parse(period);
            var start = new DateTime(date.Year, date.Month, 1);
            var end = start.AddMonths(1);
            var ret = new SaleQuickSummaryViewModel();

            using (var context = new ShoppingCartContext())
            {
                var orderPlaced = context.Orders.Where(o => o.OrderDate <= end && o.OrderDate >= start).Select(o => o.Id).ToList();
                ret.NumberOfOrderPlaced = orderPlaced.Count;
                if (ret.NumberOfOrderPlaced > 0)
                {
                    context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                        .ForEach(s => ret.NumberOfItemSold += s.Quantity);
                    context.LineOrderDetails.Where(l => orderPlaced.Contains(l.OrderId))
                        .ForEach(s =>
                        {
                            ret.EstimatedProfit += s.Profit;
                            ret.TotalSale += s.Total;
                            ret.Fee += s.ShippingCost;
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
                    ret.ActualProfit = ret.EstimatedProfit + (ret.TotalReceived - ret.TotalSale - ret.Fee);

                    //top customers

                }
            }
            
            return Json(ret);
        }
    }
}