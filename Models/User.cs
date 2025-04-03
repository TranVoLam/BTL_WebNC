using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WebNC.Models;

[Table("Users")]
public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email không thể để trống")]
    [DataType(DataType.EmailAddress)]
    [Column(TypeName = "NVARCHAR(254)")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string PasswordHash { get; set; } = string.Empty;

    [NotMapped]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
    [MaxLength(50, ErrorMessage = "Mật khẩu tối đa là 50 kí tự")]
    [MinLength(10, ErrorMessage = "Mật khẩu tối thiểu là 10 kí tự")]
    public string Password { get; set; } = string.Empty;

    [NotMapped]
    [Required(ErrorMessage = "Nhập lại mật khẩu")]
    [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
    public string RePassword { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "DATETIME2(2)")]
    [DataType(DataType.DateTime)]
    public DateTime CreateAt { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string? FullName { get; set; } = string.Empty;

    [Column(TypeName = "NVARCHAR(200)")]
    public string? AvatarURL { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    [Column(TypeName = "TINYINT")]
    public byte? Sex { get; set; }

    [Required]
    [Column(TypeName = "BIT")]
    public bool UserRole { get; set; } = false;

    [Required]
    [Column(TypeName = "BIT")]
    public bool UserStatus { get; set; } = false;

    [Column(TypeName = "DATETIME2(2)")]
    public DateTime? TimeUnlock { get; set; }

    public ICollection<Review> Reviews { get; set; } = null!;
    public ICollection<Report> Reports { get; set; }= null!;
    public ICollection<Vote> Votes { get; set; } = null!;
    public ICollection<Save> Saves { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<Notification> Notifications { get; set; } = null!;
    public ICollection<History> Histories { get; set; } = null!;
}