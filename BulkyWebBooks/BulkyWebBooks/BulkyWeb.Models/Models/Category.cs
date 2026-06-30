using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("First Category Name")]
        public string CategoryName { get; set; }
        [DisplayName("First Display Order")]
        public int DisplayOrder { get; set; }

    }
}
