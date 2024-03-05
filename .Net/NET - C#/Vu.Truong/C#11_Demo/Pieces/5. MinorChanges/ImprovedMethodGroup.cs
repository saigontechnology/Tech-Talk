namespace C_11_Demo.MinorChanges;
internal class ImprovedMethodGroup
{
    private readonly List<int> Numbers;

    public ImprovedMethodGroup()
    {
        Numbers = Enumerable.Range(1, 100).ToList();
    }

    public int Sum()
    {
        return Numbers.Where(x => x > 0).Sum();
    }

    public int SumFilter()
    {
        return Numbers.Where(x => Filter(x)).Sum();
    }

    /// <summary>
    /// Before C#11: performance is not good because it's not cached
    /// C#11: performance is same with other, it's cached
    /// </summary>
    /// <returns></returns>
    public int SumFilterMethodGroup()
    {
        return Numbers.Where(Filter).Sum();
    }

    private static bool Filter(int number)
    {
        return number > 0;
    }
}
