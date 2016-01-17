using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace MyProject.Models.Content
{
    public class Content
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string ItemCode { get; set; }

        public byte[] Image { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("ContentType")]
        public int ContentTypeId { get; set; }

        public virtual ContentType ContentType { get; set; }

    }
}