using System.ComponentModel.DataAnnotations;

namespace MySharedModels
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)] // محدود کردن طول URL
        public string Url { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
