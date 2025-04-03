using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Saves")]
public class Save {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Column(TypeName = "DATETIME2(2)")]
    public DateTime CreateAt { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;
}