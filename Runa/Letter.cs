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
        void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property);
    }

    public class PrimitiveLetter<T> : Letter<T>
    {
        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                    Property.SetValue(Object, Convert.ChangeType(Field.Value.ToString().Replace('.', ','), Property.PropertyType));
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value);
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            var A = Activator.CreateInstance(Property.PropertyType);
            var B = Activator.CreateInstance(Property.PropertyType);

            A = Property.GetValue(ObjectA, null);
            B = Property.GetValue(ObjectB, null);

            if (A.Equals(B))
                Result = false;
            else
                Result = true;
        }

        public void Dispose()
        { }
    }

    public class StringLetter<T> : Letter<T>
    {
        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                    Property.SetValue(Object, Convert.ChangeType(Field.Value.ToString().Replace('.', ','), Property.PropertyType));
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value);
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            var A = "";
            var B = "";

            A = Property.GetValue(ObjectA, null);
            B = Property.GetValue(ObjectB, null);

            if (A.Equals(B))
                Result = false;
            else
                Result = true;
        }

        public void Dispose()
        { }
    }
}
