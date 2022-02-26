﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace King.Helper
{
    public static class EnumHelper
    {
        /// <summary> 
        /// 获取枚举值的描述文本 
        /// </summary> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 获取枚举值的描述文本 
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<T>(string value)
        {
            FieldInfo fi = typeof(T).GetTypeInfo().GetField(value);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
