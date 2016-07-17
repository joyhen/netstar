using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace NetStar.Tools
{
    public class EnumUtils
    {
        /// <summary>
        /// 获取枚举类型的 Dictionary(枚举名, DescriptionAttribute值)
        /// </summary>
        public static Dictionary<string, string> GetDescription<T>()
        {
            var enumDic = new Dictionary<string, string>();
            var enumFields = typeof(T).GetFields();

            foreach (var f in enumFields)
            {
                var attr = Attribute.GetCustomAttribute(f, typeof(DescriptionAttribute));
                if (attr == null) continue;

                enumDic.Add(f.Name, ((DescriptionAttribute)attr).Description);
            }

            return enumDic;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        public static T GetEnumBuyStr<T>(string str)
        {
            try
            {
                T enumOne = (T)Enum.Parse(typeof(T), str);
                return enumOne;
            }
            catch (Exception ex)
            {
                LogHelp.Log(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 判断是否定义了FlagsAttribute属性
        /// </summary>
        public static bool HasFlagsAttribute(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(FlagsAttribute), true);
            return attributes != null && attributes.Length > 0;
        }

        //...

    }
}