using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MyProject.DAL;
using MyProject.Models.ShoppingCart;

namespace MyProject.Models.Account
{
    public class AccountAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        //public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Address Address { get; set; }
    }

    public class AddressFlow
    {
        public static void AddNewAddress(string userName, Address address)
        {
            string userId;
            using (IdentityContext _idDb = new IdentityContext())
            {
                userId = _idDb.Users.FirstOrDefault(x => x.UserName == userName).Id;
            }

            using (var context = new ShoppingCartContext())
            {
                //context.Addresses.Add(address);
                //context.SaveChanges();

                context.AccountAddresses.Add(new AccountAddress()
                {
                    Address = address,
                    UserId = userId
                });

                context.SaveChanges();
            }
        }
    }
}