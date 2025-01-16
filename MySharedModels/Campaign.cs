using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySharedModels
{
    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // نام کمپین

        [Required]
        public int BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }

        [Required]
        public decimal MinAmount { get; set; } // حداقل مبلغ خرید برای شرکت در کمپین

        [Required]
        public int StarsPerUnit { get; set; } // تعداد ستاره به ازای هر واحد پولی
    }

    public class Reward
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // نام جایزه

        [Required]
        public int RequiredStars { get; set; } // تعداد ستاره مورد نیاز برای دریافت جایزه
    }
}
