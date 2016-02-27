using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MyProject.DAL;
using MyProject.Models.Account;
using MyProject.Models.Core;
using MyProject.Models.ShoppingCart;
using MyProject.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class ProductManagementController : Controller
    {
        public ActionResult FilterByCategory(string code)
        {
            if (code == "All")
                return RedirectToAction("Index");

            var catDescription = "";
            var prods = new List<Product>();

            using (var context = new ShoppingCartContext())
            {
                prods = (from p in context.Products
                    where p.Categories.Any(c => c.Code == code)
                    select p).ToList();
                catDescription = context.Categories.Single(c => c.Code == code).Description;
            }
            using (var context = new ShoppingCartContext())
            {

                var productCategories = new ProductManagementViewModel();
                foreach (var prod in prods)
                {
                    productCategories.Products.Add(new ProductViewModel()
                    {
                        Code = prod.Code,
                        Description = prod.Description,
                        Image = prod.Image,
                        Id = prod.Id,
                        Active = prod.Active,
                        QuantityOnHand = prod.QuantityOnHand,
                        CategoriesString = catDescription,
                    });
                }

                productCategories.SelectedCategory = code;

                return View("Index", productCategories);
            }

        }

        public ActionResult Index()
        {


            if (ModelState.IsValid)
            {
                using (var context = new ShoppingCartContext())
                {
                    var productCategories = new ProductManagementViewModel();

                    var categories = context.Categories.ToList();

                    //var defaultCat = categories.First();

                    //var prods = from p in context.Products
                    //            where p.Categories.Any(c => c.Code == defaultCat.Code)
                    //            select p;

                    var prods = context.Products;
                    //productCategories.Products.AddRange(prods);
                    //                  where cartItems.Code == ShoppingCartId
                    foreach (var prod in prods)
                    {

                        productCategories.Products.Add(new ProductViewModel()
                        {
                            Code = prod.Code,
                            Description = prod.Description,
                            Image = prod.Image,
                            //ImageAlt0 = prod.ImageAlt0,
                            //ImageAlt1 = prod.ImageAlt1,
                            Id = prod.Id,
                            Active = prod.Active,
                            QuantityOnHand = prod.QuantityOnHand
                        });

                    }

                    foreach (var product in productCategories.Products)
                    {

                            var catList = 
                            context.Products.Where(p => p.Id == product.Id)
                                .SelectMany(prod => prod.Categories).ToList();
                        if (catList.Any())
                        {
                            product.CategoriesString =
                                catList.Select(c => c.Description).ToList().Aggregate((i, j) => i + "," + j);
                        }
                        else
                        {
                            product.CategoriesString = "";
                        }
                    }

                    return View(productCategories);
                }
            }
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
                    priceTypes.ForEach(pt => offers.Add(new ProductOffer() {PriceTypeId = pt.Id, Price = 0m}));
                    var ret = new EditProductViewModel()
                    {
                        ProductView = new ProductViewModel()
                        {
                            Code = "",
                            Description = "Description",
                            DetailDescription = "Detail description",
                            BuyInPrice = 0m,
                            Categories = new List<Category>(),
                            Active = true,
                            ReviewVideoUrl = "Enter Youtube video Url here"
                        },
                        Categories = new List<CategoryViewModel>(),
                        Offers = new List<ProductOffer>(),
                        PriceTypes = new List<PriceType>(),
                    };
                    categories.ForEach(
                        c =>
                            ret.Categories.Add(new CategoryViewModel()
                            {
                                Code = c.Code,
                                Description = c.Description,
                                IsChecked = false,
                                ParentId = c.ParentId,
                                Id = c.Id
                            }));
                    ret.PriceTypes.AddRange(priceTypes);
                    ret.Offers.AddRange(offers);

                    ret.ProductView.WeightOunce = 0m;
                    ret.ProductView.WeightPounds = 0m;
                    ret.ProductView.QuantityOnHand = 0;

                    ret.ProductView.Notes = "";

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
                    var newPriceTypes = priceTypes.Where(p => offers.All(o2 => o2.PriceTypeId != p.Id));
                    newPriceTypes.ForEach(p => offers.Add(new ProductOffer()
                    {
                        Code = product.Code + "-" + p.Code,
                        ProductId = product.Id,
                        PriceTypeId = p.Id,
                        Price = 0m,
                        Discountable = true
                    }));

                    var ret = new EditProductViewModel()
                    {
                        ProductView = new ProductViewModel()
                        {
                            Code = product.Code,
                            Description = product.Description,
                            DetailDescription = product.DetailDescription,
                            Image = product.Image,
                            ImageAlt0 = product.ImageAlt0,
                            ImageAlt1 = product.ImageAlt1,
                            Id = product.Id,
                            Categories = new List<Category>(),
                            BuyInPrice = product.BuyInPrice,
                            Active = product.Active,
                            ReviewVideoUrl = product.ReviewVideoUrl
                        },
                        Categories = new List<CategoryViewModel>(),
                        Offers = new List<ProductOffer>(),
                        PriceTypes = new List<PriceType>(),
                    };

                    ret.Offers.AddRange(offers);
                    ret.PriceTypes.AddRange(priceTypes);

                    TempData["ProductEditId"] = id;

                    var weightInOunce = product.Weight*16;
                    ret.ProductView.WeightOunce = weightInOunce%16;
                    ret.ProductView.WeightPounds = (weightInOunce - ret.ProductView.WeightOunce)/16;

                    //quantity on hand
                    ret.ProductView.QuantityOnHand = product.QuantityOnHand;

                    //categories
                    categories.ForEach(
                        c =>
                            ret.Categories.Add(new CategoryViewModel()
                            {
                                Id = c.Id,
                                Code = c.Code,
                                Description = c.Description,
                                IsChecked = false,
                                ParentId = c.ParentId,
                            }));
                    ret.ProductView.Categories.AddRange(
                        context.Products.Where(p => p.Id == id).SelectMany(prod => prod.Categories));

                    foreach (var cat in ret.Categories)
                    {
                        if (ret.ProductView.Categories.Select(c => c.Code).ToList().Contains(cat.Code))
                        {
                            cat.IsChecked = true;
                        }
                    }

                    ret.ProductView.Notes = product.Notes;

                    return View(ret);
                }

                return View();
            }
        }

        public ActionResult SearchProduct(ProductManagementViewModel productCategories)
        {
            using (var context = new ShoppingCartContext())
            {
                if (productCategories.SearchProductId.IsNullOrWhiteSpace())
                {
                    return RedirectToAction("Index");
                }
                var prods =
                    context.Products.Where(
                        p =>
                            productCategories.SearchProductId.Contains(p.Code) ||
                            productCategories.SearchProductId.Contains(p.Description));


                var categories = context.Categories.ToList();
                var defaultCat = categories.First();

                foreach (var prod in prods.Take(12))
                {

                    productCategories.Products.Add(new ProductViewModel()
                    {
                        Code = prod.Code,
                        Description = prod.Description,
                        Image = prod.Image,
                        Id = prod.Id
                    });
                }
                //no thing return
                if (prods.ToList().Count == 0)
                {
                    ViewBag.Status = "No product matched the search criteria";

                }
                return View("Index", productCategories);

            }

        }


        [HttpPost]
        public async Task<ActionResult> EditProduct(EditProductViewModel model)
        {
            var id = (int) TempData["ProductEditId"];
            if (!ModelState.IsValid) return View(model);

            using (var context = new ShoppingCartContext())
            {
                //new product
                if (id == -1)
                {
                    var newProd = new Product()
                    {
                        Code = SeqHelper.Next("Item").ToString(),
                        //Code = (string.IsNullOrEmpty(model.ProductView.Code)) ? SeqHelper.Next("Item").ToString() : (model.ProductView.Code),
                        Description = model.ProductView.Description,
                        FeatureProduct = true,
                        //Weight = 0.5m,
                        //QuantityOnHand = 22,
                        BuyInPrice = model.ProductView.BuyInPrice,
                        DetailDescription = model.ProductView.DetailDescription,
                        Categories = new List<Category>()
                        {
                            context.Categories.SingleOrDefault(c => c.Code == "1001")
                        },
                        Active = model.ProductView.Active,
                        Notes = model.ProductView.Notes,
                        ReviewVideoUrl = (model.ProductView.ReviewVideoUrl.IsNullOrWhiteSpace()) ? "" : model.ProductView.ReviewVideoUrl
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
                            return View();
                        }

                        byte[] data = new byte[model.ProductView.ProductImage.ContentLength];
                        model.ProductView.ProductImage.InputStream.Read(data, 0,
                            model.ProductView.ProductImage.ContentLength);

                        newProd.Image = data;
                    }

                    //alt image 0
                    if (model.ProductView.ProductImageAlt0 != null)
                    {
                        if (model.ProductView.ProductImageAlt0.ContentLength > (4*1024*1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.ProductView.ProductImageAlt0.ContentType == "image/jpeg" ||
                              model.ProductView.ProductImageAlt0.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ProductView.ProductImageAlt0.ContentLength];
                        model.ProductView.ProductImageAlt0.InputStream.Read(data, 0,
                            model.ProductView.ProductImageAlt0.ContentLength);

                        newProd.ImageAlt0 = data;
                    }

                    //alt image 1
                    if (model.ProductView.ProductImageAlt1 != null)
                    {
                        if (model.ProductView.ProductImageAlt1.ContentLength > (4*1024*1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.ProductView.ProductImageAlt1.ContentType == "image/jpeg" ||
                              model.ProductView.ProductImageAlt1.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ProductView.ProductImageAlt1.ContentLength];
                        model.ProductView.ProductImageAlt1.InputStream.Read(data, 0,
                            model.ProductView.ProductImageAlt1.ContentLength);

                        newProd.ImageAlt1 = data;
                    }

                    context.Products.Add(newProd);
                    await context.SaveChangesAsync();


                    id = newProd.Id;

                    //update pricing
                    foreach (var p in model.Offers)
                    {
                        context.ProductOffers.Add(new ProductOffer()
                        {
                            ProductId = id,
                            PriceTypeId = p.PriceTypeId,
                            Price = p.Price

                        });
                    }

                    //weight
                    decimal ounces = model.ProductView.WeightOunce;
                    decimal lbs = model.ProductView.WeightPounds;
                    decimal weight = lbs + (ounces/16);
                    newProd.Weight = weight;

                    //quantity on hand
                    newProd.QuantityOnHand = model.ProductView.QuantityOnHand;

                    //categories
                    foreach (var cat in model.Categories)
                    {

                        if (cat.IsChecked)
                        {
                            newProd.Categories.Add(context.Categories.Single(c => c.Code == cat.Code));
                        }
                    }

                    await context.SaveChangesAsync();

                    //return View("Index");
                    //return RedirectToAction("EditProduct", newProd.Id);
                }
                else
                {


                    //existing product
                    var product = context.Products.Single(p => p.Id == id);

                    //string paymentTypeValue = Request.Form["paymentType"].ToString();

                    product.Description = model.ProductView.Description;
                    //product.Code = model.ProductView.Code;
                    product.DetailDescription = model.ProductView.DetailDescription;
                    product.BuyInPrice = model.ProductView.BuyInPrice;
                    product.Active = model.ProductView.Active;
                    product.Notes = model.ProductView.Notes;
                    product.ReviewVideoUrl = (model.ProductView.ReviewVideoUrl.IsNullOrWhiteSpace())
                        ? ""
                        : model.ProductView.ReviewVideoUrl;

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

                    //alt image 0
                    if (model.ProductView.ProductImageAlt0 != null)
                    {
                        if (model.ProductView.ProductImageAlt0.ContentLength > (4*1024*1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.ProductView.ProductImageAlt0.ContentType == "image/jpeg" ||
                              model.ProductView.ProductImageAlt0.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ProductView.ProductImageAlt0.ContentLength];
                        model.ProductView.ProductImageAlt0.InputStream.Read(data, 0,
                            model.ProductView.ProductImageAlt0.ContentLength);

                        product.ImageAlt0 = data;
                    }

                    //alt image 1
                    if (model.ProductView.ProductImageAlt1 != null)
                    {
                        if (model.ProductView.ProductImageAlt1.ContentLength > (4*1024*1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.ProductView.ProductImageAlt1.ContentType == "image/jpeg" ||
                              model.ProductView.ProductImageAlt1.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ProductView.ProductImageAlt1.ContentLength];
                        model.ProductView.ProductImageAlt1.InputStream.Read(data, 0,
                            model.ProductView.ProductImageAlt1.ContentLength);

                        product.ImageAlt1 = data;
                    }

                    //update pricing
                    foreach (var p in model.Offers)
                    {
                        var update = context.ProductOffers.SingleOrDefault(
                            po => po.ProductId == product.Id && p.PriceTypeId == po.PriceTypeId);
                        if (update != null)
                        {
                            update.Price = p.Price;
                        }
                        else
                        {
                            context.ProductOffers.Add(new ProductOffer()
                            {
                                ProductId = product.Id,
                                PriceTypeId = p.PriceTypeId,
                                Discountable = true,
                                Price = p.Price,
                                Code = product.Code + "-" + "O"
                            });
                        }
                    }

                    //weight
                    decimal ounces = model.ProductView.WeightOunce;
                    decimal lbs = model.ProductView.WeightPounds;
                    decimal weight = lbs + (ounces/16);
                    product.Weight = weight;


                    //quantity on hand
                    product.QuantityOnHand = model.ProductView.QuantityOnHand;


                    //categories
                    model.ProductView.Categories.AddRange(
                        context.Products.Where(p => p.Id == id).SelectMany(prod => prod.Categories));

                    var setCat = model.ProductView.Categories;

                    //load both  collection before removing many-to-many can work !
                    context.Entry(product).Collection("Categories").Load();

                    foreach (var cat in model.Categories)
                    {

                        if (cat.IsChecked && !setCat.Select(c => c.Code).ToList().Contains(cat.Code))
                        {
                            product.Categories.Add(context.Categories.Single(c => c.Code == cat.Code));
                        }
                        if (!cat.IsChecked && setCat.Select(c => c.Code).ToList().Contains(cat.Code))
                        {


                            product.Categories.Remove(context.Categories.Single(c => c.Code == cat.Code));
                        }
                    }

                    await context.SaveChangesAsync();
                }

                return RedirectToAction("EditProduct", id);
            }
            
        }


        [HttpGet]
        public ActionResult ViewProductLessDetail(int id)
        {
            var ret = new ProductViewLessDetail();

            using (var context = new ShoppingCartContext())
            {
                var shippingRate = Convert.ToDecimal(context.AppSettings.Single(a => a.Code == "ShippingRate").Value);

                //existing product
                var product = context.Products.Single(p => p.Id == id);

                var shippingCost = product.Weight * shippingRate;

                if (product != null)
                {
                    //query for available price type and categories, also product offers for this product
                    //var priceTypes = context.PriceTypes.ToList();
                    //var categories = context.Categories.ToList();
                    var retailpriceTypeid = context.PriceTypes.Single(pt => pt.Code == "R").Id;
                    ret.Price =
                        context.ProductOffers.Single(o => o.ProductId == id && o.PriceTypeId == retailpriceTypeid).Price;
                    
                    ret.Code = product.Code;
                    ret.Description = product.Description;
                    ret.DetailDescription = product.DetailDescription;
                    ret.Image = product.Image;
                    ret.ImageAlt0 = product.ImageAlt0;
                    ret.ImageAlt1 = product.ImageAlt1;
                    ret.Id = product.Id;
                    ret.BuyInPrice = product.BuyInPrice + shippingCost;
                    ret.Active = product.Active;
                    ret.QuantityOnHand = product.QuantityOnHand;
                    ret.Profit = ret.Price - ret.BuyInPrice;

                    TempData["ProductEditId"] = id;

                    
                }
            }
            return View(ret);
        }
    }
}