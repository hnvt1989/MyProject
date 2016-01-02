using System.ComponentModel.DataAnnotations;
using Core.Attribute;

namespace Account
{

    public class Account
    {
        public Account() { }

        [Required]
        public string Code { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }

        //Full name
        public string Name { get; set; }
        public string Email { get; set; }

        [Reference(typeof(AccountType))]
        public int AccountType { get; set; }
    }
}