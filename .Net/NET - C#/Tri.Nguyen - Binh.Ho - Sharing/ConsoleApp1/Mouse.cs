using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{
    public class Mouse : Animal, ICanEat, ICanDrink
    {

        // Implements abstract method of Animal
        // (Must have keyword 'override').
        public override void Back()
        {
            Console.WriteLine("Mouse back ...");
        }

        // Implements abstract method of Animal 
        public override int GetVelocity()
        {
            return 85;
        }

        // Implements abstract method of CanDrink.
        public void Drink()
        {
            Console.WriteLine("Mouse drink ...");
        }

        // Implements abstract method of CanEat.
        public void Eat()
        {
            Console.WriteLine("Mouse eat ...");
        }

    }
}
