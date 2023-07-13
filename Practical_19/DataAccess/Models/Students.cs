using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Students
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student Name is required")]
        [StringLength(50)]
        public string StudentName { get; set; }
        [Required(ErrorMessage ="Mobile number should be of 10 digit")]
        [MaxLength(10)]
        public string MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
