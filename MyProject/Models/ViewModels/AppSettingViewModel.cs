using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class AppSettingViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Value")]
        public string Value { get; set; }

        [DisplayName("Value Type")]
        public string ValueType { get; set; }
    }
}