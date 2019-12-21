using System;
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

            var budgetList = _budgetRepo.GetAll();

            var days = QueryDays(start, end);

            var yearMonthStart = Convert.ToInt32(start.ToString("yyyyMM"));
            var yearMonthEnd =  Convert.ToInt32(end.ToString("yyyyMM"));

            var amountList = budgetList.Select(budget => new
            {
                YearMonth = Convert.ToInt32(budget.YearMonth),
                Amount = budget.Amount
            }).Where(r => r.YearMonth >= yearMonthStart && r.YearMonth <= yearMonthEnd)
                .ToList();
            if (yearMonthStart == yearMonthEnd)
            {

                var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
                return (amountList.FirstOrDefault()?.Amount ?? 0) * days / daysInMonth;
            }
            
            var sum = 0m;
            foreach (var budget in amountList)
            {
                if (budget.YearMonth == yearMonthStart)
                {
                    var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
                    days = QueryDays(start, new DateTime(start.Year, start.Month, 1).AddMonths(1).AddDays(-1));
                    sum += budget.Amount * days / daysInMonth;
                }
                else if(budget.YearMonth == yearMonthEnd)
                {
                    var daysInMonth = DateTime.DaysInMonth(end.Year, end.Month);
                    days = QueryDays(new DateTime(end.Year,end.Month,1), end);
                    sum += budget.Amount * days / daysInMonth;
                }
                else
                {
                    sum += budget.Amount;
                }
            }

            return sum;
        }

        private static int QueryDays(DateTime start, DateTime end)
        {
            return end.DayOfYear - start.DayOfYear + 1;
        }
    }
}