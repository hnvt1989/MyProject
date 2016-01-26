using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class SaleQuickSummaryViewModel
    {
        public SaleQuickSummaryViewModel()
        {
            Criterias = new List<Criteria>()
            {
                new Criteria(){Code = "Amount", Description = "Highest $$"},
                new Criteria(){Code = "Item", Description = "Most items"}
            };
            NumberOfItemSold = 0;
            NumberOfOrderPlaced = 0;
            TotalSale = 0m;
            EstimatedProfit = 0m;

            TotalReceived = 0m;
            ActualProfit = 0m;
            Fee = 0m;

        }
        public int NumberOfOrderPlaced { get; set; }
        public int NumberOfItemSold { get; set; }

        public decimal TotalSale { get; set; }

        public decimal EstimatedProfit { get; set; }

        public decimal TotalReceived { get; set; }

        public decimal ActualProfit { get; set; }

        public decimal Fee { get; set; }

        public List<string> TopCustomers { get; set; }
        public List<Criteria> Criterias { get; set; } 
    }

    public class TopCustomer
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Age { get; set; }
        public List<TopProduct> Products { get; set; } 
    }

    public class TopProduct
    {
        public string ProductCode { get; set; }
        public string Category { get; set; }
    }

    public class Criteria
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}