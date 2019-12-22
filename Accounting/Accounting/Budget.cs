using System;

namespace Accounting
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
        private DateTime BudgetFirstDate => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        private DateTime BudgetEndDate => BudgetFirstDate.AddMonths(1).AddDays(-1);
        private int BudgetDays => DateTime.DaysInMonth(BudgetFirstDate.Year, BudgetFirstDate.Month);

        public decimal EffectiveBudget(DateTime start, DateTime end)
        {
            var effectiveStartDate = start < BudgetFirstDate 
                ? BudgetFirstDate 
                : start;
            var effectiveEndDate = end > BudgetEndDate 
                ? BudgetEndDate 
                : end;
            var effectiveDays = (effectiveEndDate - effectiveStartDate).Days + 1;
            return Amount * effectiveDays / BudgetDays;
        }
    }
}