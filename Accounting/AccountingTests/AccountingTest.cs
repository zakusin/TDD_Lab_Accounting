using System;
using System.Collections.Generic;
using Accounting;
using FluentAssertions;
using NUnit.Framework;

namespace AccountingTests
{
    [TestFixture]
    public class AccountingTest
    {
        private Accounting.Accounting _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Accounting.Accounting(new FakeBudgetRepo());
        }

        [Test]
        public void test_OneMonth_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2019, 12, 1), new DateTime(2019, 12, 31));
            QueryBudgetShouldBe(queryBudget, 310);
        }

        [Test]
        public void test_PartialMonth_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2019, 12, 1), new DateTime(2019, 12, 3));
            QueryBudgetShouldBe(queryBudget, 30);
        }

        [Test]
        public void test_CrossYear_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2019, 12, 30), new DateTime(2020, 1, 2));
            QueryBudgetShouldBe(queryBudget, 40);
        }

        [Test]
        public void test_CrossFullYear_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2019, 12, 1), new DateTime(2020, 12, 31));
            QueryBudgetShouldBe(queryBudget, 1240);
        }

        [Test]
        public void test_StartDate_GreaterThan_EndDate_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2021, 12, 31), new DateTime(2021, 12, 1));
            QueryBudgetShouldBe(queryBudget, 0);
        }

        [Test]
        public void test_NoData_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2021, 10, 1), new DateTime(2021, 10, 31));
            QueryBudgetShouldBe(queryBudget, 0);
        }

        [Test]
        public void test_CrossMonth_Budget()
        {
            var queryBudget = _sut.QueryBudget(new DateTime(2021, 11, 29), new DateTime(2021, 12, 1));
            QueryBudgetShouldBe(queryBudget, 40);
        }

        private void QueryBudgetShouldBe(decimal queryBudget, int expected)
        {
            Assert.AreEqual(expected, queryBudget);
        }
    }

    public class FakeBudgetRepo : IBudgetRepo
    {
        public List<Budget> GetAll()
        {
            return new List<Budget>()
            {
                new Budget(){YearMonth = "201912", Amount = 310},
                new Budget(){YearMonth = "202001", Amount = 310},
                new Budget(){YearMonth = "202012", Amount = 620},
                new Budget(){YearMonth = "202102", Amount = 930},
                new Budget(){YearMonth = "202111", Amount = 300},
                new Budget(){YearMonth = "202112", Amount = 620},
                new Budget(){YearMonth = "202201", Amount = 930},
            };
        }
    }
}
