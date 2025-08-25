using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        // Use Phone attribute + RegularExpression for stricter validation
        [Required]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Phone must be 10 to 15 digits, optional leading +")]
        public string? Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        // store only the hash in the DB
        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string? Role { get; set; }
    }
}
