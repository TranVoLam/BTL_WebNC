using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Votes")]
public class Vote {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(20)")]
    public string TargetType { get; set; } = string.Empty;

    [Required]
    public int TargetId { get; set; }

    [Column(TypeName = "BIT")]
    public bool? UpDown { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Column(TypeName = "DATETIME2(2)")]
    public DateTime CreateAt { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}