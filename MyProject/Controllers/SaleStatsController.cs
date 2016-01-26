using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class SaleStatsController: Controller
    {
        public ActionResult Index()
        {
            var ret = new SaleStatViewModel();

            return View(ret);
        }
    }
}