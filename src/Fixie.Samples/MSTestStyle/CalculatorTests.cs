using System;
using System.Text;
using Should;

namespace Fixie.Samples.MSTestStyle
{
    [TestClass]
    public class CalculatorTests : IDisposable
    {
        Calculator calculator;
        readonly StringBuilder log;

        bool executedAddTest = false;
        bool executedSubtractTest = false;

        public CalculatorTests()
        {
            log = new StringBuilder();
            log.WhereAmI();
        }

        [ClassInitialize]
        public void ClassInitialize()
        {
            log.WhereAmI();
            calculator = new Calculator();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            log.WhereAmI();
        }

        [TestMethod]
        public void ShouldAdd()
        {
            executedAddTest = true;
            log.WhereAmI();
            calculator.Add(2, 3).ShouldEqual(5);
        }

        [TestMethod]
        public void ShouldSubtract()
        {
            executedSubtractTest = true;
            log.WhereAmI();
            calculator.Subtract(5, 3).ShouldEqual(2);
        }

        [TestCleanUp]
        public void TestCleanUp()
        {
            log.WhereAmI();
        }

        [ClassCleanUp]
        public void ClassCleanUp()
        {
            log.WhereAmI();
        }

        public void Dispose()
        {
            log.WhereAmI();
            (executedAddTest && executedSubtractTest).ShouldBeFalse();
            (executedAddTest || executedSubtractTest).ShouldBeTrue();

            log.ShouldHaveLines(
                ".ctor",
                "ClassInitialize",
                "TestInitialize",
                executedAddTest
                    ? "ShouldAdd"
                    : "ShouldSubtract",
                "TestCleanUp",
                "ClassCleanUp",
                "Dispose");
        }
    }
}
