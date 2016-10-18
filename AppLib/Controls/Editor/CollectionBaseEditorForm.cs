using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
namespace AppLib.Controls.Editor
{
	/// <summary>
	///  的摘要说明。
	/// </summary>
	public class CollectionBaseEditorForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		protected ITypeDescriptorContext Context;
		/// <summary>
		/// 
		/// </summary>
		public object EditValue;

		/// <summary>
		/// 
		/// </summary>
		protected object ParentControl = null;
		/// <summary>
		/// 
		/// </summary>
		protected Button ApplyButton = null;


		private System.ComponentModel.Container components = null;
		/// <summary>
		/// 
		/// </summary>
        public List<object> Items = new List<object>();

		/// <summary>
		/// 
		/// </summary>
		protected Type ItemType;

		/// <summary>
		/// 
		/// </summary>
		protected string TitleField = "";

		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.TreeView tree;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button btnOK;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button btnAddRoot;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button btnDelete;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button downButton;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.PropertyGrid propGrid;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button upButton;
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.Button btnApply;
		/// <summary>
		/// 
		/// </summary>
		protected GroupBox groupBox1;    

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="context"></param>
		public CollectionBaseEditorForm( object items , ITypeDescriptorContext context )
		{
			this.Context = context;
			InitializeComponent();
			this.EditValue = items;
		}

