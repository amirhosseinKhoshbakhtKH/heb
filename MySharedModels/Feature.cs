using System.ComponentModel.DataAnnotations;

namespace MySharedModels
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // محدود کردن طول نام
        public string Name { get; set; }

        [Required]
        public int BusinessId { get; set; }

        public Business? Business { get; set; }

        public ICollection<FeatureOption>? Options { get; set; } = new List<FeatureOption>();
    }
}
