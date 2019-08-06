using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{
    public class ReflectHelper
    {
        #region 对私有字段和属性值的设置及获取、私有方法的执行
        //1、得到私有字段的值
        public static T GetPrivateField<T>(object instance, string fieldName)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldName, flag);
            return (T)field.GetValue(instance);
        }

        //2、得到私有属性的值：
        public static T GetPrivateProperty<T>(object instance, string propertyName)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyName, flag);
            return (T)field.GetValue(instance, null);
        }

        //3、设置私有字段的值：
        public static void SetPrivateField(object instance, string fieldName, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldName, flag);
            field.SetValue(instance, value);
        }

        //4、设置私有属性的值： 
        public static void SetPrivateProperty(object instance, string propertyName, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyName, flag);
            field.SetValue(instance, value, null);
        }

        //5、调用私有方法：
        public static T InvokePrivateMethod<T>(object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            return (T)method.Invoke(instance, param);
        }
        #endregion

    }
}
