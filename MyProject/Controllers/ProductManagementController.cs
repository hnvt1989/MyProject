using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Account;
using MyProject.Models.Core;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;

namespace MyProject.Controllers
{
    public class ProductManagementController : Controller
    {
        //
        // GET: /ProductManagement/
        public ActionResult Index()
        {
            if (ModelState.IsValid)
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
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            using (var context = new ShoppingCartContext())
            {
                //new product
                if (id == -1)
                {
                    var priceTypes = context.PriceTypes.ToList();
                    var categories = context.Categories.ToList();
                    var offers = new List<ProductOffer>();
                    priceTypes.ForEach(pt => offers.Add(new ProductOffer(){PriceTypeId = pt.Id,Price = 0m}));
                    var ret = new EditProductViewModel()
                    {
                        ProductView = new ProductViewModel()
                        {
                            Code = "",
                            Description = "Description",
                            DetailDescription = "Detail description",
                        },
                        Categories = new List<Category>(),
                        Offers = new List<ProductOffer>(),
                        PriceTypes = new List<PriceType>(),
                    };
                    ret.Categories.AddRange(categories);
                    ret.PriceTypes.AddRange(priceTypes);
                    ret.Offers.AddRange(offers);

                    TempData["ProductEditId"] = id;
                    return View(ret);
                }


                //existing product
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
                        },
                        Categories = new List<Category>(),
                        Offers = new List<ProductOffer>(),
                        PriceTypes = new List<PriceType>(),
                    };
                    ret.Categories.AddRange(categories);
                    ret.Offers.AddRange(offers);
                    ret.PriceTypes.AddRange(priceTypes);

                    TempData["ProductEditId"] = id;
                    return View(ret);
                }
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditProduct(EditProductViewModel model)
        {
            var id = (int)TempData["ProductEditId"];
            if (ModelState.IsValid)
            {
                using (var context = new ShoppingCartContext())
                {
                    //new product
                    if (id == -1)
                    {
                        var newProd = new Product()
                        {
                            Code = (string.IsNullOrEmpty(model.ProductView.Code)) ? SeqHelper.Next("Item").ToString() : (model.ProductView.Code),
                            Description = model.ProductView.Description,
                            FeatureProduct = true,
                            //Weight = 0.5m,
                            //QuantityOnHand = 22,
                            DetailDescription = model.ProductView.DetailDescription,
                            Categories = new List<Category>()
                            {
                                context.Categories.SingleOrDefault(c => c.Code == "1001")
                            },
                        };

                        if (model.ProductView.ProductImage != null)
                        {
                            if (model.ProductView.ProductImage.ContentLength > (4*1024*1024))
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
                            model.ProductView.ProductImage.InputStream.Read(data, 0,
                                model.ProductView.ProductImage.ContentLength);

                            newProd.Image = data;
                        }
                        context.Products.Add(newProd);
                        await context.SaveChangesAsync();
                        int productId = newProd.Id;

                        //update pricing
                        foreach (var p in model.Offers)
                        {
                            context.ProductOffers.Add(new ProductOffer()
                            {
                                ProductId = productId,
                                PriceTypeId = p.PriceTypeId,
                                Price = p.Price
                                
                            });
                        }

                        await context.SaveChangesAsync();

                        return RedirectToAction("EditProduct", productId);
                    }
                    else
                    {


                        //existing product
                        var product = context.Products.Single(p => p.Id == id);

                        //string paymentTypeValue = Request.Form["paymentType"].ToString();

                        product.Description = model.ProductView.Description;
                        product.Code = model.ProductView.Code;
                        product.DetailDescription = model.ProductView.DetailDescription;

                        if (model.ProductView.ProductImage != null)
                        {
                            if (model.ProductView.ProductImage.ContentLength > (4*1024*1024))
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
                            model.ProductView.ProductImage.InputStream.Read(data, 0,
                                model.ProductView.ProductImage.ContentLength);

                            product.Image = data;
                        }

                        //update pricing
                        foreach (var p in model.Offers)
                        {
                            context.ProductOffers.Single(
                                po => po.ProductId == product.Id && p.PriceTypeId == po.PriceTypeId)
                                .Price = p.Price;
                        }

                        await context.SaveChangesAsync();
                    }
                    return RedirectToAction("EditProduct", model.ProductView.Id);
                }
                
            }
            return View();
        }
    }
}