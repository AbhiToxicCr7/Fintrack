using FinTrack.Models;

namespace AuthenticationServer.DTOs
{
    public class UserExpenseResponseDTO
    {
        public decimal TotalExpenseAmount { get; set; }
        public IEnumerable<UserExpenseDTO> Expenses { get; set; }

        public Dictionary<CategoryTypes, decimal>  ExpenseByCategory { get; set; }

        public string HighestSpentCategory { get; set; }    
    }
}
