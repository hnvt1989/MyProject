using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Core;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class AppSettingController : Controller
    {

        public ActionResult Index()
        {
            var appSettings = new List<AppSettingViewModel>();
            using (var context = new ShoppingCartContext())
            {
                context.AppSettings.ForEach(a => appSettings.Add(new AppSettingViewModel()
                {
                    Code = a.Code,
                    Value = a.Value,
                    ValueType = a.ValueType,
                    Id = a.Id,
                    Description = a.Description
                }));
            }
            return View(appSettings);
        }

        //public ActionResult EditAppSetting()
        //{
            
        //}
    }
}