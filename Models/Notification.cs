using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Notifications")]
public class Notification {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(20)")]
    public string TargetType { get; set; } = string.Empty;

    [Required]
    public int TargetId { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string NotificationType { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR(500)")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    [Column(TypeName = "DATETIME2(2)")]
    public DateTime CreateAt { get; set; }

    [Required]
    [Column(TypeName = "BIT")]
    public bool IsRead { get; set; } = false;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}