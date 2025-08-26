using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class EmployeeTask
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an employee.")]
        public int? AssignedTo { get; set; }


        public int AssignedBy { get; set; }  // Manager Id

        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
