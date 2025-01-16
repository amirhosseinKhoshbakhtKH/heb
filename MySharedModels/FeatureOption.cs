using System.ComponentModel.DataAnnotations;

namespace MySharedModels
{
    public class FeatureOption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // محدود کردن طول مقدار
        public string Value { get; set; }

        [Required]
        public int FeatureId { get; set; }

        public Feature Feature { get; set; }
    }
}
