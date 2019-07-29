using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static partial class Extentions
    {
        public static string ToName(this Enum value)
        {
            Type type = value.GetType();
            string res = ((NameValueAttribute)type.GetField(value.ToString()).GetCustomAttributes(typeof(NameValueAttribute), false).FirstOrDefault()).Name;
            return res;
        }
        public static object ToValue(this Enum value)
        {
            Type type = value.GetType();
            object tempValue = ((NameValueAttribute)type.GetField(value.ToString()).GetCustomAttributes(typeof(NameValueAttribute), false).FirstOrDefault()).Value;
            if (tempValue == null)
                tempValue = type.GetField(value.ToString()).GetValue(value);
            return tempValue;
        }
        public static List<NameValueInfo> GetNameValues<T>()
        {
            List<NameValueInfo> nameValues = new List<NameValueInfo>();
            Type type = typeof(T);
            Array arrays = Enum.GetValues(type);
            object item;
            System.Reflection.FieldInfo fieldInfo;
            NameValueAttribute nameValue;
            for (int i = 0; i < arrays.Length; i++)
            {
                item = arrays.GetValue(i);
                fieldInfo = item.GetType().GetField(item.ToString());
                if (!fieldInfo.IsDefined(typeof(NameValueAttribute), false))
                    continue;
                nameValue = ((NameValueAttribute)fieldInfo.GetCustomAttributes(false).FirstOrDefault());
                nameValues.Add(new NameValueInfo
                {
                    Name = nameValue.Name,
                    Value = nameValue.Value ?? item
                });
            }
            return nameValues;
        }
    }
}
