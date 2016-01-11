using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        [Display(Name = "Category Description")]
        public string Description { get; set; }

        public bool IsChecked { get; set; }
    }
}