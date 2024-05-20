namespace BestPractices;

public class Calculator
{
    public int Add(int a, int b)
    {
        const int MAX_INPUT = 100000;
        if (a > MAX_INPUT || b > MAX_INPUT)
        {
            throw new ArgumentOutOfRangeException("The input value cannot greater than " + MAX_INPUT);
        }
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public float Divide(float a, float b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException();
        }
        return a / b;
    }
}
