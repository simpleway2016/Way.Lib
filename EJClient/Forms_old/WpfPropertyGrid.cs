using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Reflection;
using System.Windows.Controls;

namespace EJClient.Forms
{
    /// <summary>
    /// 不支持editor
    /// </summary>
    public class WpfPropertyGrid : Grid
    {
        #region Private fields
        private WorkflowDesigner Designer;
        private MethodInfo RefreshMethod;
        private MethodInfo OnSelectionChangedMethod;
        private TextBlock SelectionTypeLabel;
        private object TheSelectedObject = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Get or sets the selected object. Can be null.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return this.TheSelectedObject;
            }
            set
            {
                this.TheSelectedObject = value;

                if (value != null)
                {
                    var context = new EditingContext();
                    var mtm = new ModelTreeManager(context);
                    mtm.Load(value);
                    var selection = Selection.Select(context, mtm.Root);

                    OnSelectionChangedMethod.Invoke(Designer.PropertyInspectorView, new object[] { selection });
                    this.SelectionTypeLabel.Text = value.GetType().Name;
                }
                else
                {
                    OnSelectionChangedMethod.Invoke(Designer.PropertyInspectorView, new object[] { null });
                    this.SelectionTypeLabel.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// XAML information with PropertyGrid's font and color information
        /// </summary>
        /// <seealso>Documentation for WorkflowDesigner.PropertyInspectorFontAndColorData</seealso>
        public string FontAndColorData
        {
            set 
            { 
                Designer.PropertyInspectorFontAndColorData = value; 
            }
        }
        #endregion

        /// <summary>
        /// Default constructor, creates a hidden designer view and a property inspector
        /// </summary>
        public WpfPropertyGrid()
        {
            this.Designer = new WorkflowDesigner();
            var inspector = Designer.PropertyInspectorView;
            System.Type inspectorType = inspector.GetType();
            
            inspector.Visibility = System.Windows.Visibility.Visible;
            this.Children.Add(inspector);

            var methods = inspectorType.GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.DeclaredOnly);

            //MethodInfo[] ms = inspectorType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

            this.RefreshMethod = inspectorType.GetMethod("RefreshPropertyList",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            this.OnSelectionChangedMethod = inspectorType.GetMethod("OnSelectionChanged", 
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            this.SelectionTypeLabel = inspectorType.GetMethod("get_SelectionTypeLabel",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.DeclaredOnly).Invoke(inspector, new object[0]) as TextBlock;

            this.SelectionTypeLabel.Text = string.Empty;
        }

        /// <summary>
        /// Updates the PropertyGrid's properties
        /// </summary>
        public void RefreshPropertyList()
        {

            var context = new EditingContext();
            var mtm = new ModelTreeManager(context);
            mtm.Load(this.SelectedObject);
            var selection = Selection.Select(context, mtm.Root);

            OnSelectionChangedMethod.Invoke(Designer.PropertyInspectorView, new object[] { selection });

           // var olditem = this.SelectedObject;
           // this.SelectedObject = null;
           // this.SelectedObject = olditem;
            //RefreshMethod.Invoke(Designer.PropertyInspectorView, new object[] { false });
        }
    }
}