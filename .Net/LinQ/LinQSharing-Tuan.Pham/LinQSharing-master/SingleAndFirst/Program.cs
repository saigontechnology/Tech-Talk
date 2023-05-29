namespace SingleAndFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var users = new List<string>
            {
                "David",
                "John",
                "Will"
            };
            var single = users.SingleOrDefault(x => x.StartsWith("D"));


        }
    }
}