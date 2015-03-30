using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace RuneFramework
{
    public class RuneBook
    {
        public List<RuneSpell> Spells { get; set; }
        public List<RuneSpellage> Spellage { get; set; }
        public List<XElement> Elements { get; set; }
    }

    public class SimpleRuneBook
    {
        public List<SimpleRuneSpell> Spells { get; set; }
    }

    public class SpellEmptyException : NullReferenceException
    {
        public SpellEmptyException(string Message)
            : base(Message)
        { }
    }

    public class RuneSpell
    {
        public RuneSpell()
        {
            if (this.GetType() != typeof(SimpleRuneSpell))
                throw new SpellEmptyException("Current spell have empty field, operator and value!");
        }

        public RuneSpell(String Field, String Operator, Object Value)
        {
            this.Field = Field;
            this.Operator = Operator;
            this.Value = Value;
        }

        protected object Value { get; set; }
        protected object Instance { get; set; }
        protected string Operator { get; set; }
        protected string Field { get; set; }

        public bool Spell(XElement Source)
        {
            Instance = Convert.ChangeType(Source.Element(Field).Value, Value.GetType());
            switch (Operator)
            {
                case "==": return (Instance as IComparable).CompareTo(Value) == 0;
                case "!=": return (Instance as IComparable).CompareTo(Value) != 0;
                case ">": return (Instance as IComparable).CompareTo(Value) > 0;
                case ">=": return (Instance as IComparable).CompareTo(Value) >= 0;
                case "<": return (Instance as IComparable).CompareTo(Value) < 0;
                case "<=": return (Instance as IComparable).CompareTo(Value) <= 0;

                case "@": return (Instance as string).CompareToIn(Value) >= 0;
                case "!@": return (Instance as string).CompareToIn(Value) < 0;
                case "%": return (Instance as string).CompareToLike(Value) != 0;
                case "!%": return (Instance as string).CompareToLike(Value) != 0;

                default: return false;
            }
        }
    }

    public class SimpleRuneSpell : RuneSpell
    {
        public SimpleRuneSpell(String Field, String Operator, Object Value)
        {
            this.Field = Field;
            this.Operator = Operator;
            this.Value = Value;
        }
        public string GetField
        { get { return this.Field; } }
        public object GetValue
        { get { return this.Value; } }
        public bool Spell(Object Source)
        {
            Instance = Convert.ChangeType(Source.GetType().GetProperty(Field).GetValue(Source), Value.GetType());
            switch (Operator)
            {
                case "==": return (Instance as IComparable).CompareTo(Value) == 0;
                case "!=": return (Instance as IComparable).CompareTo(Value) != 0;
                case ">": return (Instance as IComparable).CompareTo(Value) > 0;
                case ">=": return (Instance as IComparable).CompareTo(Value) >= 0;
                case "<": return (Instance as IComparable).CompareTo(Value) < 0;
                case "<=": return (Instance as IComparable).CompareTo(Value) <= 0;

                case "@": return (Instance as string).CompareToIn(Value) >= 0;
                case "!@": return (Instance as string).CompareToIn(Value) < 0;
                case "%": return (Instance as string).CompareToLike(Value) != 0;
                case "!%": return (Instance as string).CompareToLike(Value) != 0;

                default: return false;
            }
        }
    }

    public class RuneSpellage
    {
        public RuneSpellage(String Field, Object Value)
        {
            this.Field = Field;
            this.Value = Value;
        }

        protected string Field { get; set; }
        protected Object Value { get; set; }

        /// <summary>
        /// If Element not exists create
        /// </summary>
        /// <param name="Element"></param>
        public void SetValue(ref XElement Element)
        {
            if (Value.GetType() != typeof(XElement))
                if (Element.Element(Field) != null)
                    Element.Element(Field).Value = Value.ToString();
                else
                    Element.Add(new XElement(Field, Value));
            else
            {
                if (Element.Element(Field) != null)
                    Element.Element(Field).ReplaceWith(Value);
                else
                    Element.Add(Value);
            }
        }
    }
}