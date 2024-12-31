using System.ComponentModel.DataAnnotations;

namespace PlexIn.Models
{
    public class Business
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // هش‌شده باشد.

        // رابطه با Category
        public ICollection<Category> Categories { get; set; }
        public ICollection<Feature> Features { get; set; } = new List<Feature>();
        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
    }
}
