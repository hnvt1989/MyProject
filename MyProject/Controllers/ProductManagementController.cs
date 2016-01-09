using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Account;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class ProductManagementController : Controller
    {
        //
        // GET: /ProductManagement/
        public ActionResult Index()
        {


            using (var context = new ShoppingCartContext())
            {
                var productCategories = new ProductManagementViewModel();

                var categories = context.Categories.ToList();
                var defaultCat = categories.First();

                var prods = from p in context.Products
                    where p.Categories.Any(c => c.Code == defaultCat.Code)
                    select p;
                //productCategories.Products.AddRange(prods);
                //                  where cartItems.Code == ShoppingCartId
                foreach (var prod in prods)
                {

                    productCategories.Products.Add(new ProductViewModel()
                    {
                        Code = prod.Code,
                        Description = prod.Description,
                        Image = prod.Image,
                        Id = prod.Id
                    });
                }
                return View(productCategories);
            }
        }

        public ActionResult Index1()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult AddNew()
        {
            return View();
        }

        public ActionResult EditProduct(int id)
        {
            using (var context = new ShoppingCartContext())
            {
                var product = context.Products.Single(p => p.Id == id);
                if (product != null)
                {
                    //query for available price type and categories, also product offers for this product
                    var priceTypes = context.PriceTypes.ToList();
                    var categories = context.Categories.ToList();
                    var offers = context.ProductOffers.Where(o => o.ProductId == id).ToList();

                    var ret = new EditProductViewModel()
                    {
                        ProductView = new ProductViewModel()
                        {
                            Code = product.Code,
                            Description = product.Description,
                            DetailDescription = product.DetailDescription,
                            Image = product.Image,
                            Id = product.Id
                        }
                    };
                    //ret.Categories.AddRange(categories);
                    //ret.Offers.AddRange(offers);
                    //ret.PriceTypes.AddRange(priceTypes);

                    TempData["ProductEditId"] = id;
                    return View(ret);
                }
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditProduct(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ShoppingCartContext())
                {
                    var id = (int)TempData["ProductEditId"];
                    var product = context.Products.Single(p => p.Id == id);

                    //string paymentTypeValue = Request.Form["paymentType"].ToString();
                    
                    product.Description = model.ProductView.Description;
                    product.Code = model.ProductView.Code;
                    product.DetailDescription = model.ProductView.DetailDescription;

                    if (model.ProductView.ProductImage != null)
                    {
                        if (model.ProductView.ProductImage.ContentLength > (4 * 1024 * 1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.ProductView.ProductImage.ContentType == "image/jpeg" ||
                              model.ProductView.ProductImage.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ProductView.ProductImage.ContentLength];
                        model.ProductView.ProductImage.InputStream.Read(data, 0, model.ProductView.ProductImage.ContentLength);

                        product.Image = data;
                    }
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("EditProduct", model.ProductView.Id);
            }
            return View();
        }
    }
}