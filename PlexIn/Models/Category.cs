using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlexIn.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // نام دسته‌بندی

        // رابطه یک به چند
        [Required]
        public int BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business Business { get; set; }

        public ICollection<Product>? Products { get; set; } // محصولات مرتبط با این دسته
    }
}
