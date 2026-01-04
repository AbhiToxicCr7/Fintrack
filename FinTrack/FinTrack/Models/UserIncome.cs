using FinTrack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Models
{
    public class UserIncome
    {
        [Key]
        public int Id { get; set; }

        // FK to User (one User -> many UserExpenses)
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }  // e.g. "USD", "INR"

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }  // e.g. "Food", "Transport"

        public bool IsActive { get; set; } = true;

        public DateTime Date { get; set; }

        // Clarification: Storing TotalExpenses on each row duplicates data.
        // If you truly need a persisted column, keep this.
        // Prefer computing total per user via a query instead (see notes below).
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal TotalExpenses { get; set; }

        // Navigation
        public User User { get; set; }
    }
}
