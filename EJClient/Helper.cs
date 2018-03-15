using EJClient.Net;
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

        public static string WebSite;
        public static RemotingClient Client;
        public static EJ.User_RoleEnum CurrentUserRole;
        public static int CurrentUserID;

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
        public static void ShowError(Window win, string err)
        {
            MessageBox.Show(win, err);
        }
        public static void ShowError(Exception err)
        {
            if (MainWindow.instance != null)
            MessageBox.Show(MainWindow.instance, err.Message);
            else
                MessageBox.Show(err.Message);
        }
        public static void ShowError(string err)
        {
            if (MainWindow.instance != null)
                MessageBox.Show(MainWindow.instance, err);
            else
                MessageBox.Show(err);
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
         
        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
