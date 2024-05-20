using FluentAssertions;
using NSubstitute;

namespace BestPractices.Tests.Unit
{
    public class DateTimeNow
    {
        private readonly Greeter _sut;
        private readonly GreeterNaive _sutNaive;
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        public DateTimeNow()
        {
            _sut = new Greeter(_dateTimeProvider);
            _sutNaive = new GreeterNaive();
        }

        [Fact]
        public void GenerateGreetMessage_ShouldSayGoodAfternoon_WhenItsAfternoon()
        {
            // Arrange
            _dateTimeProvider.DateTimeNow.Returns(new DateTime(2020, 1, 1, 9, 0, 0));

            // Act
            var result = _sut.GenerateGreetMessage();

            // Assert
            result.Should().Be("Good morning");
        }

        [Fact]
        public void GenerateGreetMessage_ShouldSayGoodEvening_WhenItsEvening_Naive()
        {
            // Act
            var result = _sutNaive.GenerateGreetMessage();

            // Assert
            result.Should().Be("Good afternoon");
        }
    }
}
