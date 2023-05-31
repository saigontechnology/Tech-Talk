public class Program
{
    class Employee
    {
        public string Name { get; set; }
    }

    public static void Main(string[] args)
    {
        //Example - Value Type Comparision based on Content
        int x = 2;
        int y = 2;

        Console.WriteLine(x == y); //True
        Console.WriteLine(x.Equals(y)); //True

        //Example - Value Type Comparision
        int m = 2;
        Console.WriteLine(m == 2.0); //True
        Console.WriteLine(m.Equals(2.0)); //False


        //Example - Referenced Based Comparision
        Employee obj1 = new Employee();
        obj1.Name = "Tutorialsrack";

        Employee obj2 = new Employee();
        obj2.Name = "Tutorialsrack";

        Console.WriteLine(obj1 == obj2); //False
        Console.WriteLine(obj1.Equals(obj2)); //False

        //Example - Referenced Based Comparision
        Employee obj3 = new Employee();
        obj1.Name = "Tutorialsrack";

        Employee obj4 = obj1;

        Console.WriteLine(obj3 == obj4); //True
        Console.WriteLine(obj3.Equals(obj4)); //True

        //Example - Compare Two String on Content based
        object str1 = "tutorialsrack";
        object str2 = "tutorialsrack";

        Console.WriteLine(str1 == str2);  //True
        Console.WriteLine(str1.Equals(str2));  //True

        object str3 = new string(new char[] { 't', 'u', 't', 'o', 'r', 'i', 'a', 'l' });
        object str4 = new string(new char[] { 't', 'u', 't', 'o', 'r', 'i', 'a', 'l' });

        Console.WriteLine(str3 == str4);  //False
        Console.WriteLine(str3.Equals(str4));  //True
    }
}