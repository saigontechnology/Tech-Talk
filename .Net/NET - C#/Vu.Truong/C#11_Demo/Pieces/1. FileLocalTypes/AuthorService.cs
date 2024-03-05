namespace C_11_Demo.Pieces._1._FileLocalTypes;
internal class BlogUtil
{
    public static long Costs => new BlogService().GetCosts();

    public static void CheckBlogItemOverview()
    {
        var blogInfo = new BlogItem
        {
            Cost = Costs
        };

        if (blogInfo is { Cost: > 1000 })
        {
            // do something when cost is large
        }
        else
        {
            // do something else
        }
    }
}

internal class AuthorService
{
    public static long SystemCost => BlogUtil.Costs;
}

file class BlogItem
{
    public long Cost { get; set; }
}