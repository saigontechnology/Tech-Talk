namespace C_10_Demo.Pieces;

public class FileScopedNamespacesTest
{
    public int Number { get; set; }

    public void Init() { Number = 0; }
}

// Source file can only contain one file-scoped namespace declaration
// namespace C_10_Demo.Pieces2; -> error