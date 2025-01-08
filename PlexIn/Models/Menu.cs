using System.ComponentModel.DataAnnotations;

namespace PlexIn.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BusinessId { get; set; }

        public Business Business { get; set; }

        public ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
