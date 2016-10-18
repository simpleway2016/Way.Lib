using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EJClient.Forms.BugCenter
{
    /// <summary>
    /// BugEditor.xaml 的交互逻辑
    /// </summary>
    public partial class BugEditor : UserControl
    {
        IToolBarItem[] m_ToolItems;
        public static BugEditor instance;
        string[] fileNames;
        BitmapImage[] bitmaps;
        internal EditingCache[] EditingCaches;
        int fileIndex;

        bool[] IsSelectCurrentPics;

        public bool IsSelectCurrentPic
        {
            get
            {
                if (fileNames == null)
                    return false;
                return IsSelectCurrentPics[fileIndex];
            }
            set
            {
                if (fileNames == null)
                    return;

                IsSelectCurrentPics[fileIndex] = value;
                txtWhich.Text = (fileIndex + 1) + "/" + fileNames.Length;
                if (!value)
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Forms/BugCenter/imgs/checked.png"));

                    FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                    newFormatedBitmapSource.BeginInit();
                    newFormatedBitmapSource.Source = bitmapImage;
                    newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
                    newFormatedBitmapSource.EndInit();

                    chkSelected.Source = newFormatedBitmapSource;
                }
                else
                {
                    chkSelected.Source = new BitmapImage(new Uri("pack://application:,,,/Forms/BugCenter/imgs/checked.png"));
                }
            }
        }

        public BugEditor()
        {
            instance = this;
            InitializeComponent();
            m_ToolItems = new IToolBarItem[] { btnText,btnDraw };

            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = 3;
            inkDA.Height = 3;
            inkDA.Color = Colors.Red;
            this.canvas.DefaultDrawingAttributes = inkDA;

            this.canvas.IsHitTestVisible = false;
            this.canvas.StrokeCollected += canvas_StrokeCollected;

            foreach (FrameworkElement ctrl in m_ToolItems)
            {
                ctrl.MouseDown += ctrl_MouseDown;
            }
        }

        void canvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            IsSelectCurrentPic = true;
        }
        public void LoadScreenPictures()
        {
            string[] picFiles = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "BugPics", "*.png");
            if (picFiles.Length == 0)
                throw new Exception("没有发现任何截图");
            System.IO.FileInfo[] fileinfos = new System.IO.FileInfo[picFiles.Length];
            EditingCaches = new EditingCache[picFiles.Length];
            IsSelectCurrentPics = new bool[picFiles.Length];
            for (int i = 0; i < picFiles.Length; i++)
            {
                fileinfos[i] = new System.IO.FileInfo(picFiles[i]);
            }
            fileNames = fileinfos.OrderBy(m => m.LastWriteTime).Select(m => m.FullName).ToArray();
            fileIndex = fileNames.Length - 1;
            if (fileIndex >= 0)
            {
                var bit = new BitmapImage(new Uri(fileNames[fileIndex], UriKind.Absolute));
                canvas.Background = new ImageBrush()
                {
                    Stretch = System.Windows.Media.Stretch.None,
                    ImageSource = bit,
                };
                mainGrid.Width = bit.Width;
                mainGrid.Height = bit.Height;
            }
            IsSelectCurrentPic = false;
        }

        public void Load(byte[] content)
        {
            toolControl.Visibility = System.Windows.Visibility.Collapsed;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(content))
            {
                List<BitmapImage> bitImages = new List<BitmapImage>();
                List<EditingCache> caches = new List<EditingCache>();
                ms.Position = 0;
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
                while (true)
                {
                    int len = br.ReadInt32();
                    if (len == 0)
                        break;
                    byte[] bs = br.ReadBytes(len);
                    System.IO.MemoryStream bitms = new System.IO.MemoryStream(bs);
                    bitms.Position = 0;

                   BitmapImage bit = new BitmapImage();
                    bit.BeginInit();
                    bit.StreamSource = bitms;
                    bit.EndInit();
                    bitImages.Add(bit);

                    len = br.ReadInt32();
                    if (len > 0)
                    {
                        bs = br.ReadBytes(len);
                        string xaml = br.ReadString();
                        caches.Add(new EditingCache()
                            {
                                FileName = null,
                                StrokeStream = new System.IO.MemoryStream(bs),
                                Xaml = xaml,
                            });
                    }
                    else
                    {
                        caches.Add(null);
                    }
                }

                bitmaps = bitImages.ToArray();
                EditingCaches = caches.ToArray();

                fileIndex = 0;
            }
            loadCurrentPic();
        }
        public byte[] GetContent()
        {
            saveCurrent();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                for (int i = 0; i < mainGrid.Children.Count; i++)
                {
                    //清楚空的textbox
                    TextBox t = mainGrid.Children[i] as TextBox;
                    if (t != null && string.IsNullOrEmpty(t.Text.Trim()))
                    {
                        mainGrid.Children.RemoveAt(i);
                        i--;
                    }
                }

                    for (int i = 0; i < IsSelectCurrentPics.Length; i++)
                    {
                        if (IsSelectCurrentPics[i])
                        {
                            byte[] filecontent = System.IO.File.ReadAllBytes(fileNames[i]);
                            bw.Write(filecontent.Length);
                            bw.Write(filecontent);
                            if (EditingCaches[i] != null)
                            {
                                filecontent = new byte[EditingCaches[i].StrokeStream.Length];
                                EditingCaches[i].StrokeStream.Read(filecontent, 0, filecontent.Length);
                                bw.Write(filecontent.Length);
                                bw.Write(filecontent);
                                bw.Write(EditingCaches[i].Xaml);
                            }
                            else
                            {
                                bw.Write(0);
                            }
                        }
                    }
                bw.Write(0);
                byte[] bs = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bs, 0, bs.Length);
                return bs;
            }
        }

    
        void ctrl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var activedItem = m_ToolItems.FirstOrDefault(m=>m.IsActived);
            if (activedItem != null && activedItem == sender)
            {
                activedItem.IsActived = false;
                return;
            }
            if (activedItem != null && activedItem != sender)
            {
                activedItem.IsActived = false;
            }
            activedItem = sender as IToolBarItem;
            activedItem.IsActived = true;
        }


        private void containerGrid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var activedItem = m_ToolItems.FirstOrDefault(m => m.IsActived);
            if (activedItem != null  )
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    Point p = e.MouseDevice.GetPosition(mainGrid);
                    activedItem.Do(p);
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    activedItem.IsActived = false;
                }
            }
        }

      

        private void 清除画笔_Click_1(object sender, RoutedEventArgs e)
        {
            this.canvas.Strokes.Clear();
        }

        private void btnPrePic_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (fileIndex > 0)
            {
                saveCurrent();
                fileIndex--;
                loadCurrentPic();
            }
        }

        private void btnNextPic_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (fileIndex < EditingCaches.Length - 1)
            {
                saveCurrent();
                fileIndex++;
                loadCurrentPic();
            }
        }

     

        void saveCurrent()
        {
            if (fileNames == null)
                return;

            if (this.canvas.Strokes.Count == 0 && mainGrid.Children.Count == 1)
            {
                EditingCaches[fileIndex] = null;
                return;
            }
            var ms = new System.IO.MemoryStream();
            this.canvas.Strokes.Save(ms);
            ms.Position = 0;

            this.canvas.Strokes.Clear();
            canvas.Background = Brushes.White;
            EditingCaches[fileIndex] = new EditingCache()
            {
                StrokeStream = ms,
                FileName = fileNames[fileIndex],
                Xaml = System.Windows.Markup.XamlWriter.Save(mainGrid),
            };
        }

        void loadCurrentPic()
        {
            txtWhich.Text = (fileIndex + 1) + "/" + EditingCaches.Length;
            canvas.Strokes.Clear();
            while (mainGrid.Children.Count > 1)
                mainGrid.Children.RemoveAt(1);

            IsSelectCurrentPic = IsSelectCurrentPic;
            if (EditingCaches[fileIndex] != null)
            {
                EditingCaches[fileIndex].StrokeStream.Position = 0;
                this.canvas.Strokes = new StrokeCollection(EditingCaches[fileIndex].StrokeStream);

                var bit = bitmaps != null ? bitmaps[fileIndex] : new BitmapImage(new Uri(EditingCaches[fileIndex].FileName, UriKind.Absolute));
                canvas.Background = new ImageBrush()
                {
                    Stretch = System.Windows.Media.Stretch.None,
                    ImageSource = bit,
                };

                mainGrid.Width = bit.Width;
                mainGrid.Height = bit.Height;

                Grid grid = (Grid)System.Windows.Markup.XamlReader.Parse(EditingCaches[fileIndex].Xaml);
                while(grid.Children.Count > 1)
                {
                    var child = grid.Children[1];
                    grid.Children.RemoveAt(1);
                    mainGrid.Children.Add(child);
                }
            }
            else
            {
                if (bitmaps != null)
                {
                    var bit = bitmaps[fileIndex];
                    canvas.Background = new ImageBrush()
                    {
                        Stretch = System.Windows.Media.Stretch.None,
                        ImageSource = bit,
                    };
                    mainGrid.Width = bit.Width;
                    mainGrid.Height = bit.Height;
                }
                else
                {
                    var bit = new BitmapImage(new Uri(fileNames[fileIndex], UriKind.Absolute));
                    canvas.Background = new ImageBrush()
                    {
                        Stretch = System.Windows.Media.Stretch.None,
                        ImageSource = bit,
                    };
                    mainGrid.Width = bit.Width;
                    mainGrid.Height = bit.Height;
                }
            }
        }

        private void chkSelected_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            IsSelectCurrentPic = !IsSelectCurrentPic;
        }
    }

    class EditingCache
    {
        public System.IO.Stream StrokeStream;
        public string Xaml;
        public string FileName;
    }

    class BugEditorWindow : Window
    {
        public BugEditor Editor
        {
            get;
            private set;
        }
        public BugEditorWindow()
        {
            Editor = new BugEditor();
            Editor.LoadScreenPictures();
            this.Content = this.Editor;
        }
        protected override void OnClosed(EventArgs e)
        {
            foreach (var obj in this.Editor.EditingCaches)
            {
                if (obj != null)
                {
                    obj.StrokeStream.Dispose();
                    obj.Xaml = null;
                }
            }
            base.OnClosed(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}
