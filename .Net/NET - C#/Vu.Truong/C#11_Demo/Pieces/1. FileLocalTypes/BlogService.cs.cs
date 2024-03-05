namespace C_11_Demo.Pieces._1._FileLocalTypes;
file class BlogUtil
{
    private static BlogUtil _instance;
    private List<BlogItem> _blogItems;

    private BlogUtil()
    {
        _blogItems = Enumerable.Range(0, 50)
            .Select(i => new BlogItem
            {
                Id = i,
                Name = $"Blog {i}",
                Cost = i * 10
            })
            .ToList();
    }

    public static BlogUtil Instance => _instance ?? (_instance = new BlogUtil());

    public long GetCosts()
    {
        return _blogItems.Sum(i => i.Cost);
    }

    public void Update(int id, int cost)
    {
        var blog = _blogItems.FirstOrDefault(x => x.Id == id);

        if (blog is not null)
            blog.Cost = cost;
        else 
            _blogItems.Add(
                new() { Cost = cost, Id = id, Name = $"New Blog { DateTime.Now.ToFileTime() }" }
            );
    }
}

file class BlogItem
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Cost { get; set; }
}

file interface IBlogService
{
    long GetCosts();

    void Update(int id, int cost);
}

internal class BlogService : IBlogService
{
    public long GetCosts()
    {
        return BlogUtil.Instance.GetCosts();
    }

    public void Update(int id, int cost)
    {
        BlogUtil.Instance.Update(id, cost);
    }
}