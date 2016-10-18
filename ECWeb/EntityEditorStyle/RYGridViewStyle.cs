using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AppLib.Controls;
namespace ECWeb.DataEditorStyle
{
    public class RYGridViewStyle : AppLib.Controls.IGridViewStyle
    {
        public void SetStyle(object obj)
        {
            GridView gridview = obj as GridView;
            gridview.HeaderStyle.CssClass = "HeaderStyle";
            gridview.FooterStyle.CssClass = "FooterStyle";
            gridview.CellPadding = 5;
            gridview.BorderWidth = new Unit(1, UnitType.Pixel);
            gridview.GridLines = System.Web.UI.WebControls.GridLines.None;
            gridview.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            gridview.BorderColor = System.Drawing.ColorTranslator.FromHtml("#d8d8d8");
            gridview.CssClass = "ryGrid";
            gridview.ShowFooter = false;
            gridview.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");

            string pagerid = ((AppLib.Controls.EntityGridView)gridview).AspNetPager;

            if (!string.IsNullOrEmpty(pagerid))
            {
                List<System.Web.UI.Control> pagers = AppHelper.GetControlsByTypes(gridview.Page, new Type[] { typeof(AppLib.Controls.Pager) });
                AppLib.Controls.Pager pager = pagers.FirstOrDefault(m => m.ID == pagerid) as AppLib.Controls.Pager;
                if (pager != null)
                {
                    pager.AlwaysShow = false;
                    pager.PageSize = 20;
                    pager.CssClass="pageBtn";
                    pager.CurrentPageButtonClass="pageBtn";
                    pager.FirstLastButtonClass="pageBtn";
                    pager.FirstLastButtonStyle="pageBtn";
                    pager.FirstPageText="首页";
                    pager.LastPageText="尾页";
                    pager.NextPageText="下一页";
                    pager.PrevPageText = "上一页";
                }
            }
            //gridview.Width = new Unit(100, UnitType.Percentage);
        }
    }
}