
namespace BestPractices.Tests.Unit
{
    public class ArrangingYourTests
    {
        [Fact]
        public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers()
        {
            // Arrange
            var calculator = new Calculator();

            // Assert
            Assert.Equal(-1, calculator.Subtract(1, 2));
        }

        [Fact]
        public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers_Arranged()
        {
            //Arrange
            var calculator = new Calculator();

            //Act
            var result = calculator.Subtract(1, 2);

            //Assert
            Assert.Equal(-1, result);
        }
    }
}
