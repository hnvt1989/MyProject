using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MyProject.DAL;

namespace MyProject.Models.Core
{
    public class Sequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public long StartValue { get; set; }

        public long CurrentValue { get; set; }
    }

    public class SeqHelper
    {
        public static long Next(string seq)
        {
            using (var context = new ShoppingCartContext())
            {
                var next = context.Sequences.SingleOrDefault(s => s.Code == seq);
                next.CurrentValue++;
                context.SaveChanges();
                return next.CurrentValue;
            }
        }
    }
}