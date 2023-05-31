class MyClass
{
    public int Get20() // Func<int> compatible
    {
        return 20;
    }
    public async Task DoWorkAsync()
    {
        Func<int> twenty = new Func<int>(Get20);
        int a = await Task.Run(twenty);
        int b = await Task.Run(new Func<int>(Get20));
        int c = await Task.Run(() => { return 20; });
        Console.WriteLine("{0} {1} {2}", a, b, c);
    }
    class Program
    {
        static void Main()
        {
            Task t = (new MyClass()).DoWorkAsync();
            t.Wait();
        }
    }
}