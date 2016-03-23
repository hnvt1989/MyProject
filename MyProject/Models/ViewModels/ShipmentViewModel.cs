using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class ShipmentViewModel
    {
        public int Id { get; set; }

        [DisplayName("Shipment Code")]
        public string Code { get; set; }

        [DisplayName("Ship Date")]
        public string ShipDate { get; set; }

        public string Recipient { get; set; }

        [DisplayName("List of Orders Id, coma separated")]
        public string OrderIdsString { get; set; }
        public List<int> OrdersId { get; set; } 
        public List<ShipmentOrderViewModel> Orders { get; set; } 
    }

    public class ShipmentOrderViewModel
    {
        public int Id { get; set; }
        public long OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal OrderTotal { get; set; }
    }
}