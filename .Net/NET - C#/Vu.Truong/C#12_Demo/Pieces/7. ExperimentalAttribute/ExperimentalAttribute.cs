using System.Diagnostics.CodeAnalysis;

namespace C_12_Demo.Pieces;

[Experimental("DEMO0006")]
internal class ExperimentalAttributes
{
    public void Execute()
    {

    }
}


internal class TestExperimental
{
    public static void Execute()
    {
#pragma warning disable DEMO0006 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        var test = new ExperimentalAttributes();
#pragma warning restore DEMO0006 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        test.Execute();
    }
}