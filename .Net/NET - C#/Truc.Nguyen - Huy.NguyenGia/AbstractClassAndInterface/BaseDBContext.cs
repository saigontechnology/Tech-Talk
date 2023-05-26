using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractClassAndInterface
{
    public abstract class BaseDBContext
    {
        protected virtual void OnConfiguring()
        {
            // do something
        }

        protected virtual void OnModelCreating()
        {
            // do something
        }
    }
}
