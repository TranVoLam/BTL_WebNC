using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTL_WebNC.Models;

namespace BTL_WebNC.Models;

[Table("Categories")]
public class Category {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(150)")]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    public bool CreateBy { get; set; } = true;
    public ICollection<Review> Reviews { get; set; } = null!;

}