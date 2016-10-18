using AppLib.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ECWeb.WebForm
{
    /// <summary>
    /// 根据需要，自动隐藏grid，dataEditor
    /// </summary>
    public class AutoHideGridPage : VerifyPage
    {
        /// <summary>
        /// 和Grid一起隐藏的类型
        /// </summary>
        public virtual Type[] TypeWithGrid
        {
            get
            {
                return new Type[] { typeof(EntityGridView), typeof(EntityToolbar)};
            }
        }

        /// <summary>
        /// 新增数据后，是否继续保持grid的隐藏状态
        /// </summary>
        public virtual bool KeepHideGridAfterInsertNewData
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            List<Control> editors = AppHelper.GetControlsByTypes(this.GetType(), this, new Type[] { typeof(EntityEditor) });
            foreach (EntityEditor editor in editors)
            {
                if (!IsPostBack)
                {
                    editor.Visible = false;
                }
                editor.BeforeChangeMode += editor_BeforeChangeMode;
                editor.AfterSave += editor_AfterSave;
            }
        }

        void editor_AfterSave(object database, object sender, ActionType actionType, object dataitem)
        {
            EntityEditor editor = sender as EntityEditor;
            if (actionType == ActionType.Update || KeepHideGridAfterInsertNewData == false)
            {
                //保存后，显示grid，隐藏editor
                List<Control> grids = AppHelper.GetControlsByTypes(this.GetType(), this, TypeWithGrid);
                foreach (Control grid in grids)
                {
                    grid.Visible = true;
                    if (grid is EntityGridView)
                        ((EntityGridView)grid).DataBind();
                }
                editor.Visible = false;
            }
            else if (actionType == ActionType.Insert)
            {
                editor.ChangeMode(ActionType.Insert, null);
            }
           
        }

        void editor_BeforeChangeMode(object sender, ActionType actionType, object dataID)
        {
            EntityEditor editor = sender as EntityEditor;
            //进入编辑模式，隐藏页面上所有的grid，显示editor
            //离开编辑模式，显示页面上所有grid，隐藏editor
            List<Control> grids = AppHelper.GetControlsByTypes(this.GetType(), this, TypeWithGrid);
            foreach (Control grid in grids)
            {
                grid.Visible = (actionType== ActionType.None ? true : false);
                if (grid is EntityGridView && actionType == ActionType.None && IsPostBack)
                    ((EntityGridView)grid).DataBind();
            }

           
            editor.Visible = (actionType == ActionType.None ? false : true);
     
        }
    }
}