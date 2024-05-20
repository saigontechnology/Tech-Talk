using System.Collections;
using System.Collections.Generic;
using AdvancedTechniques;
using Xunit;

namespace AdvancedTechniques.Tests.Unit;

public class CalculatorTests
{
    private readonly Calculator _sut = new();

    [Theory]
    // [InlineData(5, 5, 10)]
    // [InlineData(-5, 5, 0)]
    // [InlineData(-15, -5, -20)]
    [MemberData(nameof(AddTestData))]
    public void Add_ShouldAddTwoNumbers_WhenTheNumbersAreValidIntegers(
        int firstNumber, int secondNumber, int expectedResult)
    {
        // Act
        var result = _sut.Add(firstNumber, secondNumber);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    // [InlineData(5, 5, 0)]
    // [InlineData(-5, -5, 0)]
    // [InlineData(-15, -5, -10)]
    // [InlineData(15, 5, 10)]
    // [InlineData(5, 10, -5)]
    [ClassData(typeof(CalculatorSubtractTestData))]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTheNumbersAreValidIntegers(
        int firstNumber, int secondNumber, int expectedResult)
    {
        // Act
        var result = _sut.Subtract(firstNumber, secondNumber);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static IEnumerable<object[]> AddTestData =>
        new List<object[]>
        {
            new object[] { 5, 5, 10 },
            new object[] { -5, 5, 0 },
            new object[] { -15, -5, -20 }
        };
}

public class CalculatorSubtractTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 5, 5, 0 };
        yield return new object[] { -5, -5, 0 };
        yield return new object[] { -15, -5, -10 };
        yield return new object[] { 15, 5, 10 };
        yield return new object[] { 5, 10, -5 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
