using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{

    // Cat extends from Animal and implements 2 interface CanEat, CanDrink.
    public class Cat : Animal, ICanEat, ICanDrink
    {

        private String name;

        public Cat(String name)
        {
            this.name = name;
        }

        public String getName()
        {
            return this.name;
        }

        // Implements abstract method of Animal.
        // (Must specify the 'override').
        public override void Back()
        {
            Console.WriteLine(name + " cat back ...");
        }

        // Implements abstract method of Animal.
        // (Must specify the 'override').
        public override int GetVelocity()
        {
            return 110;
        }

        // Implements abstract method of CanEat.
        public void Eat()
        {
            Console.WriteLine(name + " cat eat ...");
        }

        // Implements abstract method of CanDrink.
        public void Drink()
        {
            Console.WriteLine(name + " cat drink ...");
        }

    }

}
