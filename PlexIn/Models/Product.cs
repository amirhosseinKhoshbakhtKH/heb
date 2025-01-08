using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlexIn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }

        // ارتباط با Category
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // ارتباط با Menu
        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }

        // سایر روابط
        public ICollection<ProductFeature>? ProductFeatures { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
    }
}
