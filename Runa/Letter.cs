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

    public class RuneStringLetter<T> : Letter<T>
    {
        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                    Property.SetValue(Object, Enum.Parse(Property.PropertyType, Field.Value.ToString()));
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value.ToString());
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            object A = Property.GetValue(ObjectA, null);
            object B = Property.GetValue(ObjectA, null);

            if (A.ToString().Equals(B.ToString()))
                Result = false;
            else
                Result = true;
        }

        public void Dispose()
        { }
    }

    public class StringLetter<T> : PrimitiveLetter<T>
    {
        public new void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            string A = (string)Property.GetValue(ObjectA, null) ?? "";
            string B = (string)Property.GetValue(ObjectB, null) ?? "";

            if (A.Equals(B))
                Result = false;
            else
                Result = true;
        }
    }
}