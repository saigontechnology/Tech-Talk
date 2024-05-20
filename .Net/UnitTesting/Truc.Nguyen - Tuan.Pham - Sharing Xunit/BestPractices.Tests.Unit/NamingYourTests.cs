
namespace BestPractices.Tests.Unit
{
    public class NamingYourTests
    {
        [Fact]
        public void Test_Add()
        {
            var calculator = new Calculator();

            var actual = calculator.Add(1, 2);

            Assert.Equal(3, actual);
        }

        [Fact]
        public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers()
        {
            var calculator = new Calculator();

            var result = calculator.Add(1, 2);

            Assert.Equal(3, result);
        }
    }
}
