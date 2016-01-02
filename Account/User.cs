using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Attribute;

namespace Account
{
    public class User
    {
        [Required]
        public string Code { get; set; }

        //the account this user belong to
        [Required]
        [Reference(typeof(Account))]
        public int Account { get; set; }

        [Required]
        public string Password { get; set; }

        public string PasswordEncoding { get; set; }

        public string Hint { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public bool MustChangePassword { get; set; }

        [Required]
        public int Culture { get; set; }
    }
}
