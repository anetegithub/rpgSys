using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace RuneFramework
{
    public interface Letter<T> : IDisposable
    {
        void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property);
        void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property);
        void GetProperty(ref dynamic ObjectAtRunic, T Object, string Name, object Value);
    }

    public class PrimitiveLetter<T> : Letter<T> 
    {
        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (Object as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                    Property.SetValue(Object, Field.Value);
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value);
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, string Name, object Value)
        {
            (ObjectAtRunic as IDictionary<string, object>).Add(Name, Value);
        }

        public void Dispose()
        {}
    }
}
