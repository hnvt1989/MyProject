using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class PromotionSettingViewModel
    {
        public PromotionSettingViewModel()
        {
            this.Promotions = new List<PromotionSettingLineItemViewModel>();
        }
        public int Id { get; set; }
        public string Code { get; set; }

        public List<PromotionSettingLineItemViewModel> Promotions { get; set; } 
    }

    public class PromotionSettingLineItemViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool Active { get; set; }

        public string PromotionLineItemExpression { get; set; }

        [DisplayName("Categories, coma separated")]
        public string Categories { get; set; }

        [DisplayName("Price types, coma separated")]
        public string PriceTypes { get; set; }

        [DisplayName("Items, coma separated")]
        public string Items { get; set; }

        [DisplayName("Percent discount (e.g 0.15 = 15%)")]
        public string PercentDiscount { get; set; }

        [DisplayName("Amount discount (e.g 100 = 100 discount)")]
        public string AmountDiscount { get; set; }

        [DisplayName("Number of items bought to qualify for promotion")]
        public string BuyItemCounts { get; set; }

        [DisplayName("The items bought to qualify for promotion")]
        public string BuyItemCodes { get; set; }

        [DisplayName("The awarded items")]
        public string GetItemCodes { get; set; }

        [DisplayName("How many awarded items")]
        public string GetItemCount { get; set; }
    }
}