		/// <summary>
		/// 
		/// </summary>
		public CollectionBaseEditorForm()
		{

		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private System.Windows.Forms.TreeNode SelectedNode
		{
			get
			{
				return this.tree.SelectedNode;
			}

			set
			{
				this.tree.SelectedNode = value;

				if (value == null)
				{
					this.propGrid.SelectedObject = null;
					btnDelete.Enabled = false;
					upButton.Enabled = false;
					downButton.Enabled = false;
				}
				else
				{
					this.propGrid.SelectedObject = value.Tag;
					btnDelete.Enabled = true;
					upButton.Enabled = (value.PrevNode != null);
					downButton.Enabled = (value.NextNode != null);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual object CreateItem()
        {
			object item;

			item = this.GetType().Assembly.CreateInstance( this.ItemType.FullName );
			if( item == null )//必须用system.web.ui.webcontrols.control创建
			{
				System.Web.UI.WebControls.CheckBox control = new System.Web.UI.WebControls.CheckBox();
				item = control.GetType().Assembly.CreateInstance( this.ItemType.FullName );
			}
			return item;
		}

		protected System.Windows.Forms.TreeNode Add(System.Windows.Forms.TreeNode parent) 
		{
			System.Windows.Forms.TreeNode newnode;
			object item;

			item = this.CreateItem();

            FieldInfo fInfo = item.GetType().GetField("Container", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fInfo != null)
            {
                fInfo.SetValue(item, this.Context.Instance);
            }


			fInfo = item.GetType().GetField( "ApplyButton" );
			if( fInfo != null )
			{
				fInfo.SetValue( item , this.btnApply );
			}

			newnode = new System.Windows.Forms.TreeNode();
			if(this.TitleField != "")
			{
				PropertyInfo pInfo = item.GetType().GetProperty(this.TitleField);
				if( pInfo != null )
				{
					object titleValue = pInfo.GetValue(item , null);
					if( titleValue != null )
						newnode.Text = titleValue.ToString();
				}
			}
			else
			{
				newnode.Text = ItemType.Name;
			}

            if (newnode.Text == "")
            {
                newnode.Text = "Item";
            }
			newnode.Tag = item;
			if (parent == null)
			{
				this.tree.Nodes.Add(newnode);
			}
			else 
			{
				parent.Nodes.Add(newnode);
				parent.Expand();
			}
			btnApply.Enabled = true;

			return newnode;
		}

		private void PropertyChanged(Object sender, PropertyValueChangedEventArgs e)
		{
			if( e.ChangedItem.Label == this.TitleField )
			{
				this.SelectedNode.Text = e.ChangedItem.Value.ToString();
			}
			btnApply.Enabled = true;
		}

		private void BtnOK_click(object sender, EventArgs e) 
		{
            this.Context.OnComponentChanging();
			MethodInfo m = this.EditValue.GetType().GetMethod("Clear");
			m.Invoke(this.EditValue , null);
			m = this.EditValue.GetType().GetMethod("Add");
			for( int i = 0 ; i < tree.Nodes.Count ; i++ )
			{
				m.Invoke( this.EditValue , new object[1] { tree.Nodes[i].Tag } );
			}
			this.Context.OnComponentChanged();

			this.btnApply.Enabled = false;
			if( this.ApplyButton != null )
				this.ApplyButton.Enabled = true;
		}

		private void addItem(System.Windows.Forms.TreeNodeCollection nodes , List<object> lists)
		{
			for(int i = 0 ; i < nodes.Count ; i++)
			{
				lists.Add(nodes[i].Tag);
			}
		}

		#region GetItem
		private object[] GetItem()
		{
			ArrayList list1;
			ICollection collection1;
			object obj1;
			object[] array1;
			IEnumerator enumerator1;
			IDisposable disposable1;
			if (( this.EditValue != null) && (( this.EditValue as ICollection) != null))
			{
				list1 = new ArrayList();
				collection1 = ((ICollection)this.EditValue);
				enumerator1 = collection1.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						obj1 = enumerator1.Current;
						list1.Add(obj1);
 
					}
 
				}
				finally
				{
					disposable1 = (enumerator1 as IDisposable);
					if (disposable1 != null)
					{
						disposable1.Dispose();
 
					}
 
				}
				array1 = new object[((uint) list1.Count)];
				list1.CopyTo(array1, 0);
				return array1; 
			}
			return new object[0];
		}

		#endregion

		#region 设计
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CollectionBaseEditorForm));
			this.tree = new System.Windows.Forms.TreeView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAddRoot = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.propGrid = new System.Windows.Forms.PropertyGrid();
			this.upButton = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tree
			// 
			this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.tree.HideSelection = false;
			this.tree.ImageIndex = -1;
			this.tree.Indent = 19;
			this.tree.LabelEdit = true;
			this.tree.Location = new System.Drawing.Point(12, 8);
			this.tree.Name = "tree";
			this.tree.SelectedImageIndex = -1;
			this.tree.Size = new System.Drawing.Size(212, 328);
			this.tree.TabIndex = 1;
			this.tree.Text = "tvNodes";
			this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(10, 370);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(628, 9);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(360, 384);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(90, 25);
			this.btnOK.TabIndex = 11;
			this.btnOK.Text = "确  定";
			this.btnOK.Click += new System.EventHandler(this.BtnOK_click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(456, 384);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 25);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "取  消";
			// 
			// btnAddRoot
			// 
			this.btnAddRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddRoot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAddRoot.Location = new System.Drawing.Point(17, 344);
			this.btnAddRoot.Name = "btnAddRoot";
			this.btnAddRoot.Size = new System.Drawing.Size(71, 25);
			this.btnAddRoot.TabIndex = 6;
			this.btnAddRoot.Text = "添 加";
			this.btnAddRoot.Click += new System.EventHandler(this.btnAddRoot_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDelete.Location = new System.Drawing.Point(96, 344);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(72, 25);
			this.btnDelete.TabIndex = 8;
			this.btnDelete.Text = "删 除";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// downButton
			// 
			this.downButton.Image = ((System.Drawing.Image)(resources.GetObject("downButton.Image")));
			this.downButton.Location = new System.Drawing.Point(230, 40);
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size(28, 24);
			this.downButton.TabIndex = 3;
			this.downButton.Click += new System.EventHandler(this.DownButton_click);
			// 
			// propGrid
			// 
			this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propGrid.CommandsVisibleIfAvailable = true;
			this.propGrid.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.propGrid.LargeButtons = false;
			this.propGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propGrid.Location = new System.Drawing.Point(264, 8);
			this.propGrid.Name = "propGrid";
			this.propGrid.Size = new System.Drawing.Size(376, 328);
			this.propGrid.TabIndex = 10;
			this.propGrid.Text = "PropertyGrid";
			this.propGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyChanged);
			// 
			// upButton
			// 
			this.upButton.Image = ((System.Drawing.Image)(resources.GetObject("upButton.Image")));
			this.upButton.Location = new System.Drawing.Point(230, 8);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(28, 25);
			this.upButton.TabIndex = 2;
			this.upButton.Click += new System.EventHandler(this.UpButton_click);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Enabled = false;
			this.btnApply.Location = new System.Drawing.Point(552, 384);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(90, 25);
			this.btnApply.TabIndex = 13;
			this.btnApply.Text = "应  用";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// CollectionBaseEditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(646, 413);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.propGrid);
			this.Controls.Add(this.btnAddRoot);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.upButton);
			this.Controls.Add(this.downButton);
			this.Controls.Add(this.tree);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(544, 383);
			this.Name = "CollectionBaseEditorForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Way集合编辑器";
			this.Load += new System.EventHandler(this.WayItemCollectionEditorForm_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private void WayItemCollectionEditorForm_Load(object sender, System.EventArgs e)
		{
			tree.Nodes.Clear();
			if (EditValue != null)
			{
				
				this.ParentControl = Context.Instance;
				

				FieldInfo fInfo = this.ParentControl.GetType().GetField( "ApplyButton" );
				if( fInfo != null )
				{
					this.ApplyButton = fInfo.GetValue( this.ParentControl ) as Button;
				}

				fInfo = this.ParentControl.GetType().GetField( "ParentControl" );
				while( fInfo != null )
				{
					this.ParentControl = fInfo.GetValue( this.ParentControl );
					if (this.ParentControl != null)
					{
						fInfo = this.ParentControl.GetType().GetField("ParentControl");
					}
					else
					{
						break;
					}
				}

				object [] items = this.GetItem();
				Type[] ftype = new Type[1];
				ftype.SetValue( typeof(int) , 0 );
				this.ItemType = EditValue.GetType().GetProperty("Item" , ftype).PropertyType;

				/////////TitleFieldAttribute
				object[] atts = this.ItemType.GetCustomAttributes( typeof( TitleFieldAttribute) , true );
				if( atts.Length > 0 )
				{
					this.TitleField = ((TitleFieldAttribute)atts[0]).TitleField;
				}
				//////////////////////
				System.Windows.Forms.TreeNode childnode;

				for(int i = 0 ; i < items.Length ; i++)
				{
					childnode = new System.Windows.Forms.TreeNode();
					childnode.Text = ItemType.Name;
					MethodInfo method = items[i].GetType().GetMethod( "Clone" );

					if( method != null )
					{
						if( items[i] is ICloneable )
						{
                            childnode.Tag = ((ICloneable)items[i]).Clone();
						}
						else
						{
							childnode.Tag = method.Invoke( items[i] , BindingFlags.InvokeMethod , null , null ,null );
						}
					}
					else
					{
						childnode.Tag = this.Clone( items[i] );
					}
					object item = childnode.Tag;

                    fInfo = item.GetType().GetField("Container" , BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if( fInfo != null )
					{
						fInfo.SetValue( item , this.Context.Instance );
					}

					

					if(this.TitleField != "")
					{
						PropertyInfo pInfo = item.GetType().GetProperty(this.TitleField);
						if( pInfo != null )
						{
							object titleValue = pInfo.GetValue(item , null);
							if( titleValue != null )
								childnode.Text = titleValue.ToString();
						}
					}

					tree.Nodes.Add(childnode);
				}
				this.propGrid.SelectedObject = null;
				if( tree.Nodes.Count > 0 )
					this.SelectedNode = tree.Nodes[0];

				this.btnApply.Enabled = false;
			}
		}

		#region Clone
		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		protected virtual object Clone(object target)
		{
			Type type = target.GetType();
			object obj = type.Assembly.CreateInstance( type.FullName );
			PropertyInfo [] pInfos = type.GetProperties();
			foreach( PropertyInfo pinfo in pInfos )
			{
				if( pinfo.PropertyType.IsClass || pinfo.CanWrite == false )
					continue;
				object Value = pinfo.GetValue( target , null );

				if( Value != null )
				{
					if( pinfo.PropertyType.IsEnum )
					{
						System.Reflection.FieldInfo [] fInfo = pinfo.PropertyType.GetFields();
						for( int k = 0 ; k < fInfo.Length ; k ++ )
						{
							if( fInfo[k].IsPublic && fInfo[k].Name == (string)Value )
							{
								pinfo.SetValue( obj , fInfo[k].GetValue(null) , null );
								break;
							}
						}
					}
					else
						pinfo.SetValue( obj , Value , null);
				}
			}
			return obj;
		}
		#endregion

		#region buttonsEvent
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            if (tree.SelectedNode == null) return;
            this.SelectedNode.Remove();
			if( this.SelectedNode == null )
				this.propGrid.SelectedObject = null;
			else
				this.propGrid.SelectedObject = this.SelectedNode.Tag;
			this.btnApply.Enabled = true;
			
		}

		private void btnAddChild_Click(object sender, System.EventArgs e)
		{
			this.SelectedNode = this.Add(this.SelectedNode);
		}

		private void DownButton_click(object sender, EventArgs e) 
		{
			System.Windows.Forms.TreeNodeCollection col;
			
			System.Windows.Forms.TreeNode movingNode = SelectedNode;
			System.Windows.Forms.TreeNode nextnode = movingNode.NextNode;

			if (movingNode.Parent != null)
			{
				col = movingNode.Parent.Nodes;
			}
			else
			{
				col = tree.Nodes;
			}
			movingNode.Remove();
			col.Insert(nextnode.Index + 1, movingNode);

			SelectedNode = movingNode;
			//btnApply.Enabled = true;
		}

		/// <summary>
		/// Moves the selected item up one.
		/// </summary>
		/// <param name="sender">Source object</param>
		/// <param name="e">Event arguments</param>
		private void UpButton_click(object sender, EventArgs e) 
		{
			System.Windows.Forms.TreeNodeCollection col;

			System.Windows.Forms.TreeNode movingNode = SelectedNode;
			System.Windows.Forms.TreeNode prevnode = movingNode.PrevNode;

			if (movingNode.Parent != null)
			{
				col = movingNode.Parent.Nodes;
			}
			else
			{
				col = tree.Nodes;
			}
			movingNode.Remove();
			col.Insert(prevnode.Index, movingNode);

			SelectedNode = movingNode;
			//btnApply.Enabled = true;
		}

		
		private void tree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			this.SelectedNode = e.Node;
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			this.BtnOK_click( null , EventArgs.Empty );
		}

		private void btnAddRoot_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.TreeNode rootNode = Add(null);
			SelectedNode = rootNode;
			this.btnApply.Enabled = true;
		}

		#endregion

		
	}
}
