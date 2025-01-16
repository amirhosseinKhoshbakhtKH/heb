using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySharedModels
{
    public class DiscountCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } // کد تخفیف

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        public int BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }

        public decimal? DiscountAmount { get; set; } // مقدار تخفیف ثابت
        public float? DiscountPercentage { get; set; } // درصد تخفیف
    }
}
