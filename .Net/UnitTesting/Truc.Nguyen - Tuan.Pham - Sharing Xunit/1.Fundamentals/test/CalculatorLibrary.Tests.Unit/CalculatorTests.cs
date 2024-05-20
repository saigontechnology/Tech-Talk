using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CalculatorLibrary.Tests.Unit;

public class CalculatorTests : IAsyncLifetime
{
    private readonly Calculator _sut = new();
    private readonly ITestOutputHelper _outputHelper;
    
    // Setup goes here
    public CalculatorTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _outputHelper.WriteLine("Hello from ctor");
    }
    
    [Fact]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers()
    {
        // Arrange

        // Act

        // Assert
    }
    public async Task InitializeAsync()
    {
        await Task.Delay(200);
    }
    public async Task DisposeAsync()
    {
        _outputHelper.WriteLine("Hello from cleanup");
    }

    // [Theory]
    // [InlineData(5, 5, 10)]
    // [InlineData(-5, 5, 0)]
    // [InlineData(-15, -5, -20)]
    // public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(
    //     int a, int b, int expected)
    // {
    //     // Act
    //     var result = _sut.Add(a, b);
    //
    //     // Assert
    //     Assert.Equal(expected, result);
    // }

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
        Assert.Equal(expected, result);
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
        Assert.Equal(expected, result);
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
        Assert.Equal(expected, result);
    }
    
}
