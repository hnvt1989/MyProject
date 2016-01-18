using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels
{
    public class CategoryManagementViewModel
    {
        public CategoryManagementViewModel()
        {
            CategoryViewModels = new List<CategoryViewModel>();
        }
        public List<CategoryViewModel> CategoryViewModels { get; set; }

    }
}