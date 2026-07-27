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
            //var res = expenses.GroupBy(x => x.Category).ToDictionary(
            //    g=> g.Key,
            //    g=>g.Sum(x => x.Amount)
            //    );

            //var maxValue = res.Max(x => x.Value);

            //var bcd = res.Where(x => x.Value == maxValue).Select(x => x.Key).FirstOrDefault().ToString();
            //return bcd;

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
    }
}
