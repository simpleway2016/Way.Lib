using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WayControls.Windows.CommBase;

namespace WayControls.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public class Comm : CommBase.CommBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static CommBaseTermSettings settings;
        /// <summary>
        /// 
        /// </summary>
        public static string settingsFileName = "";

        private int lineCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public class CommBaseTermSettings : CommBaseSettings
        {
            /// <summary>
            /// 
            /// </summary>
            public bool showAsHex = false;
            /// <summary>
            /// 
            /// </summary>
            public bool breakLineOnChar = false;
            /// <summary>
            /// 
            /// </summary>
            public ASCII lineBreakChar = 0;
            /// <summary>
            /// 
            /// </summary>
            public int charsInLine = 0;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static new CommBaseTermSettings LoadFromXML(Stream s)
            {
                return (CommBaseTermSettings)LoadFromXML(s, typeof(CommBaseTermSettings));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Immediate = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void SendChar(byte c)
        {
            if (Immediate)
                SendImmediate(c);
            else
                Send(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        public void SendFile(FileStream fs)
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            Send(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool SendCtrl(string s)
        {
            ASCII a = 0;
            try
            {
                a = (ASCII)ASCII.Parse(a.GetType(), s, true);
            }
            catch
            {
                return false;
            }
            SendChar((byte)a);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override CommBaseSettings CommSettings()
        {
            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        protected override void OnRxChar(byte c)
        {
            string s; bool nl = false;
            ASCII v = (ASCII)c;
            if (settings.charsInLine > 0)
            {
                nl = (++lineCount >= settings.charsInLine);
            }
            if (settings.breakLineOnChar) if (v == settings.lineBreakChar) nl = true;
            if (nl) lineCount = 0;
            if (settings.showAsHex)
            {
                s = c.ToString("X2");
                if (!nl) s += " ";
            }
            else
            {
                if ((c < 0x20) || (c > 0x7E))
                {
                    s = "<" + v.ToString() + ">";
                }
                else
                {
                    s = new string((char)c, 1);
                }
            }
            //frm.ShowChar(s, nl);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnBreak()
        {
            //frm.ShowMsg(">>>> BREAK");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool AfterOpen()
        {
            //frm.OnOpen();
            ModemStatus m = GetModemStatus();
            //frm.SetIndics(m.cts, m.dsr, m.rlsd, m.ring);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void BeforeClose(bool e)
        {
            if ((settings.autoReopen) && (e))
            {
                //frm.OnOpen();
            }
            else
            {
                //frm.OnClose();
                //frm.ShowMsg(">>>> OFFLINE");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="v"></param>
        protected override void OnStatusChange(ModemStatus c, ModemStatus v)
        {
            //frm.SetIndics(v.cts, v.dsr, v.rlsd, v.ring);
        }
    }
}
