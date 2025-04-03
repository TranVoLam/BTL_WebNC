using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BTL_WebNC.Models;

namespace BTL_WebNC.Models;

[Table("Reviews")]
public class Review {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Đừng quên tiêu đề nhé")]
    [Column(TypeName = "NVARCHAR(255)")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.DateTime)]
    [Column(TypeName = "DATETIME2(2)")]
    public DateTime CreateAt { get; set; }

    [Required(ErrorMessage = "Hãy đánh giá gì đó nhé")]
    [Column(TypeName = "NVARCHAR(MAX)")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Đánh giá nhé")]
    public byte Rating { get; set; }

    [Required]
    public bool ReviewStatus { get; set; } = false;

    [Required(ErrorMessage = "Đừng quên chọn danh mục")]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Media> Medias { get; set; } = null!;
    public ICollection<Vote> Votes { get; set; } = null!;
    public ICollection<Save> Saves { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}