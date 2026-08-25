using AuthenticationServer.DTOs;
using FinTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Helpers
{
    public class DashboardHelper
    {
        private readonly ApplicationDbContext _context;
        public DashboardHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<RecentTransaction> GetRecentTransactions(int userId, int count=10)
        {
            var expenses = _context.UserExpenses
                                   .Where(x => x.UserId == userId && x.IsActive)
                                   .Select(x => new RecentTransaction
                                   {
                                       Category = x.Category.ToString(),
                                       Note = x.Note,
                                       Amount = -x.Amount,
                                       Date = x.Date,
                                       TransactionType = "Expense"
                                   });

            var incomes = _context.UserIncomes
                .Where(x => x.UserId == userId && x.IsActive)
                .Select(x => new RecentTransaction
                {
                    Category = x.Category.ToString(),
                    Note = x.Note,
                    Amount = x.Amount,
                    Date = x.Date,
                    TransactionType = "Income"
                });

            return expenses
                .Concat(incomes)
                .OrderByDescending(x => x.Date)
                .Take(count)
                .ToList();
        }
    }
}
