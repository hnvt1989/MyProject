using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;
using Address = MyProject.Models.ShoppingCart.Address;

namespace MyProject.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [DisplayName("Tên")]
        public string Name { get; set; }

        public string CartCode { get; set; }

        public decimal CartTotal { get; set; }

        public decimal PaymentAmount { get; set; }

        [Required]
        [DisplayName("Số phone")]
        public string Phone { get; set; }

        [Required]
        [DisplayName("Địa chỉ Email")]
        public string Email { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public PaymentTransaction PaymentTransaction { get; set; }

        //public List<PaymentType> PaymentTypes { get; set; } 

        public SelectList PaymentTypesList {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    return new SelectList(context.PaymentTypes.ToList().Select(c => c.Description), "Bank");
                };
                
            }
        }
        }
}