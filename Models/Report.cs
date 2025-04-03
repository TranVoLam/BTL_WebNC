using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Reports")]
public class Report {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "DATETIME2(2)")]
    [DataType(DataType.DateTime)]
    public DateTime CreateAt { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(20)")]
    public string TargetType { get; set; } = string.Empty;

    [Required]
    public int TargetId { get; set; }

    [Column(TypeName = "NVARCHAR(500)")]
    public string? Content { get; set; } 

    [Required]
    [Column(TypeName = "BIT")]
    public bool ReportStatus { get; set; } = false;

    [Column(TypeName = "BIT")]
    public bool? Result { get; set; }

    public int? LockDays { get; set; }

    [Column(TypeName = "DATETIME2(2)")]
    [DataType(DataType.DateTime)]
    public DateTime? ResolvedAt { get; set; }

    public int ReporterId { get; set; }
    public int HandlerId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int ReasonId { get; set; }
    public Reason Reason { get; set; } = null!;
}