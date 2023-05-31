class Program
{
    static void Main()
    {
        var testThread = new AsyncAwaitExample();
        testThread.DoWork();

        while (true)
        {
            Console.WriteLine("Doing work on the Main Thread !!");
        }

        //returns a Task<int> object:
        Task<int> value = DoAsyncWork.CalculateSumAsync(10, 11);
        //Do Other processing
        Console.WriteLine("Value: {0}", value.Result);

        value.Wait();
        Console.WriteLine("Async stuff is done");
    }
}

public class AsyncAwaitExample
{
    public async Task DoWork()
    {
        await Task.Run(() => {
            int counter;

            for (counter = 0; counter < 1000; counter++)
            {
                Console.WriteLine(counter);
            }
        });
    }

}

static class DoAsyncWork
{
    public static async Task<int> CalculateSumAsync(int i1, int i2)
    {
        int sum = await Task.Run(() => GetSum(i1, i2));
        //Console.WriteLine("Value: {0}", sum);
        return sum;
    }

    private static int GetSum(int i1, int i2)
    {
        return i1 + i2;
    }
}
