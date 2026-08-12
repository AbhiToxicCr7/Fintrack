using AuthenticationServer.DTOs;
using FinTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Helpers
{
    public class UserExpenseHelper
    {
        public IEnumerable<UserExpense> FilterDataWithParams(IQueryable<UserExpense> expenses, string? category = null, int? day = null, int? month = null, int? year = null)
        {
            IQueryable<UserExpense> res = expenses;

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                res = expenses.Where(x => x.Category.ToString() == category);
            }

            // Filter by year
            if (year.HasValue)
            {
                res = expenses.Where(x => x.Date.Year == year);
            }

            // Filter by month
            if (month.HasValue)
            {
                res = expenses.Where(x => x.Date.Month == month);
            }

            // Filter by day
            if (day.HasValue)
            {
                res = expenses.Where(x => x.Date.Day == day);
            }

            return res.ToList();
        }

        public decimal CalculateMonthlyTotalExpense(IQueryable<UserExpense> expenses)
        {
            return expenses.Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year).Sum(x => x.Amount);
        }

        public Dictionary<CategoryTypes, decimal> CalculateCategoryWiseExpense(IQueryable<UserExpense> expenses, string? category = null)
        {
            var abc = expenses.GroupBy(x => x.Category).ToDictionary(
                g => g.Key,
                g => g.Sum(x => x.Amount)
                );

            return abc;
        }

        public string CaculateHighestSpentCategory(IQueryable<UserExpense> expenses)
        {
            var ans = expenses.GroupBy(x=>x.Category)
                .Select(g=> new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x=>x.TotalAmount)
                .Select(x=>x.Category.ToString())
                .FirstOrDefault() ?? "No Expenses";

            return ans;
        }

        public IEnumerable<MonthWiseIncomeExpenseData> GetIncomeVsExpense(IQueryable<UserExpense> expenses, decimal userMonthlySalary)
        {
            var result = expenses
                .GroupBy(x => new
                {
                    x.Date.Year,
                    x.Date.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalExpenseAmount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return result.Select(x => new MonthWiseIncomeExpenseData
            {
                Year = x.Year,
                MonthName = new DateTime(x.Year, x.Month, 1).ToString("MMMM"),
                TotalExpenseAmount = x.TotalExpenseAmount,
                TotalIncomeAmount = userMonthlySalary
            });
        }
    }
}
