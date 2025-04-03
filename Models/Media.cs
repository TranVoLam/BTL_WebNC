using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Medias")]
public class Media {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(20)")]
    public string MediaType { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "VARCHAR(200)")]
    public string MediaURL { get; set; } = string.Empty;

    [Required]
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;
}