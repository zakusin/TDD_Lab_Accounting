using System;

namespace Accounting
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
        private DateTime BudgetFirstDay => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        private DateTime BudgetLastDay => BudgetFirstDay.AddMonths(1).AddDays(-1);
        private int BudgetDays => DateTime.DaysInMonth(BudgetFirstDay.Year, BudgetFirstDay.Month);

        public decimal EffectiveBudget(DateTime start, DateTime end)
        {
            var effectiveStartDate = start < BudgetFirstDay 
                ? BudgetFirstDay 
                : start;
            var effectiveEndDate = end > BudgetLastDay 
                ? BudgetLastDay 
                : end;
            var effectiveDays = (effectiveEndDate - effectiveStartDate).Days + 1;
            return Amount * effectiveDays / BudgetDays;
        }
    }
}