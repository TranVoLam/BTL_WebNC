using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Comments")]
public class Comment {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [Column(TypeName = "NVARCHAR(2000)")]
    public string? Content { get; set; }

    [Required]
    [Column(TypeName = "DATETIME2(2)")]
    [DataType(DataType.DateTime)]
    public DateTime CreateAt { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;
    public int? ParentId { get; set; }
    public Comment? Parent { get; set; }

    public ICollection<Comment> Children { get; set; } = new List<Comment>();
<<<<<<< HEAD
    
=======
>>>>>>> e374e4d (Hoàn thành các model)
}