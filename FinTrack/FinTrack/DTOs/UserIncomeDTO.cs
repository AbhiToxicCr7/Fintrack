using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.DTOs
{
    public class UserIncomeDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime Date { get; set; }

        public string? Note { get; set; }

        public decimal TotalIncomeAmount { get; set; }
    }
}
