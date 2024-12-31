using System.ComponentModel.DataAnnotations;

namespace PlexIn.Models
{
    public class ProductFeature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int FeatureId { get; set; }

        public Feature Feature { get; set; }

        [Required]
        public int OptionId { get; set; }

        public FeatureOption Option { get; set; }
    }
}
