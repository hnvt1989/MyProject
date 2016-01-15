using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class PromotionSettingController : Controller
    {
        public ActionResult Index()
        {
            var promotions = new List<PromotionSettingViewModel>();
            using (var context = new ShoppingCartContext())
            {
                context.Promotions.ForEach( p => promotions.Add(new PromotionSettingViewModel()
                {
                    Code = p.Code,
                    Id = p.Id
                }));
            }
            return View(promotions);
        }

        public ActionResult EditPromotionDetails(int id)
        {
            var ret = new PromotionSettingViewModel();
            using (var context = new ShoppingCartContext())
            {
                var promo = context.Promotions.Single(p => p.Id == id);
                ret.Code = promo.Code;
                promo.PromotionLineItems.ForEach(p => ret.Promotions.Add(new PromotionSettingLineItemViewModel()
                {
                    Code = p.Code,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Active = p.Active,
                    PromotionLineItemExpression = p.PromotionLineItemExpression
                }));

                foreach (var r in ret.Promotions)
                {
                    r.Categories =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("Category")).Split('=')[1]
                            .ToString();
                    r.PercentDiscount =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("PercentDiscount")).Split('=')[1]
                            .ToString();
                    r.AmountDiscount =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("AmountDiscount")).Split('=')[1]
                            .ToString();
                    r.PriceTypes =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("PriceType")).Split('=')[1]
                            .ToString();
                    r.Items =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("ItemCode")).Split('=')[1]
                            .ToString();
                    r.BuyItemCodes =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("BuyItemCode")).Split('=')[1]
                            .ToString();
                    r.BuyItemCounts =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("BuyItemCount")).Split('=')[1]
                            .ToString();
                    r.GetItemCodes =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("GetItemCode")).Split('=')[1]
                            .ToString();
                    r.GetItemCount =
                        r.PromotionLineItemExpression.Split(';').Single(c => c.StartsWith("GetItemCount")).Split('=')[1]
                            .ToString();
                }
            }
            return null;
        }
        
    }
}