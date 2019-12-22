using System;

namespace Accounting
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
        private DateTime FirstDay => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        private DateTime LastDay => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null).AddMonths(1).AddDays(-1);
        private int BudgetDays => DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);

        public decimal GetEffectiveBudget(DateTime start, DateTime end)
        {
            var effectiveStartDate = start < FirstDay
                ? FirstDay
                : start;
            var effectiveEndDate = end > LastDay
                ? LastDay
                : end;
            var effectiveDays = (effectiveEndDate - effectiveStartDate).Days + 1;
            return Amount * effectiveDays / BudgetDays;
        }
    }
}