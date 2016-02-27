using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class QuickFinanceStatisticsViewModel
    {
        public QuickFinanceStatisticsViewModel()
        {
            TotalSale = 0m;
            Revenue = 0m;
            Expense = 0m;
            EstimatedProfit = 0m;
            Bank = 0m;
            Fee = 0m;
        }
        public decimal TotalSale { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expense { get; set; }
        public decimal EstimatedProfit { get; set; }
        public decimal Bank { get; set; }
        public decimal Fee { get; set; }
    }
}