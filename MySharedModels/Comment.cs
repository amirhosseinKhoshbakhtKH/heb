using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySharedModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public int? BusinessId { get; set; } // کامنت برای بیزنس
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }

        public int? ProductId { get; set; } // کامنت برای محصول
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CommentReply>? Replies { get; set; }
    }

    public class CommentReply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
