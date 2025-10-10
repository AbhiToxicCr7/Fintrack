using System.ComponentModel.DataAnnotations;
using FinTrack.Models;
using ResourceServer.Models;

namespace FinTrack.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address {  get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<UserRole> UserRole { get; set; }
        public UserDetail? UserDetail { get; set; }

        // Computed property for full name
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
