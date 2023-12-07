using System.ComponentModel.DataAnnotations;

namespace testtask.Models
{
    public class ProfileDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(11)]
        public string PersonalNumber { get; set; }
    }
}
