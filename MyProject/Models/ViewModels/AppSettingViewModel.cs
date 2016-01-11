using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class AppSettingViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Code")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Value")]
        public string Value { get; set; }

        [Required]
        [DisplayName("Value Type")]
        public string ValueType { get; set; }
    }
}