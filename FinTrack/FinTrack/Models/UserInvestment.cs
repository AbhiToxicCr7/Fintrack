using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceServer.Models
{
    public class UserInvestment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserDetail")]
        public int UserDetailId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FundName { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal SalaryPercentageInvested { get; set; }

        [Required]
        [MaxLength(10)]
        public string InvestmentType { get; set; } // "Stock" or "SIP"

        [Column(TypeName = "decimal(18,2)")]
        public decimal InvestmentAmount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public UserDetail UserDetail { get; set; }
    }
}
