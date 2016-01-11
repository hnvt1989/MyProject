using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Core;
using MyProject.Models.ShoppingCart;
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

        public ActionResult EditAppSetting(int id)
        {
            TempData["AppSettingEditId"] = id;
            using (var context = new ShoppingCartContext())
            {
                var appSetting = new AppSettingViewModel();
                if (id == -1)
                {
                    appSetting.Value = "";
                    appSetting.Code = "";
                    appSetting.Description = "AppSetting description";
                    appSetting.ValueType = "decimal";
                }
                else
                {
                    var oCat = context.AppSettings.Single(c => c.Id == id);
                    if (oCat != null)
                    {
                        appSetting.Value = oCat.Value;
                        appSetting.Code = oCat.Code;
                        appSetting.Description = oCat.Description;
                        appSetting.ValueType = oCat.ValueType;
                    }
                }
                return View(appSetting);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditAppSetting(AppSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = (int)TempData["AppSettingEditId"];
                using (var context = new ShoppingCartContext())
                {
                    if (id > 0)
                    {
                        var appSeting = context.AppSettings.Single(c => c.Id == id);
                        appSeting.Description = model.Description;
                        appSeting.Code = model.Code;
                        appSeting.Value = model.Value;
                        appSeting.ValueType = model.ValueType;

                  
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var ret = new AppSetting
                        {
                            Code = model.Code,
                            Description = model.Description,
                            Value = model.Value,
                            ValueType = model.ValueType
                        };

                        context.AppSettings.Add(ret);

                        await context.SaveChangesAsync();
                    }
                }
            }
            return View();
        }
    }
}