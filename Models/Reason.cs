using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WebNC.Models;

[Table("Reasons")]
public class Reason {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string ReasonName { get; set; } = string.Empty;

    public ICollection<Report> Reports { get; set; } = null!;
}