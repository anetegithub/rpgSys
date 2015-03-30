using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Dynamic;

namespace RuneFramework
{
    public class RuneString : IEquatable<RuneString>
    {
        [Obsolete("Enabled only for run-time.", true)]
        public RuneString()
        { }

        private RuneString(int i)
        {
            Id = i;
        }

        public RuneString(string s)
        { Value = s; }

        public int Id { get; set; }
        public string Value { get; set; }

        public static explicit operator int(RuneString s)
        {
            if (s == null)
                throw new NullReferenceException();

            return s.Id;
        }

        public static implicit operator string(RuneString s)
        {
            if (s == null)
                throw new NullReferenceException();

            return s.Value;
        }

        public static implicit operator RuneString(int i)
        {
            return new RuneString(i) { Value = "VALUE_NOT_FOUND" };
        }

        public static implicit operator RuneString(string s)
        { return new RuneString(s); }

        public bool Equals(RuneString other)
        {
            if (this.Id == other.Id && this.Value == this.Value)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            if (this == null)
                throw new NullReferenceException();

            return this.Id.ToString();
        }
    }
}