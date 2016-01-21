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
    public class CategoryManagmentController : Controller
    {
        //
        // GET: /CategoryManagment/
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                var ret = new CategoryManagementViewModel();

                using (var context = new ShoppingCartContext())
                {
                    

                    context.Categories.ForEach(c => ret.CategoryViewModels.Add(new CategoryViewModel()
                    {
                        Code = c.Code,
                        Description = c.Description,
                        Id = c.Id,
                        Icon = c.Icon,
                        Active = c.Active
                    }));
                    
                }

                using (var context = new ShoppingCartContext())
                {

                    foreach (var cat in ret.CategoryViewModels)
                    {
                        cat.ProductsCount = (from p in context.Products
                            where p.Categories.Any(c => c.Code == cat.Code)
                            select p).Count();
                    }
                }
                return View(ret);
            }
            return View();
        }

        public ActionResult EditCategory(int id)
        {
            TempData["CategoryEditId"] = id;
            using (var context = new ShoppingCartContext())
            {
                var cat = new CategoryViewModel();
                if (id == -1)
                {

                    cat.Code = "";
                    cat.Description = "Description";
                    cat.Icon = null;
                    cat.Active = true;
                }
                else
                {
                    var oCat = context.Categories.Single(c => c.Id == id);
                    if (oCat != null)
                    {
                        cat.Id = oCat.Id;
                        cat.Code = oCat.Code;
                        cat.Description = oCat.Description;
                        cat.Icon = oCat.Icon;
                        cat.Active = oCat.Active;
                    }
                }
                return View(cat);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = (int) TempData["CategoryEditId"];
                using (var context = new ShoppingCartContext())
                {
                    if (id > 0)
                    {
                        var cat = context.Categories.Single(c => c.Id == id);
                        cat.Description = model.Description;

                        if (model.IconImage != null)
                        {
                            if (model.IconImage.ContentLength > (4 * 1024 * 1024))
                            {
                                ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                                return View();
                            }
                            if (
                                !(model.IconImage.ContentType == "image/jpeg" ||
                                  model.IconImage.ContentType == "image/gif"))
                            {
                                ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                            }

                            byte[] data = new byte[model.IconImage.ContentLength];
                            model.IconImage.InputStream.Read(data, 0,
                                model.IconImage.ContentLength);

                            cat.Icon = data;
                        }

                        cat.Active = model.Active;

                        await context.SaveChangesAsync();
                        return RedirectToAction("EditCategory", cat.Id);
                    }
                    else
                    {
                        var ret = new Category();
                        ret.Code = SeqHelper.Next("Category").ToString();
                        ret.Description = model.Description;

                        if (model.IconImage != null)
                        {
                            if (model.IconImage.ContentLength > (4 * 1024 * 1024))
                            {
                                ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                                return View();
                            }
                            if (
                                !(model.IconImage.ContentType == "image/jpeg" ||
                                  model.IconImage.ContentType == "image/gif"))
                            {
                                ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                            }

                            byte[] data = new byte[model.IconImage.ContentLength];
                            model.IconImage.InputStream.Read(data, 0,
                                model.IconImage.ContentLength);

                            ret.Icon = data;
                        }
                        ret.Active = model.Active;

                        context.Categories.Add(ret);

                        await context.SaveChangesAsync();
                        return RedirectToAction("EditCategory", ret.Id);
                    }
                }
            }
            return View();
        }
    }
}