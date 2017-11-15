using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SunRizStudio
{
    static class ExtendMethods
    {
        /// <summary>
        /// 安全的ToString方法，对象如果是null，返回""
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
        /// 字符串是否是空字符串,空格也认为是空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBlank(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            if (str.Trim().Length == 0)
                return true;
            return false;
        }

        internal static T JsonToObject<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        internal static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }

    /// <summary>
    /// WPF/Silverlight 查找控件扩展方法
    /// </summary>
    internal static class VisualHelperTreeExtension
    {

        /// <summary>
        /// 根据控件名称，查找父控件
        /// elementName为空时，查找指定类型的父控件
        /// </summary>
        internal static T GetParentByName<T>(this DependencyObject obj, string elementName)
        where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if ((parent is T) && (((T)parent).Name == elementName || string.IsNullOrEmpty(elementName)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// 根据控件名称，查找子控件
        /// elementName为空时，查找指定类型的子控件
        /// </summary>
        internal static T GetChildByName<T>(this DependencyObject obj, string elementName)
        where T : FrameworkElement
        {
            if (obj == null)
                return null;
            DependencyObject child = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == elementName || (string.IsNullOrEmpty(elementName))))
                {
                    return (T)child;
                }
                else
                {
                    T grandChild = GetChildByName<T>(child, elementName);
                    if (grandChild != null)
                    {
                        return grandChild;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据控件名称，查找子控件集合
        /// elementName为空时，查找指定类型的所有子控件
        /// </summary>
        internal static List<T> GetChildsByName<T>(this DependencyObject obj, string elementName)
        where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == elementName || (string.IsNullOrEmpty(elementName))))
                {
                    childList.Add((T)child);
                }
                else
                {
                    List<T> grandChildList = GetChildsByName<T>(child, elementName);
                    if (grandChildList != null)
                    {
                        childList.AddRange(grandChildList);
                    }
                }
            }
            return childList;
        }
    }
}
