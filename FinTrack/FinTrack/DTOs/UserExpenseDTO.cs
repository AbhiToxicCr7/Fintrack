using FinTrack.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.DTOs
{
    public class UserExpenseDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; }

        public CategoryTypes Category { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime Date { get; set; }

        public string Note { get; set; }    
    }
}
