﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using MyProject.DAL;
using MyProject.Models.Content;

namespace MyProject.Models.ViewModels.ContentManagement
{
    public class ContentManagmentViewModel
    {
        public ContentManagmentViewModel()
        {
            this.Contents = new List<ContentManagmentQuickSummaryViewModel>();
        }
        public List<ContentManagmentQuickSummaryViewModel> Contents { get; set; } 
    }



    public class ContentManagmentQuickSummaryViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int ContentTypeId { get; set; }

        public string ContentType { get; set; }
    }

    public class ContentDetailSummaryViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        //[RegularExpression("([1-9 .&'-]+)", ErrorMessage = "Only digit 1-9") ]
        //[Required]
        public int DisplayOrder { get; set; }

        public string Description { get; set; }
        public string ContentType { get; set; }

        //the URL will route to, right now just the product #
        public string RouteTo { get; set; }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        [DisplayName("Location of the text")]
        public string TextLocation { get; set; }

        [DisplayName("Text value. **** DO NOT ENTER IF THIS IS AN AD")]
        [AllowHtml]
        public string TextValue { get; set; }

        [DisplayName("Text display above Ad")]
        public string AdText { get; set; }

        [AllowHtml]
        [DisplayName("Style of the text display above Ad")]
        public string AdTextStyle { get; set; }

        public HttpPostedFileBase ContentImage { get; set; }
        public byte[] Image { get; set; }

        public SelectList ContentTypes
        {
            get
            {
                using (var context = new ShoppingCartContext())
                {
                    var ret = new List<Content.ContentType>();
                    ret.Add(new Content.ContentType()
                    {
                        Id = -1,
                        Code = "None",
                        Description = "Select Type"
                    });
                    ret.AddRange(context.ContentTypes.ToList());
                    return new SelectList(ret, "Code", "Description", "None");
                }
            }
        }

        public string SelectedContentType { get; set; }
    }
}