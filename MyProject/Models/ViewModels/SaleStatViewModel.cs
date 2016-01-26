using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;

namespace MyProject.Models.ViewModels
{
    public class SaleStatViewModel
    {
        [DisplayName("Select calendar period:")]
        public SelectList AvailPeriods
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var firstOrderDate = context.Orders.OrderBy(o => o.OrderDate).Take(1).Select(a => a.OrderDate).FirstOrDefault();
                    var firstMonth = firstOrderDate;

                    //return the list of the following 12 months
                    var ret = new Dictionary<string, string>();
                    ret.Add("Default", "Select a period");
                    for (int i = 0; i < 12; i++)
                    {
                        var next = firstMonth.AddMonths(i).ToString("MM/yyyy");
                        ret.Add(next,next);
                    }
                    return new SelectList(ret, "Key", "Value", ret.ToList()[0].Key);
                }
            }
        }

        public string SelectedPeriod { get; set; }

        public SaleQuickSummaryViewModel QuickSummary { get; set; }
    }
}