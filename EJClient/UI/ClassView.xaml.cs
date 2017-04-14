using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EJClient.UI
{
    /// <summary>
    /// ClassView.xaml 的交互逻辑
    /// </summary>
    public partial class ClassView : UserControl
    {
        class JsonObject
        {
            public string FilePath;
            public string FullName;
        }
        class MyTextBox : TextBox
        {
            public MyTextBox()
            {
                this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                this.SelectAll();
                base.OnMouseUp(e);
            }
        }

        public event EventHandler MoveCompleted;
        Forms.InterfaceCenter.Nodes.ClassNode m_DataSource;
        JsonObject m_jsonObj = new JsonObject();
        internal EJ.InterfaceInModule InterfaceInModule;
        internal ClassView(Forms.InterfaceCenter.Nodes.ClassNode source, string filepath, EJ.InterfaceInModule interfaceInModule)
        {
            this.InterfaceInModule = interfaceInModule;
            m_DataSource = source;
            m_jsonObj.FilePath = filepath;
            m_jsonObj.FullName = source.FullName;
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            this.DataBind();
        }
        internal ClassView( EJ.InterfaceInModule interfaceInModule)
        {
            this.InterfaceInModule = interfaceInModule;
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            m_jsonObj = interfaceInModule.JsonData.ToJsonObject<JsonObject>();
            m_DataSource = Forms.InterfaceCenter.DllClassLoader.GetClass(m_jsonObj.FilePath, m_jsonObj.FullName);
            this.DataBind();

        }
        internal ClassView(string fullname, string filepath, EJ.InterfaceInModule interfaceInModule)
        {
            this.InterfaceInModule = interfaceInModule;
            m_jsonObj.FilePath = filepath;
            m_jsonObj.FullName = fullname;
            m_DataSource = Forms.InterfaceCenter.DllClassLoader.GetClass(filepath , fullname);
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            this.DataBind();
        }

        public string GetJsonData()
        {
            return m_jsonObj.ToJsonString();
        }

        void setTextBoxStyle(TextBox t, int column)
        {
            t.IsReadOnly = true;
            t.BorderThickness = new Thickness(0);
            Grid.SetColumn(t, column);
            t.Background = Brushes.Transparent;
            t.Height = 21;
            if (column == 1)
            {
                t.Margin = new Thickness(5, 0, 5, 0);
            }
            else
            {
                t.Margin = new Thickness(5, 0, 0, 0);
            }
            t.Padding = new Thickness(0, 1, 0, 0);
            //IsReadOnly="True" BorderThickness="0" Grid.Column="0" Background="Transparent" Height="21" Margin="5,0,0,0" Padding="0,1,0,0" Text="Test caption"></TextBox>
        }

        /// <summary>
        /// 绑定显示数据
        /// </summary>
        public void DataBind()
        {
            if (this.m_DataSource != null)
            {
                txtComment.Text = m_DataSource.Comment;
                txtFullname.Text = m_DataSource.FullName;

                if (string.IsNullOrEmpty(m_DataSource.Comment))
                {
                    txtComment.Visibility = System.Windows.Visibility.Collapsed;
                    titleArea.Height = 22;
                    titleArea.RowDefinitions.Clear();
                }

                gridColumns.RowDefinitions.Clear();
                gridColumns.Children.Clear();
                for (int i = 0; i < m_DataSource.Members.Count; i++)
                {
                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = GridLength.Auto;
                    gridColumns.RowDefinitions.Add(rowdef);

                    var member = m_DataSource.Members[i];

                    TextBox t1 = new MyTextBox();
                    t1.Text = member.Comment;
                    setTextBoxStyle(t1, 0);
                    Grid.SetRow(t1, i);
                    gridColumns.Children.Add(t1);

                    TextBox t2 = new MyTextBox();
                    t2.Text = m_DataSource.Name + "." + member.Name;
                    setTextBoxStyle(t2, 1);
                    Grid.SetRow(t2, i);
                    gridColumns.Children.Add(t2);

                                  }
            }
        }

        #region 移动
        Point m_oldMousePoint;
        bool m_titleMoving = false;
        Thickness m_oldLocation;
        private void titleArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(this.titleArea);
            m_oldMousePoint = e.GetPosition(this.Parent as IInputElement);
            m_titleMoving = true;
            m_oldLocation = this.Margin;
            e.Handled = true;
        }

        private void titleArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_titleMoving)
            {
                e.Handled = true;
                Point p = e.GetPosition(this.Parent as IInputElement);
                this.Margin = new Thickness(m_oldLocation.Left + p.X - m_oldMousePoint.X, m_oldLocation.Top + p.Y - m_oldMousePoint.Y, 0, 0);
            }
        }

        private void titleArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (m_titleMoving)
            {
                e.Handled = true;
                e.MouseDevice.Capture(null);
                m_titleMoving = false;

                InterfaceInModule.x = (int)this.Margin.Left;
                InterfaceInModule.y = (int)this.Margin.Top;
                Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);

                if (MoveCompleted != null)
                {
                    MoveCompleted(this, null);
                }
            }
        }
        #endregion

        private void MenuItem_删除_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Helper.Client.InvokeSync<string>("DeleteInterfaceInModule", this.InterfaceInModule);
                ((Panel)this.Parent).Children.Remove(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
