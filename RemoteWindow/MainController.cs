using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib.ScriptRemoting;
using static RemoteWindow.Helper;

namespace RemoteWindow
{
    public class MainController : RemotingController
    {
        internal static List<BitmapMarker> BitmapQueue = new List<BitmapMarker>();
        static int BitmapQueueIndex = 0;
        static WindowBitmap Checker;


        internal static bool RemotingController_OnMessageReceiverConnect(SessionState session, string groupName)
        {

            return true;
        }

        internal static Bitmap GetBitmapById(int id)
        {
            BitmapMarker item = null;
            lock (BitmapQueue)
            {
                for (int i = 0; i < BitmapQueue.Count; i++)
                {
                    if (BitmapQueue[i].Id == id)
                    {
                        item = BitmapQueue[i];
                        BitmapQueue.RemoveAt(i);
                        break;
                    }
                }
            }
            if (item == null)
                return null;
            return item.Bitmap;
        }

        private static void Checker_OnDiffentMaked(object sender, BitmapMarker marker)
        {
            DateTime enterTime = DateTime.Now;
            while (BitmapQueue.Count > 0)
            {
                if ((DateTime.Now - enterTime).TotalSeconds > 1)
                {
                    //客户端下载图片超时，输出整张图片
                    marker.Id = BitmapQueueIndex++;
                    marker.Bitmap.Dispose();
                    marker.Rect = new RECT(new System.Drawing.Rectangle(Point.Empty, ((WindowBitmap)sender).CurrentBitmap.Size));
                    marker.Bitmap = (Bitmap)((WindowBitmap)sender).CurrentBitmap.Clone();

                    lock (BitmapQueue)
                    {
                        BitmapQueue.Clear();                      
                        BitmapQueue.Add(marker);
                        
                    }
                    SendGroupMessage("group1", Newtonsoft.Json.JsonConvert.SerializeObject(marker));
                    return;
                }
                System.Threading.Thread.Sleep(0);
            }
            lock (BitmapQueue)
            {
                marker.Id = BitmapQueueIndex++;
                BitmapQueue.Add(marker);
            }
            SendGroupMessage("group1", Newtonsoft.Json.JsonConvert.SerializeObject(marker));
        }

        [RemotingMethod]
        public string Start(int width, int height)
        {
            if (Checker == null)
            {
                Checker = new WindowBitmap(Form1.HWND, width, height);
                Checker.OnDiffentMaked += Checker_OnDiffentMaked;
                Checker.Start();
            }
            return "Hello World!";
        }
    }

    class BitmapMarker
    {
        public int Id;
        public RECT Rect;
        [Newtonsoft.Json.JsonIgnore]
        public Bitmap Bitmap;
    }
    class WindowBitmap
    {
        public Bitmap CurrentBitmap;
        IntPtr _hwnd;
        public delegate void OnDiffentMakedHandler(object sender, BitmapMarker bitmap);
        public event OnDiffentMakedHandler OnDiffentMaked;

        bool _started = false;
        int _clientWidth, _clientHeight;
        System.Drawing.Rectangle _targetRect = System.Drawing.Rectangle.Empty;
        public WindowBitmap(IntPtr hwnd, int clientWidth, int clientHeight)
        {
            _clientWidth = clientWidth;
            _clientHeight = clientHeight;
            _hwnd = hwnd;
        }

        public void Start()
        {
            _started = true;
            Task.Run(() => check());
        }

        public void Stop()
        {
            _started = false;
        }

        void check()
        {
            while (_started)
            {
                System.Threading.Thread.Sleep(50);
                RECT rect;
                if(_targetRect == System.Drawing.Rectangle.Empty)
                {
                    Helper.GetClientRect(_hwnd, out rect);
                    double scalex = _clientWidth / (double)rect.Width;
                    double scaley = _clientHeight / (double)rect.Height;
                    scalex = Math.Min(scalex, scaley);
                    var targetWidth =(int)( rect.Width * scalex);
                    var targetHeight = (int)(rect.Height * scalex);
                    _targetRect = new Rectangle( (_clientWidth - targetWidth) / 2  , (_clientHeight - targetHeight) / 2 , targetWidth, targetHeight);
                }
                var bitmap = Helper.GetWindowBitmap(_hwnd, out rect,_clientWidth,_clientHeight, _targetRect);
                var diffent = Helper.CompareBitmap(CurrentBitmap, bitmap, out rect);
                if (CurrentBitmap != null)
                    CurrentBitmap.Dispose();
                CurrentBitmap = bitmap;

                if (OnDiffentMaked != null && diffent != null)
                {
                    OnDiffentMaked(this, new BitmapMarker()
                    {
                        Bitmap = diffent,
                        Rect = rect
                    });
                }
            }
        }
    }
}
