using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Collections;

namespace RuneFramework
{
    public interface Letter<T> : IDisposable
    {
        bool NeedRune();
        void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune);
        void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property);
        void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property);
        void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property);
    }

    public class GenericListsLetter<T> : Letter<T>
    {
        public bool NeedRune()
        { return true; }

        public bool NeedRuneChanges()
        { return true; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {
            if ((ObjectAtRunic as IDictionary<string, object>).ContainsKey(Property.Name))
            {
                if ((ObjectAtRunic as IDictionary<string, object>)[Property.Name] != "")
                {
                    RuneList RList = (RuneList)(ObjectAtRunic as IDictionary<string, object>)[Property.Name];
                    var ListOfItems = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

                    foreach (var Item in RList.List)
                    {
                        int Id = Int32.Parse(Item.ToString());
                        string IdName;

                        if (Property.PropertyType.GetGenericArguments()[0].GetProperty("Id") == null)
                            IdName = Property.PropertyType.Name + "Id";
                        else
                            IdName = "Id";


                        foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                        {
                            if (RuneWord.Name == Property.Name)//Property.PropertyType.GetGenericArguments()[0].Name)
                            {
                                var Value = RuneWord.GetValue(Rune, null)
                                    .GetType()
                                    .GetMethod("QueryUniq")
                                    .Invoke(RuneWord.GetValue(Rune, null),
                                    new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });


                                ListOfItems.Add(Value);
                            }
                        }
                    }

                    Property.SetValue(Object, ListOfItems);
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {

        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            RuneList RList = new RuneList() { List = new List<object>(), TypeName = Property.PropertyType.GetGenericArguments()[0].Name };

            var List = (IList)Property.GetValue(Object, null);
            if (List != null)
                foreach (var Item in List)
                {
                    string IdName;
                    if (Property.PropertyType.GetGenericArguments()[0].GetProperty("Id") == null)
                        IdName = Property.PropertyType.Name + "Id";
                    else
                        IdName = "Id";

                    RList.List.Add(Item.GetType().GetProperty(IdName).GetValue(Item, null).ToString());
                }

            (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, RList);
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            Result = false;

            var ListA = (IList)Property.GetValue(ObjectA, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));
            var ListB = (IList)Property.GetValue(ObjectB, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

            if (ListA.Count != ListB.Count)
            {
                Result = true;
                return;
            }

            IEnumerator EnumA = ListA.GetEnumerator();
            IEnumerator EnumB = ListB.GetEnumerator();

            while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
            {
                if (ObjectA.GetType().GetInterface("IList") != null)
                    FieldByFieldCompareCollection(out Result, Property, EnumA.Current, EnumA.Current);
                else
                    if(Property.PropertyType.GetInterface("IList") != null)
                        FieldByFieldCompare(out Result, Property.PropertyType.GetGenericArguments()[0], EnumA.Current, EnumB.Current);
                else
                        FieldByFieldCompare(out Result, Property.PropertyType, EnumA.Current, EnumB.Current);
            }
        }

        private void FieldByFieldCompareCollection(out bool Result, PropertyInfo Property, object A, object B)
        {
            Result = false;
            foreach (var InnerProperty in Property.PropertyType.GetGenericArguments()[0].GetProperties())
            {
                var AProperty = InnerProperty.GetValue(A, null);
                var BProperty = InnerProperty.GetValue(B, null);

                if (AProperty == null && BProperty == null)
                {
                    Result = false;
                    return;
                }

                if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                {
                    Result = true;
                    return;
                }

                if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                {
                    if (InnerProperty.PropertyType == typeof(String))
                    {
                        AProperty = AProperty ?? "";
                        BProperty = BProperty ?? "";
                    }

                    if(AProperty==null)

                    if (AProperty.Equals(BProperty))
                        Result = false;
                    else
                        Result = true;
                }
                else
                {
                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(out Result, InnerProperty.PropertyType, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);

                    //FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                }
            }
        }

        private void FieldByFieldCompare(out bool Result, Type Property, object A, object B)
        {
            Result = false;

            foreach (var InnerProperty in Property.GetProperties())
            {
                var AProperty = InnerProperty.GetValue(A, null);
                var BProperty = InnerProperty.GetValue(B, null);

                if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                {
                    if (InnerProperty.PropertyType == typeof(String))
                    {
                        AProperty = AProperty ?? "";
                        BProperty = BProperty ?? "";
                    }

                    if (AProperty.Equals(BProperty))
                        Result = false;
                    else
                        Result = true;
                }
                else
                {
                    if (AProperty == null && BProperty == null)
                    {
                        Result = false;
                        return;
                    }

                    if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                    {
                        Result = true;
                        return;
                    }

                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(out Result, InnerProperty.PropertyType, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                    //NeedChangesCollection(out Result, AProperty, BProperty, InnerProperty);
                }
            }
        }


        public void Dispose()
        { }
    }

    public class PrimitiveListsLetter<T> : Letter<T>
    {

        public bool NeedRune()
        { return false; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {

        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            if ((ObjectAtRunic as IDictionary<string, object>).ContainsKey(Property.Name))
            {
                if ((ObjectAtRunic as IDictionary<string, string>)[Property.Name] != "")
                {
                    RuneList RList = (RuneList)(ObjectAtRunic as IDictionary<string, object>)[Property.Name];

                    Type ItemsType = Type.GetType("System." + RList.TypeName);
                    if (ItemsType != null)
                    {
                        var ListOfItems = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ItemsType));

                        foreach (var Item in RList.List)
                        {
                            ListOfItems.Add(Convert.ChangeType(Item, ItemsType));
                        }

                        Property.SetValue(Object, ListOfItems);
                    }
                }
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            if (Property.PropertyType.GetGenericArguments()[0].IsPrimitive || Property.PropertyType.GetGenericArguments()[0] == typeof(String))
            {
                RuneList RList = new RuneList() { List = new List<object>(), TypeName = Property.PropertyType.GetGenericArguments()[0].Name };

                var List = (IList)Property.GetValue(Object, null);
                if (List != null)
                    foreach (var Item in List)
                    {
                        RList.List.Add(Item);
                    }

                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, RList);
            }
            //generic
            else
            {

            }
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            Result = false;

            var ListA = (IList)Property.GetValue(ObjectA, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));
            var ListB = (IList)Property.GetValue(ObjectB, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

            if (ListA.Count != ListB.Count)
            {
                Result = true;
                return;
            }

            if (Property.PropertyType.GetGenericArguments()[0].IsPrimitive || Property.PropertyType.GetGenericArguments()[0] == typeof(String))
            {
                IEnumerator EnumA = ListA.GetEnumerator();
                IEnumerator EnumB = ListB.GetEnumerator();

                while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                {
                    if (EnumA.Current.Equals(EnumB.Current))
                        Result = false;
                    else
                        Result = true;

                }
            }
        }

        public void Dispose()
        { }
    }

    public class PrimitiveLetter<T> : Letter<T>
    {

        public bool NeedRune()
        { return false; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        { }

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

        public bool NeedRune()
        { return true; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    int Id = Int32.Parse(Field.Value.ToString());
                    string IdName;

                    if (Property.PropertyType.GetProperty("Id") == null)
                        IdName = Property.PropertyType.Name + "Id";
                    else
                        IdName = "Id";

                    foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                    {
                        if (RuneWord.Name == Property.Name)
                        {
                            var Value = RuneWord.GetValue(Rune, null)
                                .GetType()
                                .GetMethod("QueryUniq")
                                .Invoke(RuneWord.GetValue(Rune, null),
                                new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });

                            Property.SetValue(Object, Value);
                        }
                    }
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    RuneString Value = Int32.Parse(Field.Value.ToString());

                    /* Perfomance
                     * 
                     * 2 Items and Id=1:
                     * Foreach : ~ 601-687
                     * Query : ~ 1008-1144
                     * 
                     * 52 Items and Id=50:
                     * Foreach : ~ 1312 - 2006
                     * Query : ~ 1740 - 2417
                     * 
                     * 52 Items and Id=25:
                     * Foreach : ~ 1233 - 1305
                     * Query : ~ 1821 - 2581
                     *
                     */

                    if (typeof(T) != typeof(RuneString))
                    {
                        RuneWord<RuneString> Runum = new RuneWord<RuneString>(Property.Name, null);
                        //var Query = Runum.Query(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", Value.Id.ToString()) } });
                        //Value.Value = Query[0].Value;
                        foreach (var RunumItem in Runum)
                        {
                            if (RunumItem.Id == Value.Id)
                            {
                                Value = new RuneString(RunumItem.Value);
                            }
                        }
                    }

                    Property.SetValue(Object, Value);
                }
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
            RuneString A = (RuneString)Property.GetValue(ObjectA, null) ?? 0;
            RuneString B = (RuneString)Property.GetValue(ObjectB, null) ?? 0;

            if (A.Equals(B))
                Result = false;
            else
                Result = true;
        }

        public void Dispose()
        { }
    }

    public class ClassLetter<T> : Letter<T>
    {
        public bool NeedRune()
        { return true; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    int Id = Int32.Parse(Field.Value.ToString());
                    string IdName;

                    if (Property.PropertyType.GetProperty("Id") == null)
                        IdName = Property.PropertyType.Name + "Id";
                    else
                        IdName = "Id";

                    foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                    {
                        if (RuneWord.Name == Property.Name)
                        {
                            var Value = RuneWord.GetValue(Rune, null)
                                .GetType()
                                .GetMethod("QueryUniq")
                                .Invoke(RuneWord.GetValue(Rune, null),
                                new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });

                            Property.SetValue(Object, Value);
                        }
                    }
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        { }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);

            string Id;
            if (Property.PropertyType.GetProperty("Id") == null)
                Id = Property.PropertyType.Name + "Id";//typeof(T).Name + "Id";
            else
                Id = "Id";

            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value.GetType().GetProperty(Id).GetValue(Value, null).ToString());
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            var A = Property.GetValue(ObjectA, null) ?? Activator.CreateInstance(Property.PropertyType);
            var B = Property.GetValue(ObjectB, null) ?? Activator.CreateInstance(Property.PropertyType);

            Result = true;
                        
            ABEquals(A, B, Property.PropertyType);
            //FieldByFieldCompare(ref Result, Property, A, B);
        }

        private bool ABEquals(object A, object B, Type TheyType)
        {
            // uses Reflection to check if a Type-specific `Equals` exists...
            var specificEquals = TheyType.GetMethod("Equals", new Type[] { TheyType });
            if (specificEquals != null &&
                specificEquals.ReturnType == typeof(bool))
            {
                return (bool)specificEquals.Invoke(A, new object[] { B });
            }
            return A.Equals(B);
        }

        private void FieldByFieldCompare(ref bool Result, PropertyInfo Property, object A, object B)
        {
            foreach (var InnerProperty in Property.PropertyType.GetProperties())
            {
                var AProperty = InnerProperty.GetValue(A, null);
                var BProperty = InnerProperty.GetValue(B, null);

                if (AProperty == null && BProperty == null)
                {
                    Result = false;
                    return;
                }

                if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                {
                    //Result = true;
                    return;
                }

                if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                {
                    if (InnerProperty.PropertyType == typeof(String))
                    {
                        AProperty = AProperty ?? "";
                        BProperty = BProperty ?? "";
                    }

                    if (AProperty.Equals(BProperty))
                        Result = false;
                    //else
                    //    Result = true;
                }
                else
                {                   

                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(ref Result, InnerProperty, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                        //NeedChangesCollection(out Result, AProperty, BProperty, InnerProperty);
                }
            }
        }

        private void NeedChangesCollection(out bool Result, object ObjectA, object ObjectB, PropertyInfo Property)
        {
            Result = false;

            var ListA = (IList)Property.GetValue(ObjectA, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));
            var ListB = (IList)Property.GetValue(ObjectB, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

            if (ListA.Count != ListB.Count)
            {
                Result = true;
                return;
            }

            IEnumerator EnumA = ListA.GetEnumerator();
            IEnumerator EnumB = ListB.GetEnumerator();

            while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
            {
                FieldByFieldCompareCollection(out Result, Property, EnumA.Current, EnumA.Current);
            }
        }

        private void FieldByFieldCompareCollection(out bool Result, PropertyInfo Property, object A, object B)
        {
            Result = false;
            foreach (var InnerProperty in Property.PropertyType.GetGenericArguments()[0].GetProperties())
            {
                IEnumerator EnumA = (A as IEnumerable).GetEnumerator();
                IEnumerator EnumB = (B as IEnumerable).GetEnumerator();

                while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                {
                    var AProperty = InnerProperty.GetValue(EnumA.Current, null);
                    var BProperty = InnerProperty.GetValue(EnumB.Current, null);

                    if (AProperty == null && BProperty == null)
                    {
                        Result = false;
                        return;
                    }

                    if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                    {
                        Result = true;
                        return;
                    }

                    if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                    {
                        if (InnerProperty.PropertyType == typeof(String))
                        {
                            AProperty = AProperty ?? "";
                            BProperty = BProperty ?? "";
                        }

                        if (AProperty == null)

                            if (AProperty.Equals(BProperty))
                                Result = false;
                            else
                                Result = true;
                    }
                    else
                    {
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                    }
                }
            }
        }


        public void Dispose()
        { }
    }

    public class StringLetter<T> : Letter<T>
    {
        public bool NeedRune()
        { return false; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        { }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                    Property.SetValue(Object, Convert.ChangeType(Field.Value.ToString(), Property.PropertyType));
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
            string A = (string)Property.GetValue(ObjectA, null) ?? "";
            string B = (string)Property.GetValue(ObjectB, null) ?? "";

            if (A.Equals(B))
                Result = false;
            else
                Result = true;
        }

        public void Dispose()
        { }
    }
}