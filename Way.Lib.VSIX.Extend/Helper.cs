﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Way.Lib.VSIX.Extend
{
    class Helper
    {

        public static void ShowMessage( Window win , string msg)
        {
            MessageBox.Show(win, msg);
        }

        public static void ShowError( Exception err)
        {
            MessageBox.Show( err.Message);
        }

        public static void ShowError( Window win , Exception err)
        {
            MessageBox.Show(win, err.Message);
        }
      
        public static object Clone(object src)
        {
            Type type = src.GetType();
            object clone = Activator.CreateInstance(type);
            System.Reflection.PropertyInfo[] pinfos = type.GetProperties();
            for (int i = 0; i < pinfos.Length; i++)
            {
                try
                {
                    object pvalue = pinfos[i].GetValue(src);
                    pinfos[i].SetValue( clone , pvalue );
                }
                catch
                {
                }
            }
            return clone;
        }
    }


    public static class MyExtensions
    {
        

        /// <summary>
        /// 功能和ToString一样，null会返回""
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSafeString(this object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString();
        }
        /// <summary>
        /// 功能和ToString一样，null或者空字符，会返回noneString
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="noneString"></param>
        /// <returns></returns>
        public static string ToSafeString(this object obj, string noneString)
        {
            if (obj == null)
                return noneString;
            string result = obj.ToString();
            if (result.Length == 0)
                return noneString;
            return result;
        }
        /// <summary>
        /// 转换为时间格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(str);
        }
        /// <summary>
        /// 每个对象输出一行字符
        /// </summary>
        /// <param name="arrs"></param>
        /// <returns></returns>
        public static string ToEachString(this Array arrs)
        {
            StringBuilder result = new StringBuilder();
            foreach (object str in arrs)
            {
                result.AppendLine(str.ToString());
            }
            return result.ToString();
        }
        /// <summary>
        /// 字符串转整型，空字符串返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return Convert.ToInt32(str);
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static double ToDouble(this object str)
        {
            if (str == null)
                return 0;
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(str + " ToDouble失败"));
            }
        }
        public static double ToDouble(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(str + " ToDouble失败"));
            }
        }

        /// <summary>
        /// 取小数点后四位，不四舍五入，后面的舍去
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToRound4Decimal(this decimal number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(4, arr[1].Length)));
            }
            else
            {
                return number;
            }
        }
        /// <summary>
        /// 取小数点后四位，不四舍五入，后面的舍去
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToRound4Decimal(this double number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(4, arr[1].Length)));
            }
            else
            {
                return Convert.ToDecimal(number);
            }
        }
        /// <summary>
        /// 去小数点后两位，有三位小数一律进1
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal To2DecimalBigger(this decimal number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                var result = Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
                if (result < number)
                    result += 0.01m;
                return result;
            }
            else
            {
                return number;
            }
        }

        /// <summary>
        /// 去小数点后两位，有三位小数一律进1
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal To2DecimalBigger(this decimal? number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                var result = Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
                if (result < number)
                    result += 0.01m;
                return result;
            }
            else
            {
                return number.GetValueOrDefault();
            }
        }

        /// <summary>
        /// 取小数点后2位，不四舍五入，后面的舍去
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToRound2Decimal(this decimal number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
            }
            else
            {
                return number;
            }
        }
        /// <summary>
        /// 取小数点后2位，不四舍五入，后面的舍去
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToRound2Decimal(this decimal? number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
            }
            else
            {
                return number.GetValueOrDefault();
            }
        }
        /// <summary>
        /// 取小数点后2位，不四舍五入，后面的舍去
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToRound2Decimal(this double number)
        {
            string[] arr = number.ToString().Split('.');
            if (arr.Length > 1)
            {
                return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
            }
            else
            {
                return Convert.ToDecimal(number);
            }
        }
        public static decimal ToDecimal(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return Convert.ToDecimal(str);
        }

        /// <summary>
        /// object输出money字符串，0输出空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NoZeroToMoney(this object value)
        {
            try
            {
                double d = Convert.ToDouble(value);
                if (d == 0)
                    return "";
                string v = Math.Round(d, 2).ToString("n");
                if (v.Length == 0)
                    return "";
                return v;
            }
            catch
            {
                return "";
            }
        }

        public static string ToMoney(this double value)
        {
            string v = Math.Round(Convert.ToDouble(value), 2).ToString("n");
            if (v.Length == 0)
                return "0";

            return v;
        }
        public static string ToMoney(this decimal value)
        {
            string v = Math.Round(Convert.ToDouble(value), 2).ToString("n");
            if (v.Length == 0)
                return "0";
            return v;
        }
        public static string ToMoney(this decimal? value)
        {
            string v = Math.Round(Convert.ToDouble(value.GetValueOrDefault()), 2).ToString("n");
            if (v.Length == 0)
                return "0";
            return v;
        }



        /// <summary>
        /// 每个对象逗号隔开
        /// </summary>
        /// <param name="arrs"></param>
        /// <returns></returns>
        public static string ToSplitString(this Array arrs)
        {
            return arrs.ToSplitString(",");
        }
        /// <summary>
        /// 用制定字符串联数组
        /// </summary>
        /// <param name="arrs"></param>
        /// <param name="splitchar">间隔字符</param>
        /// <returns></returns>
        public static string ToSplitString(this Array arrs, string splitchar)
        {
            StringBuilder result = new StringBuilder();
            foreach (object str in arrs)
            {
                if (result.Length > 0)
                    result.Append(splitchar);
                result.Append(str.ToString().Trim());
            }
            return result.ToString();
        }
        public static string ToSplitString(this Array arrs, string splitchar, string itemFormat)
        {
            StringBuilder result = new StringBuilder();
            foreach (object str in arrs)
            {
                if (result.Length > 0)
                    result.Append(splitchar);
                result.Append(string.Format(itemFormat, str));
            }
            return result.ToString();
        }
        //public static string ToString(this int? value)
        //{
        //    if (value == null)
        //        return "";
        //    else
        //        return value.GetValueOrDefault().ToString();
        //}
        //public static string ToString(this decimal? value)
        //{
        //    if (value == null)
        //        return "";
        //    else
        //        return value.GetValueOrDefault().ToString();
        //}
        //public static string ToString(this bool? value)
        //{
        //    if (value == null)
        //        return "";
        //    else
        //        return value.GetValueOrDefault().ToString();
        //}
        //public static string ToString(this double? value)
        //{
        //    if (value == null)
        //        return "";
        //    else
        //        return value.GetValueOrDefault().ToString();
        //}
    }

}
