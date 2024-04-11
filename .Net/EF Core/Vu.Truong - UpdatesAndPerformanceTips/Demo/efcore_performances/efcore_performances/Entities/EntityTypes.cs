namespace efcore_performances.Entities;
public enum Currency
{
    UsDollars,
    PoundsSterling
}

public enum DocumentBranchType
{
    Folder,
    File
}

public enum DbContextType
{
    Normal,
    Pooling,
    Factory,
    PooledFactory
}