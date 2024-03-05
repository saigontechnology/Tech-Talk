namespace C_10_Demo.Pieces;
public class ConstantsInterpolatedStrings
{
    const string EmailSubject = $"This is template. Please place {{{{{Email}}}}} and {{{{{Password}}}}} with your content";

    public static void Execute()
    {
        Console.WriteLine(EmailSubject);
    }
}