using System;
//a class called demonstratingclass is defined and it implements the IDidposable interface
public class demonstratingclass : IDisposable
{
    //the dispose() method is defined to to perform the release of the required resources
    public void Dispose()
    {
        Console.WriteLine("The dispose() function is releasing the specified resources freeing them from the memory.");
    }
}
//another class called demofordispose is defined
public class demofordispose
{
    //main method is called
    public static void Main()
    {
        //an instance of the demonstratingclass is created
        var check1 = new demonstratingclass();
        // 
        check1.Dispose();
    }
}