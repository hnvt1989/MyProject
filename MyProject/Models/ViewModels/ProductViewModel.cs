﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Categories = new List<Category>();
        }
        public int Id { get; set; }

        [Display(Name = "Product Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Enter description")]
        public string Description { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Enter detail description")]
        public string DetailDescription { get; set; }

        [Required]
        [DisplayName("Purchased Price (the price we bought in the product)")]
        public decimal BuyInPrice { get; set; }

        public decimal Price { get; set; }

        public decimal ConversionRate
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var rate = context.AppSettings.SingleOrDefault(a => a.Code == "ConversionRate");
                    var ret = 1m;
                    if (rate != null)
                    {
                        ret = Convert.ToDecimal(rate.Value);
                    }
                    return ret;
                }
            }
        }

        public string PriceType { get; set; }

        public decimal ShippingCost { get; set; }

        [Display(Name = "Quantity on hand")]
        public int QuantityOnHand { get; set; }

        public bool FeatureProduct { get; set; }

        [AllowHtml]
        public string Notes { get; set; }

        [Display(Name = "Pound")]
        public decimal WeightPounds { get; set; }

        [Display(Name = "Ounce")]
        public decimal WeightOunce { get; set; }

        public byte[] Image { get; set; }

        public byte[] ImageAlt0 { get; set; }

        public byte[] ImageAlt1 { get; set; }

        public HttpPostedFileBase ProductImage { get; set; }

        public HttpPostedFileBase ProductImageAlt0 { get; set; }

        public HttpPostedFileBase ProductImageAlt1 { get; set; }

        public List<Category> Categories { get; set; }

        public string CategoriesString { get; set; } 
    }

    public class ProductViewLessDetail
    {
        public int Id { get; set; }

        [DisplayName("Mã số")]
        public string Code { get; set; }

        [DisplayName("Thông tin")]
        public string Description { get; set; }

        [Display(Name = "Đang bán")]
        public bool Active { get; set; }

        [DisplayName("Thông tin chi tiết")]
        public string DetailDescription { get; set; }

        [DisplayName("Giá rao bán")]
        public decimal Price { get; set; }

        [DisplayName("Giá mua vào")]
        public decimal BuyInPrice { get; set; }

        public decimal ShippingCost { get; set; }

        [Display(Name = "Số lượng đang có")]
        public int QuantityOnHand { get; set; }

        public byte[] Image { get; set; }

        public byte[] ImageAlt0 { get; set; }

        public byte[] ImageAlt1 { get; set; }   
    }
}