using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlexIn.Models
{
    public class ProductFeature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int FeatureId { get; set; }
        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [Required]
        public int FeatureOptionId { get; set; }
        [ForeignKey("FeatureOptionId")]
        public FeatureOption FeatureOption { get; set; }
    }
}
