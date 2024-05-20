using FluentAssertions;

namespace BestPractices.Tests.Unit
{
    public class AvoidMultipleTestsInOne
    {
        [Fact]
        public void Divide_ShouldReturnZeroOrThrowException_WhenInputIsZero()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var resultShouldBeZero = calculator.Divide(0, 1);
            var requestAction = () => calculator.Divide(1, 0);

            // Assert
            resultShouldBeZero.Should().Equals(0);
            requestAction.Should()
                .Throw<DivideByZeroException>();
        }

        [Fact]
        public void Divide_ShouldReturnZero_WhenNumeratorIsZero()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Divide(0, 1);

            // Assert
            result.Should().Equals(0);
        }

        [Fact]
        public void Divide_ShouldThrowException_WhenDenominatorIsZero()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var requestAction = () => calculator.Divide(1, 0);

            // Assert
            requestAction.Should()
                .Throw<DivideByZeroException>();
        }
    }
}
