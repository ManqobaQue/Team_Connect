using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyPhonebook.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "DepartmentId")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string ExtensionNumber { get; set; } = string.Empty;

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
