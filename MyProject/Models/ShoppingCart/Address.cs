using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace MyProject.Models.ShoppingCart
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        public string Line1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Line2 { get; set; }

        [Display(Name = "Address line 3")]
        public string Line3 { get; set; }

        [ForeignKey("AddressType")]
        public int AddressTypeId { get; set; }

        public virtual AddressType AddressType { get; set; }

        public bool Primary { get; set; }
    }

    public class AddressType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}