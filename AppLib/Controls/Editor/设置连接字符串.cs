//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace AppLib.Controls.Editor
//{
//    partial class 设置连接字符串 : Form
//    {
//        ConnectionStringConfig m_conStrs;
//        public 设置连接字符串(ConnectionStringConfig conStrs)
//        {
//            m_conStrs = conStrs;
//            InitializeComponent();
//            System.Data.DataTable dtable = new DataTable();
//            dtable.Columns.Add("名称", typeof(string));
//            dtable.Columns.Add("连接字符串", typeof(string));
//            for (int i = 0; i < m_conStrs.Count; i++)
//            {
//                DataRow row = dtable.NewRow();
//                row[0] = m_conStrs[i].Name;
//                row[1] = m_conStrs[i].Value;
//                dtable.Rows.Add(row);
//            }

//            if (m_conStrs.Count == 0)
//            {
//                DataRow row = dtable.NewRow();
//                row[0] = "sysDefaultConStr";
//                row[1] = "";
//                dtable.Rows.Add(row);

//                row = dtable.NewRow();
//                row[0] = "ccdDB";
//                row[1] = "";
//                dtable.Rows.Add(row);
//            }

//            dataGridView1.DataSource = dtable;
//        }

//        private void btnOK_Click(object sender, EventArgs e)
//        {
//            m_conStrs.Save(dataGridView1.DataSource as DataTable);
//            this.DialogResult = System.Windows.Forms.DialogResult.OK;
//        }
//    }
//}
