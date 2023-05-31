using System;

namespace Overloading
{
    class Car
    {

        // constructor with no parameter
        Car()
        {
            Console.WriteLine("Car constructor");
        }

        // constructor with one parameter
        Car (string brand)
        {
            Console.WriteLine("Car constructor with one parameter");
            Console.WriteLine("Brand: " + brand);
        }

        // constructor  with int parameter
        Car (int price)
        {
            Console.WriteLine("Price: " + price);
        }

        //static void Main(string[] args)
        //{

        //    // call constructor  with string parameter
        //    Car car = new Car("Lamborghini");

        //    Console.WriteLine();

        //    // call constructor  with int parameter
        //    Car car2 = new Car(50000);

        //    Console.ReadLine();
        //}

    }

    class Program
    {

        // method with one parameter
        void display(int a)
        {
            Console.WriteLine("Arguments: " + a);
        }

        // method with two parameters
        void display(int a, int b)
        {
            Console.WriteLine("Arguments: " + a + " and " + b);
        }

        // method with string parameter
        void display(string b)
        {
            Console.WriteLine("string type: " + b);
        }

        // method with int and string parameters 
        void display(int a, string b)
        {
            Console.WriteLine("int: " + a);
            Console.WriteLine("string: " + b);
        }

        // method with string andint parameter
        void display(string b, int a)
        {
            Console.WriteLine("string: " + b);
            Console.WriteLine("int: " + a);
        }
                       
        static void Main(string[] args)
        {

            Program p1 = new Program();
            p1.display(100);
            p1.display(100, 200);
            p1.display("abc");
            p1.display(100, "abc");
            p1.display("abc", 400);
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}