using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class NameValueAttribute : Attribute
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public NameValueAttribute(string name, object value = null)
        {
            Name = name;
            Value = value;
        }
    }
}
