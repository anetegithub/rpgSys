﻿using System;
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

    public class RuneSpell
    {
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

    public class RuneSpellage
    {
        public RuneSpellage(String Field, String Value)
        {
            this.Field = Field;
            this.Value = Value;
        }

        protected string Field { get; set; }
        protected string Value { get; set; }

        public void SetValue(ref XElement Element)
        {
            Element.Element(Field).Value = Value;
        }
    }
}