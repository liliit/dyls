using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Common.Utils
{
    public class EnumHelper<T>
    {
        public static T GetEnumByInt(object num)
        {
            return (T)Enum.ToObject(typeof(T), Convert.ToInt32(num));
        }

        public static string GetString(T menuType)
        {
            return Enum.GetName(menuType.GetType(), menuType);
        }

        public static T GetEnumByString(string str)
        {
           return (T)Enum.Parse(typeof(T), str);
        }
    }
}
