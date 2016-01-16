using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Content;
using MyProject.Models.Core;
using MyProject.Models.ViewModels.ContentManagement;
using WebGrease.Css.Extensions;

namespace MyProject.Controllers
{
    public class ContentManagementController : Controller
    {
        public ActionResult Index()
        {
            var ret = new ContentManagmentViewModel();
            using (var context = new ShoppingCartContext())
            {
                context.Contents.ForEach(c => ret.Contents.Add(new ContentManagmentQuickSummaryViewModel()
                {
                    Code = c.Code,
                    Description = c.Description,
                    Id = c.Id,
                    ContentTypeId = c.ContentTypeId
                }));

                foreach (var c in ret.Contents)
                {
                    c.ContentType = context.ContentTypes.Single(ct => ct.Id == c.ContentTypeId).Code;
                }
            }


            return View(ret);
        }

        [HttpGet]
        public ActionResult EditContent(int id, string contentType)
        {
            var ret = new ContentDetailSummaryViewModel();
            using (var context = new ShoppingCartContext())
            {
                var content = context.Contents.Single(c => c.Id == id);
                ret.Code = content.Code;
                ret.Description = content.Description;
                ret.ContentType = contentType;
                if (content.Image != null)
                    ret.Image = content.Image;
                ret.Id = id;
                ret.RouteTo = content.ImageUrl;
                ret.DisplayOrder = content.DisplayOrder;
            }
            return View(ret);
        }

        [HttpPost]
        public async Task<ActionResult> EditContent(ContentDetailSummaryViewModel model)
        {
            if (!ModelState.IsValid) return View("EditContent", model);


            using (var context = new ShoppingCartContext())
            {
                string contentTypeSelected = Request.Form["contentTypeSelection"].ToString();
                string contentType = context.ContentTypes.Single(t => t.Code == contentTypeSelected).Code;

                //edit existing
                if (model.Id > 0)
                {
                    var content = context.Contents.Single(c => c.Id == model.Id);
                    content.Description = model.Description;
                    await context.SaveChangesAsync();
                }
                else
                {

                    int itemId = context.Products.Single(p => p.Code == model.RouteTo).Id;
                    var newContent = new Content()
                    {
                        Code = SeqHelper.Next("Content").ToString(),
                        Description = model.Description,
                        ContentTypeId = context.ContentTypes.Single(ct => ct.Code == contentType).Id,
                        ImageUrl = string.Format("/Store/Details/{0}", itemId ),
                        DisplayOrder = model.DisplayOrder
                    };

                    if (contentType == "Ad" && model.ContentImage != null)
                    {
                        if (model.ContentImage.ContentLength > (4 * 1024 * 1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        }
                        if (
                            !(model.ContentImage.ContentType == "image/jpeg" ||
                              model.ContentImage.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.ContentImage.ContentLength];
                        model.ContentImage.InputStream.Read(data, 0,
                            model.ContentImage.ContentLength);

                        newContent.Image = data;
                    }
                    context.Contents.Add(newContent);
                    await context.SaveChangesAsync();
                    var newId = newContent.Id;
                    return RedirectToAction("EditContent", "ContentManagement", new { id = newId, contentType = contentType });

                }
                return RedirectToAction("EditContent", "ContentManagement", new { id = model.Id, contentType = contentType });
            }
        }

        public ActionResult AddNewContent()
        {
            var ret = new ContentDetailSummaryViewModel()
            {
                Id = -1,
                Code = "",
                Description = "Enter description here",
                ContentType = "",
                Image = null,
                RouteTo = ""
            };
            return View("EditContent",ret);
        }
    }
}