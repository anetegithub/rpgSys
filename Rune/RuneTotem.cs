using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneFramework
{
    public sealed class RuneTotem<T>
    {
        RuneTotem()
        { }

        public static RuneSpirit<T> Totem
        {
            get
            {
                if (typeof(T) != typeof(RuneString))
                    return Nested.instance;
                else
                    return new RuneSpirit<T>();
            }
        }

        class Nested
        {
            static Nested()
            { }

            internal static readonly RuneSpirit<T> instance = new RuneSpirit<T>();
        }
    }
}
