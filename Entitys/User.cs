using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace callapptask.Entitys
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Base64String]
        //theoretically hashed code
        public string Password { get; set; }

        [Range(1, 1)]
        public bool IsActive { get; set; } = false;
        [JsonIgnore]
        public UserProfile? UserProfile { get; set; }
    }
}
