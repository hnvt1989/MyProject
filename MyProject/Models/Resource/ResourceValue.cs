using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyProject.Models.Resource
{
    public class ResourceValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public string Code { get; set; }

        [ForeignKey("ResourceKey")]
        public int ResourceKeyId { get; set; }

        [ForeignKey("ResourceSet")]
        public int ResourceSetId { get; set; }

        [ForeignKey("Culture")]
        public int CultureId { get; set; }
        
        public string Value { get; set; }

        public virtual Culture Culture { get; set; }

        public virtual ResourceKey ResourceKey { get; set; }

        public virtual ResourceSet ResourceSet { get; set; }
    }
}