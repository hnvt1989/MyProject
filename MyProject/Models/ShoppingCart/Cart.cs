using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }

        public System.DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}