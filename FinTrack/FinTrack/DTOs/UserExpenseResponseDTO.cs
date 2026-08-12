using FinTrack.Models;

namespace AuthenticationServer.DTOs
{
    public class UserExpenseResponseDTO
    {
        public decimal TotalExpenseAmount { get; set; }
        public IEnumerable<UserExpenseDTO> Expenses { get; set; }

        public Dictionary<CategoryTypes, decimal>  ExpenseByCategory { get; set; }

        public string HighestSpentCategory { get; set; }    

        public IEnumerable<MonthWiseIncomeExpenseData> MonthWiseIncomeExpenseData { get; set; }
    }

    public class MonthWiseIncomeExpenseData
    {
        public decimal TotalExpenseAmount { get; set; }

        public decimal TotalIncomeAmount { get; set; }
        public string MonthName { get; set; }

        public int Year { get; set; }
    }
}
