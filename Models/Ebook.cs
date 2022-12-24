using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;

namespace EbookTest.Models
{
    public class Ebook
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string auteur { get; set; }
        public string Description { get; set; }
        public float prix { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; }
        public int stock { get; set; }
        [NotMapped]
        public IFormFile BookImage { get; set; }
    
    }
}
