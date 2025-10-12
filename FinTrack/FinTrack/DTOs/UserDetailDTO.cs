using FinTrack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.DTOs
{
    public class UserDetailDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Profession { get; set; }

        [Required, MaxLength(100)]
        public string JobTitle { get; set; }

        [Required]
        public decimal AnnualSalary { get; set; }

        public decimal MonthlyIncome { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public decimal MonthlyInvestment { get; set; }
    }
}
