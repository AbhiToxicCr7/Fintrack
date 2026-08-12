using AuthenticationServer.DTOs;
using AuthenticationServer.Helpers;
using FinTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Services
{
    public class UserExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserExpenseHelper _userExpenseHelper;

        public UserExpenseService(ApplicationDbContext context)
        {
            _context = context;
            _userExpenseHelper = new UserExpenseHelper();
        }
        public async Task<UserExpenseResponseDTO?> GetUserExpenses(int userId, string? category, int? day, int? month, int? year)
        {
            var userExpenses = _context.UserExpenses
                .Where(x => x.UserId == userId);

            var userMonthlyIncome = _context.UserDetails.FirstOrDefault(x => x.UserId == userId).MonthlySalary;

            var filteredExpenses = _userExpenseHelper.FilterDataWithParams(
                userExpenses,
                category,
                day,
                month,
            year);

            if (!filteredExpenses.Any())
                return null;

            var monthlyAmount = _userExpenseHelper.CalculateMonthlyTotalExpense(userExpenses);

            var expenseByCategory = _userExpenseHelper.CalculateCategoryWiseExpense(userExpenses, category);

            var highestSpentCategory = _userExpenseHelper.CaculateHighestSpentCategory(userExpenses);

            var incomeVsExpense = _userExpenseHelper.GetIncomeVsExpense(userExpenses, userMonthlyIncome);

            var expenses = filteredExpenses.Select(x => new UserExpenseDTO
            {
                Amount = x.Amount,
                Currency = x.Currency,
                Category = x.Category,
                Date = x.Date,
                Note = x.Note,
                UserId = x.UserId,
                Id = x.Id
            });

            return new UserExpenseResponseDTO
            {
                TotalExpenseAmount = monthlyAmount,
                Expenses = expenses,
                ExpenseByCategory = expenseByCategory,
                HighestSpentCategory = highestSpentCategory,
                MonthWiseIncomeExpenseData = incomeVsExpense
            };
        }
    }
}
