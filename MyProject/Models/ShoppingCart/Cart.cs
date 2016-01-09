using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class Cart
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        //public string ProductCode { get; set; }

        public string PriceType { get; set; }

        public List<string> Categories { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        //public virtual ProductOffer ProductOffer { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal ShippingCost { get; set; }

        //price after discount
        public decimal DiscountedPrice
        {
            get { return decimal.Round((OriginalPrice * Quantity) - TotalDiscountAmount, 2, MidpointRounding.AwayFromZero); }
        }

        [NotMapped] public decimal _netBeforeDiscount;

        public decimal NetBeforeDiscount 
        {
            get
            {
                _netBeforeDiscount = decimal.Round(OriginalPrice*Quantity, 2, MidpointRounding.AwayFromZero);
                //_netBeforeDiscount =  OriginalPrice*Quantity;
                return _netBeforeDiscount;
            }
            set { _netBeforeDiscount = value; }
        }

        [NotMapped] public decimal _totalDiscountAmount;

        public decimal TotalDiscountAmount
        {
            get
            {
                _totalDiscountAmount = decimal.Round(DiscountAmount * Quantity, 2, MidpointRounding.AwayFromZero);
                //_totalDiscountAmount = DiscountAmount*Quantity;
                return _totalDiscountAmount;
            }
            set { _totalDiscountAmount = value; }
        }

        [NotMapped] public decimal _sum;

        public decimal Sum
        {
            get
            {
                _sum = decimal.Round(DiscountedPrice + ShippingCost, 2, MidpointRounding.AwayFromZero);
                //_sum = DiscountedPrice  + ShippingCost;
                return _sum;
            }
            set { _sum = value; }
        }

        public bool DiscountApplied { get; set; }

        public bool AddOnItem { get; set; }

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //public string Code { get; set; }

        //[ForeignKey("Product")]
        //public int ProductId { get; set; }

        //public int Count { get; set; }

        //public decimal ShippingCost { get; set; }

        //public System.DateTime DateCreated { get; set; }

        //public virtual Product Product { get; set; }
        
    }
}