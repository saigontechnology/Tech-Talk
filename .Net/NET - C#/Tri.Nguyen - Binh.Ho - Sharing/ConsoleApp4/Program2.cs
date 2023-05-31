using System;

namespace Overriding
{

    // base class
    class Animal
    {

        public string name;

        public void display()
        {
            Console.WriteLine("I am an animal");
        }

    }

    // derived class of Animal 
    class Dog : Animal
    {

        public void getName()
        {
            Console.WriteLine("My name is " + name);
        }
    }

    class Program
    {

        static void Main(string[] args)
        {

            // object of derived class
            Dog labrador = new Dog();

            // access field and method of base class
            labrador.name = "Rohu";
            labrador.display();

            // access method from own class
            labrador.getName();

            Console.ReadLine();
        }

    }
}