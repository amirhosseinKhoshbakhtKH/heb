using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySharedModels
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // هش‌شده باشد.

        public ICollection<StarTransaction>? StarTransactions { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
    }

    public class StarTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        public int BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }

        [Required]
        public int Stars { get; set; } // تعداد ستاره

        [Required]
        public DateTime Date { get; set; } // تاریخ تراکنش
    }
}
