using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        [Required]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Phone must be 10 to 15 digits, optional leading +")]
        public string? Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}

