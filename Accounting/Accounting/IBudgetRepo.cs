using System.Collections.Generic;

namespace Accounting
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}