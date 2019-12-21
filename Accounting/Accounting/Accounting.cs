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
            var budgetSum = 0m;

            if (start > end)
            {
                return budgetSum;
            }

            var yearMonthStart = Convert.ToInt32(start.ToString("yyyyMM"));
            var yearMonthEnd = Convert.ToInt32(end.ToString("yyyyMM"));
            var budgetList = QueryBudgetRepo(yearMonthStart, yearMonthEnd);

            if (!budgetList.Any())
            {
                return budgetSum;
            }
            
            foreach (var budget in budgetList)
            {
                if (yearMonthStart == yearMonthEnd)
                {
                    budgetSum += CalculateBudgetSum(start, end, budget?.Amount ?? 0);
                }
                else if (budget.YearMonth == yearMonthStart)
                {
                    budgetSum += CalculateBudgetSum(start, GetEndDayOfMonth(start), budget.Amount);
                }
                else if (budget.YearMonth == yearMonthEnd)
                {
                    budgetSum += CalculateBudgetSum(GetStartDayOfMonth(end), end, budget.Amount);
                }
                else
                {
                    budgetSum += budget.Amount;
                }
            }

            return budgetSum;
        }

        private List<QueryBudget> QueryBudgetRepo(int yearMonthStart, int yearMonthEnd)
        {
            return _budgetRepo.GetAll().Select(budget => new QueryBudget
                {
                    YearMonth = Convert.ToInt32(budget.YearMonth),
                    Amount = budget.Amount
                })
                .Where(r => r.YearMonth >= yearMonthStart && r.YearMonth <= yearMonthEnd)
                .ToList();
        }

        private DateTime GetStartDayOfMonth(DateTime end)
        {
            return new DateTime(end.Year, end.Month, 1);
        }

        private DateTime GetEndDayOfMonth(DateTime start)
        {
            return new DateTime(start.Year, start.Month, 1).AddMonths(1).AddDays(-1);
        }

        private int CalculateBudgetSum(DateTime start, DateTime end, int budgetAmount)
        {
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var days = CalculateDiffDays(start, end);
            var budgetSum = budgetAmount * days / daysInMonth;
            return budgetSum;
        }

        private int CalculateDiffDays(DateTime start, DateTime end)
        {
            return end.DayOfYear - start.DayOfYear + 1;
        }
    }

    public class QueryBudget
    {
        public int YearMonth { get; set; }
        public int Amount { get; set; }
    }
}