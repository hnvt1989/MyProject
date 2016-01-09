using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class ProductViewModel
    {

        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PriceType { get; set; }

        public int QuantityOnHand { get; set; }

        public bool FeatureProduct { get; set; }

        public decimal Weight { get; set; }

        public byte[] Image { get; set; }
    }
}