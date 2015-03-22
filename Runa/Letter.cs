﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;




using System.Diagnostics;

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
        { return false; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        { }

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

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property,Rune Rune)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    int Id = Int32.Parse(Field.Value.ToString());

                    foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                    {
                        if(RuneWord.Name==Property.Name)
                        {
                            var ListOfGenerics = typeof(RuneWord<>).GetMethod("Query").MakeGenericMethod(Property.PropertyType).Invoke(RuneWord.GetValue(Rune, null), new object[] { null });

                            var f = 5;
                            //foreach item in list ( if item.Id==Id then set value of this item)
                        }
                    }
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {}

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);

            string Id;
            if (typeof(T).GetProperty("Id") == null)
                Id = typeof(T).Name + "Id";
            else
                Id = "Id";
            
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value.GetType().GetProperty(Id).ToString());
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