using FinTrack.Models;
using System.Collections.Generic;
using System.Linq;
using AuthenticationServer.Models;

namespace AuthenticationServer.Helpers
{
    public class UserIncomeHelper
    {
        public IEnumerable<UserIncome> FilterDataWithParams(IQueryable<UserIncome> incomes, string? category = null, int? day = null, int? month = null, int? year = null)
        {
            IQueryable<UserIncome> res = incomes;

            if (!string.IsNullOrEmpty(category))
            {
                res = res.Where(x => x.Category == category);
            }

            if (year.HasValue)
            {
                res = res.Where(x => x.Date.Year == year);
            }

            if (month.HasValue)
            {
                res = res.Where(x => x.Date.Month == month);
            }

            if (day.HasValue)
            {
                res = res.Where(x => x.Date.Day == day);
            }

            return res.ToList();
        }

        public decimal CalculateMonthlyTotalIncome(IQueryable<UserIncome> incomes)
        {
            return incomes
                .Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year)
                .Sum(x => x.Amount);
        }

        public Dictionary<string, decimal> CalculateCategoryWiseIncome(IQueryable<UserIncome> incomes)
        {
            return incomes
                .GroupBy(x => x.Category)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
        }

        public string CalculateHighestIncomeCategory(IQueryable<UserIncome> incomes)
        {
            return incomes
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Select(x => x.Category)
                .FirstOrDefault() ?? "No Income";
        }
    }
}
