using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{
    // This interface defines a standard,
    // about things capable of moving.
    public interface ICanMove
    {

        // The methods of the Interface are public and abstract.
        // (But you are not allowed to write the public or abstract here).
        void Run();

        // Back
        void Back();

        // Return Velocity.
        int GetVelocity();

    }
}
