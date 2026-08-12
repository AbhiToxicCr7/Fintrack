using System.Collections.Generic;

namespace AuthenticationServer.DTOs
{
    public class UserIncomeResponseDTO
    {
        public decimal TotalIncomeAmount { get; set; }

        public IEnumerable<UserIncomeDTO> Incomes { get; set; }

        public Dictionary<string, decimal> IncomeByCategory { get; set; }

        public string HighestIncomeCategory { get; set; }
    }
}
