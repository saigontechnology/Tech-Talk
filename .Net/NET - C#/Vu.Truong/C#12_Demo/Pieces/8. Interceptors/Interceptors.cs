namespace C_12_Demo.Pieces;
public class InterceptorDemo
{
    public string GetText(string text)
    {
        return $"{text}, World!";
    }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class InterceptsLocationAttribute(string filePath, int line, int character) : Attribute
{
}

public static class GeneratedCode
{
    [InterceptsLocation("C:\\Projects\\VTRepositories\\dotnet\\dotnet8\\C#10_Demo\\C#12_Demo\\Program.cs", 7, 28)]
    public static string InterceptorMethod(this InterceptorDemo example, string text)
    {
        return $"{text}, intercepted !!!";
    }
}