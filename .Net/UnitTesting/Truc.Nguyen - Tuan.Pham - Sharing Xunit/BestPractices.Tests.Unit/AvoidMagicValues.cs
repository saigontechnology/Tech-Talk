using FluentAssertions;

namespace BestPractices.Tests.Unit
{
    public class AvoidMagicValues
    {
        [Fact]
        public void Add_ShouldThrowException_WhenInputExceedsMaximumNumber()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var requestAction = () => calculator.Add(100001, 2);

            // Assert
            requestAction.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Add_ShouldThrowException_WhenInputExceedsMaximumNumber_DefinedMagicValues()
        {
            // Arrange
            var calculator = new Calculator();
            const int MAX_INPUT = 100000;

            // Act
            var requestAction = () => calculator.Add(MAX_INPUT + 1, 2);

            // Assert
            requestAction.Should()
                .Throw<ArgumentOutOfRangeException>();
        }
    }
}
