using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{

    // This class extends from Object.
    // And implements CanMove interface.
    // CanMove has 3 abstract methods.
    // This class implements only one abstract method of CanMove.
    // Therefore it must be declared as 'abstract'.
    // The remaining methods must be declared with 'public abstract'.
    public abstract class Animal : ICanMove
    {

        // Implements Run() method of CanMove.
        // You have to write the contents of the method.
        // Access modifier must be public.
        public void Run()
        {
            Console.WriteLine("Animal run...");
        }

        // If this class does not implements a certain method of Interface
        // you have to rewrite it as an abstract method.
        // (Always is 'public abstract').
        public abstract void Back();

        public abstract int GetVelocity();

    }


}
