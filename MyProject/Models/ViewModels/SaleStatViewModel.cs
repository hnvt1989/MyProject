using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;

namespace MyProject.Models.ViewModels
{
    public class SaleStatViewModel
    {

        public SelectList AvailPeriods
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var firstOrderDate = context.Orders.OrderBy(o => o.OrderDate).Take(1).Select(a => a.OrderDate).ToString();
                    var firstMonth = DateTime.Parse(firstOrderDate);

                    //return the list of the following 12 months
                    var list = new List<DateTime>();
                    var ret = new 
                    for (int i = 1; i <= 12; i++)
                    {
                        list.Add(firstMonth.AddMonths(i));
                    }
                    return new SelectList(list, );
                }
            }
        }
    }
}