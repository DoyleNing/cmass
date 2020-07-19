using CODING.com;
using CODING.com.form;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CodeMachine.com.form
{
    public partial class QueryPathForm : Form
    {
        String sqlRole = "SELECT PUR.TARGET, ZD.F_NAME\n" +
                "  FROM PUB_ROLES PR, PUB_USER_ROLE PUR\n" +
                "  LEFT JOIN LSGYZD ZD\n" +
                "    ON ZD.F_GYBH = PUR.TARGET\n" +
                " WHERE PR.ROLE_ID = PUR.ROLE_ID\n" +
                "   AND PR.ROLE_ID IN\n" +
                "       (SELECT PRO.ROLE_ID\n" +
                "          FROM PUB_ROLE_OPERATION PRO, PUB_OPERATIONS PO\n" +
                "         WHERE PRO.OPERATION_CODE = PO.OPERATION_CODE\n" +
                "           AND PO.OPERATION_CODE IN\n" +
                "               (SELECT A.OPERATION_CODE\n" +
                "                  FROM PUB_OPERATIONS A\n" +
                "                 WHERE A.FUNCTION_CODE IN\n" +
                "                       (SELECT A.FUNCTION_CODE\n" +
                "                          FROM PUB_MENU_ITEM A\n" +
                "                         WHERE A.MENU_ID IN\n" +
                "                               (SELECT T.MENU_ID\n" +
                "                                  FROM PUB_MENU_STRU T\n" +
                "                                 WHERE MENU_TYPE_ID = '1'\n" +
                "                                 START WITH MENU_ID =\n" +
                "                                            '?'\n" +
                "                                CONNECT BY NOCYCLE\n" +
                "                                 PRIOR MENU_ID = PARENT_MENU_ID))))\n";
        public QueryPathForm()
        {
            InitializeComponent();
        }

        private void MetroButton2_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text = "";
        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Trim() == "")
            {
                MessageBox.Show("菜单名不能为空");
                return;
            }
            else {
                String sql = "SELECT T.MENU_ID,T.PATH_NAME FROM PUB_MENU_STRU T WHERE T.PATH_NAME LIKE '%?%' AND T.MENU_TYPE_ID!='1'";
                List<Dictionary<String, Object>> list = new BaseDao().executeQuery(sql,new List<object>() { metroTextBox1.Text.Trim() });
                listView1.Items.Clear();             
                for (int i = 0; i < list.Count; i++)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    Dictionary<String, Object> dic = list[i];
                    listViewItem.Text = Convert.ToString(i + 1);
                    listViewItem.SubItems.Add(Convert.ToString(dic["PATH_NAME"]));
                    listViewItem.SubItems.Add(Convert.ToString(dic["MENU_ID"]));
                    listView1.Items.Add(listViewItem);
                }
                
            }

        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            List<Dictionary<String, Object>> list = new BaseDao().executeQuery(sqlRole,new List<object>() { listView1.FocusedItem.SubItems[2].Text.ToString() });
            listView2.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem();
                Dictionary<String, Object> dic = list[i];
                listViewItem.Text = Convert.ToString(dic["TARGET"]);
                listViewItem.SubItems.Add(Convert.ToString(dic["F_NAME"]));
                listView2.Items.Add(listViewItem);
            }

        }
    }
}
