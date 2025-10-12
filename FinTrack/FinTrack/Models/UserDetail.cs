using FinTrack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ResourceServer.Models
{
    public class UserDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Profession { get; set; }

        [Required, MaxLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AnnualSalary { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlySalary => AnnualSalary / 12m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyInvestment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]      // Prevent model binding expecting a 'user' field
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<UserInvestment> UserInvestments { get; set; } = new List<UserInvestment>();
    }
}
