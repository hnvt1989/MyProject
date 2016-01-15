using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyProject.Models.ShoppingCart
{
    public class PaymentTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [Required]
        //[RegularExpression(@"N0", ErrorMessage = "Must be a number")]
        //[RegularExpression(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$", ErrorMessage = "Must be a number")]
        //[RegularExpression(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*$", ErrorMessage = "Must be a number")]
        [Display(Name = "Tổng số tiền")]
        public decimal Amount { get; set; }

        //public decimal PostedAmount { get; set; }

        [ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }

        public bool PartialPayment { get; set; }


        [ForeignKey("PaymentStatus")]
        public int PaymentStatusId { get; set; }

        [Display(Name = "Cách thanh toán tiền")]
        public virtual PaymentType PaymentType { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }

        public string PaymentTypeDescription { get; set; }
    }

    public class PaymentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

    }

    public class PaymentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}