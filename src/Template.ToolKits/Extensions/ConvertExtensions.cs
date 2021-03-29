/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ServiceResult.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Template.Base;

namespace Template.Extensions
{
    public static class ConvertExtensions
    {
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultNum"></param>
        /// <returns></returns>
        public static int TryToInt(this object input, int defaultNum = 0)
        {
            if (input == null)
            {
                return defaultNum;
            }

            return int.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }
        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultNum"></param>
        /// <returns></returns>
        public static long TryToLong(this object input, long defaultNum = 0)
        {
            if (input == null)
            {
                return defaultNum;
            }

            return long.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }
        /// <summary>
        ///  string转double
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultNum"></param>
        /// <returns></returns>
        public static double TryToDouble(this object input, double defaultNum = 0)
        {
            if (input == null)
                return defaultNum;
            return double.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }
        /// <summary>
        /// string转decimal
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultNum"></param>
        /// <returns></returns>
        public static decimal TryToDecimal(this object input, decimal defaultNum = 0)
        {
            if (input == null)
                return defaultNum;
            return decimal.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }
        /// <summary>
        /// string转float
        /// </summary> 
        /// <param name="input"></param>
        /// <param name="defaultNum"></param>
        /// <returns></returns>
        public static float TryToFloat(this object input, float defaultNum)
        {
            if (input == null)
                return defaultNum;
            return float.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }
        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultBool"></param>
        /// <param name="trueVal"></param>
        /// <param name="falseVal"></param>
        /// <returns></returns>
        public static bool TryToBool(this object input, bool defaultBool = false, string trueVal = "1", string falseVal = "0")
        {
            if (input == null)
                return defaultBool;
            var str = input.ToString();
            if (bool.TryParse(str, out var outBool))
                return outBool;
            outBool = defaultBool;
            if (trueVal == str)
            {
                return true;
            }

            if (falseVal == str)
            {
                return false;
            }

            return outBool;
        }
        /// <summary>
        /// 值类型转string
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string TryToString(this ValueType inputObj, string defaultStr = "")
        {
            var output = inputObj.IsNull() ? defaultStr : inputObj.ToString();
            return output;
        }
        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime TryToDateTime(this string inputStr, DateTime defaultValue = default)
        {
            if (inputStr.IsNullOrEmpty())
            {
                return defaultValue;
            }

            return DateTime.TryParse(inputStr, out var outPutDateTime) ? outPutDateTime : defaultValue;
        }
        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="formater"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime TryToDateTime(this string inputStr, string formater, DateTime defaultValue = default)
        {
            if (inputStr.IsNullOrEmpty())
                return defaultValue;
            return DateTime.TryParseExact(inputStr, formater, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outPutDateTime) ? outPutDateTime : defaultValue;
        }
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TryToDateTime(this string timestamp)
        {
            var ticks = 621355968000000000 + long.Parse(timestamp) * 10000;
            return new DateTime(ticks);
        }
        /// <summary>
        /// 时间格式转换为字符串
        /// </summary>
        /// <param name="date"></param>
        /// <param name="formater"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string TryToDateTime(this DateTime date, string formater = "MMMM dd,yyyy HH:mm:ss", string cultureInfo = "en-us")
        {
            return date.ToString(formater, new CultureInfo(cultureInfo));
        }
        /// <summary>
        /// 字符串去空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string TryToTrim(this string inputStr)
        {
            var output = inputStr.IsNullOrEmpty() ? inputStr : inputStr.Trim();
            return output;
        }
        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T TryToEnum<T>(this string str, T t = default) where T : struct
        {
            return System.Enum.TryParse<T>(str, out var result) ? result : t;
        }

        public static IEnumerable<EnumResponse> TryToList(this Type type)
        {
            var result=new List<EnumResponse>();
            foreach (var item in System.Enum.GetValues(type))
            {
                var response = new EnumResponse
                {
                    Key = item.ToString(),
                    Value = Convert.ToInt32(item)
                };
                var objArray = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArray.Any())
                    response.Description = (objArray.First() as DescriptionAttribute).Description;

                result.Add(response);

            }

            return result;
        }




    }
}
