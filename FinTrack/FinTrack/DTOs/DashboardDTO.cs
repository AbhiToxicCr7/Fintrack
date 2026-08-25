namespace AuthenticationServer.DTOs
{
    public class DashboardDTO
    { 
        public IEnumerable<RecentTransaction> RecentTransactions { get; set; }

        //TBD
        //public IEnumerable<MonthWiseIncomeExpenseData> MonthWiseIncomeExpenseData { get; set; }
    }

    public class RecentTransaction
    {
        public string Category { get; set; }
        public string? Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }
    }
}
