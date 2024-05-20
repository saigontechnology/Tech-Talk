using FluentAssertions;
using System;
using Xunit;

namespace TestingTechniques.Tests.Unit;

public class CalculatorTests
{
    private readonly Calculator _sut = new();

    [Theory]
    [InlineData(5, 5, 10)]
    [InlineData(-5, 5, 0)]
    [InlineData(-15, -5, -20)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(
        int a, int b, int expected)
    {
        // Act
        var result = _sut.Add(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 0)]
    [InlineData(15, 5, 10)]
    [InlineData(-5, -5, 0)]
    [InlineData(-15, -5, -10)]
    [InlineData(5, 10, -5)]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers(
        int a, int b, int expected)
    {
        // Act
        var result = _sut.Subtract(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 25)]
    [InlineData(50, 0, 0)]
    [InlineData(-5, 5, -25)]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(
        int a, int b, int expected)
    {
        // Act
        var result = _sut.Multiply(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 1)]
    [InlineData(15, 5, 3)]
    public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreIntegers(
        int a, int b, int expected)
    {
        // Act
        var result = _sut.Divide(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(Skip = "Don't need to test")]
    [InlineData(5, 0)]
    [InlineData(15, 0)]
    public void Divide_ShouldThowException_WhenDenominatorIsZero(
        int a, int b)
    {
        // Act
        Action act = () => _sut.Divide(a, b);

        // Assert
        act.Should().Throw<DivideByZeroException>();
    }
}
