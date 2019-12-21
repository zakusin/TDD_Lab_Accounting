using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    public class Accounting
    {
        private readonly IBudgetRepo _budgetRepo;

        public Accounting(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal QueryBudget(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var yearMonthStart = Convert.ToInt32(start.ToString("yyyyMM"));
            var yearMonthEnd = Convert.ToInt32(end.ToString("yyyyMM"));
            var budgetList = _budgetRepo.GetAll().Select(budget => new
                {
                    YearMonth = Convert.ToInt32(budget.YearMonth),
                    budget.Amount
                })
                .Where(r => r.YearMonth >= yearMonthStart && r.YearMonth <= yearMonthEnd)
                .ToList();

            if (!budgetList.Any())
            {
                return 0;
            }

            var sum = 0m;
            foreach (var budget in budgetList)
            {
                if (yearMonthStart == yearMonthEnd)
                {
                    sum += CalculateBudgetSum(start, end, budget?.Amount ?? 0);
                }
                else if (budget.YearMonth == yearMonthStart)
                {
                    sum += CalculateBudgetSum(start, GetEndDayOfMonth(start), budget.Amount);
                }
                else if (budget.YearMonth == yearMonthEnd)
                {
                    sum += CalculateBudgetSum(GetStartDayOfMonth(end), end, budget.Amount);
                }
                else
                {
                    sum += budget.Amount;
                }
            }

            return sum;
        }

        private static DateTime GetStartDayOfMonth(DateTime end)
        {
            return new DateTime(end.Year, end.Month, 1);
        }

        private static DateTime GetEndDayOfMonth(DateTime start)
        {
            return new DateTime(start.Year, start.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static int CalculateBudgetSum(DateTime start, DateTime end, int budgetAmount)
        {
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var days = QueryDays(start, end);
            var budgetSum = budgetAmount * days / daysInMonth;
            return budgetSum;
        }

        private static int QueryDays(DateTime start, DateTime end)
        {
            return end.DayOfYear - start.DayOfYear + 1;
        }
    }
}