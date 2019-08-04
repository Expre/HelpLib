using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HelpLib
{
    public static partial class Extentions
    {
        public static string ToName(this Enum value)
        {
            Type enumType = value.GetType();
            if (!Enum.IsDefined(enumType, value))
                return string.Empty;
            NameValueAttribute nameValueAttribute = enumType.GetField(value.ToString()).GetCustomAttribute<NameValueAttribute>();
            return nameValueAttribute?.Name;
        }
        public static object ToValue(this Enum value)
        {
            Type enumType = value.GetType();
            if (!Enum.IsDefined(enumType, value))
                return default;
            NameValueAttribute nameValueAttribute = enumType.GetField(value.ToString()).GetCustomAttribute<NameValueAttribute>();
            object tempValue = nameValueAttribute.Value;
            if (tempValue != null)
                return tempValue;
            Enum.TryParse(enumType, value.ToString(), out tempValue);
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
