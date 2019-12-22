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

            var budgetList = QueryBudgetRepo(start, end);

            foreach (var budget in budgetList)
            {
                budgetSum += budget.EffectiveBudget(start, end);
            }

            return budgetSum;
        }

        private List<Budget> QueryBudgetRepo(DateTime start, DateTime end)
        {
            var yearMonthStart = start.ToString("yyyyMM");
            var yearMonthEnd = end.ToString("yyyyMM");
            var budgetList = _budgetRepo.GetAll()
                .Where(r => string.CompareOrdinal(r.YearMonth, yearMonthStart) >= 0
                            && string.CompareOrdinal(r.YearMonth, yearMonthEnd) <= 0)
                .ToList();
            return budgetList;
        }
    }
}