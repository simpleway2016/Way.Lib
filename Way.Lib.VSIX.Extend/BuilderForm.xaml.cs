using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Way.Lib.VSIX.Extend
{
    /// <summary>
    /// BuilderForm.xaml 的交互逻辑
    /// </summary>
    public partial class BuilderForm : Window
    {
        System.Windows.Forms.PropertyGrid _propertyGrid;
        Way.Lib.VSIX.Extend.AppCodeBuilder.IAppCodeBuilder _currentBuilder;

        Services.ITypeDiscoverer _TypeDiscoverer;
        internal Services.ITypeDiscoverer TypeDiscoverer
        {
            get
            {
                if (_TypeDiscoverer == null)
                    _TypeDiscoverer = GetService<Services.ITypeDiscoverer>();
                return _TypeDiscoverer;
            }
        }
    
        public BuilderForm()
        {
            InitializeComponent();
            bindBuilders();

            _propertyGrid = new System.Windows.Forms.PropertyGrid();
            hostBuilderPG.Child = _propertyGrid;
        }

        void bindBuilders()
        {
            List<object> objs = new List<object>();
            objs.Add(new { Type = typeof(string), Name = "请选择控件类型" });
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(m=>m.GetInterface("Way.Lib.VSIX.Extend.AppCodeBuilder.IAppCodeBuilder") != null).OrderBy(m=>m.Name)
                .Select(m=> new { Type = m, Name = m.Name.Replace("Builder" , "") }).ToArray() ;

            objs.AddRange(types);
            lstCodeBuilders.ItemsSource = objs;
            lstCodeBuilders.SelectedIndex = 0;
            lstCodeBuilders.SelectionChanged += LstCodeBuilders_SelectionChanged;
        }

        private void LstCodeBuilders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCodeBuilders.SelectedIndex > 0)
            {
                Type type = (Type)lstCodeBuilders.SelectedValue;
                _currentBuilder = (Way.Lib.VSIX.Extend.AppCodeBuilder.IAppCodeBuilder)Activator.CreateInstance(type);
                viewContainer.Children.Clear();
                if (_currentBuilder.ViewControl != null)
                {
                    _currentBuilder.ViewControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    viewContainer.Children.Add(_currentBuilder.ViewControl);
                }
                _propertyGrid.SelectedObject = _currentBuilder;
            }
        }

        #region services
        static List<object> Services = new List<object>();
        public static void AddService(object obj)
        {
            Services.Add(obj);
        }
        public static T GetService<T>()
        {
            Type t = typeof(T);
            return (T)Services.Where(m => m.GetType().GetInterface(t.FullName) != null).FirstOrDefault();
        }
        #endregion

      
    }
}
