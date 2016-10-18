using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EJClient
{
    class Helper
    {
        public static EJ.User_RoleEnum CurrentUserRole;
        public static int CurrentUserID;
        public static string WebSite = "http://localhost:888";
        internal static System.Net.CookieContainer CookieContainer = null;
        public static Web.DatabaseService CreateWebService()
        {
            if (CookieContainer == null)
                CookieContainer = new System.Net.CookieContainer();

            Web.DatabaseService web = new Web.DatabaseService();
            web.Url = WebSite + "/DatabaseService.asmx";
            web.CookieContainer = CookieContainer;
            return web;
        }
        public static void ShowMessage( Window win , string msg)
        {
            MessageBox.Show(win, msg);
        }
        public static void ShowMessage(string msg)
        {
            if(MainWindow.instance != null)
            MessageBox.Show(MainWindow.instance, msg);
            else
                MessageBox.Show( msg);
        }
        public static void ShowError( Window win , Exception err)
        {
            MessageBox.Show(win, err.Message);
        }
        public static void ShowError(Exception err)
        {
            if (MainWindow.instance != null)
            MessageBox.Show(MainWindow.instance, err.Message);
            else
                MessageBox.Show(err.Message);
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

    static class MyExtensions
    {
        static System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return json.Serialize(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            return json.Deserialize<T>(str);
        }
    }
}
