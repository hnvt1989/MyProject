using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models.ShoppingCart
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool FeatureProduct { get; set; }

        public byte[] Image { get; set; }

        public virtual Category Category { get; set; } 
    }

    
}