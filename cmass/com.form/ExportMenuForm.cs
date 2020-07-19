using CODING.com;
using CODING.com.form;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CodeMachine.com.form
{
    public partial class ExportMenuForm : Form
    {
        private static String PUB_MODULES = "SELECT * FROM PUB_MODULES T\n" +
            " WHERE T.MODULE_CODE IN\n" +
            "       (SELECT B.MODULE_CODE\n" +
            "          FROM PUB_MENU_ITEM B\n" +
            "         WHERE B.MENU_ID IN\n" +
            "               (SELECT A.MENU_ID\n" +
            "                  FROM PUB_MENU_STRU A\n" +
            "                 WHERE MENU_TYPE_ID = '1'\n" +
            "                 START WITH MENU_ID = '?'\n" +
            "                CONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID))";
        private static String PUB_FUNCTIONS = "SELECT * FROM PUB_FUNCTIONS T\n" +
                "\t WHERE T.FUNCTION_CODE IN\n" +
                "\t\t   (SELECT A.FUNCTION_CODE\n" +
                "\t\t\t  FROM PUB_MENU_ITEM A\n" +
                "\t\t\t WHERE A.MENU_ID IN\n" +
                "\t\t\t\t   (SELECT T.MENU_ID\n" +
                "\t\t\t\t\t  FROM PUB_MENU_STRU T\n" +
                "\t\t\t\t\t WHERE MENU_TYPE_ID = '1'\n" +
                "\t\t\t\t\t START WITH MENU_ID = '?'\n" +
                "\t\t\t\t\tCONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID))";
        private static String PUB_OPERATIONS = "SELECT * FROM PUB_OPERATIONS A\n" +
                " WHERE A.FUNCTION_CODE IN\n" +
                "       (SELECT A.FUNCTION_CODE\n" +
                "          FROM PUB_MENU_ITEM A\n" +
                "         WHERE A.MENU_ID IN\n" +
                "               (SELECT T.MENU_ID\n" +
                "                  FROM PUB_MENU_STRU T\n" +
                "                 WHERE MENU_TYPE_ID = '1'\n" +
                "                 START WITH MENU_ID = '?'\n" +
                "                CONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID))";
        private static String PUB_URLS = "SELECT * FROM PUB_URLS A\n" +
                " WHERE A.OPERATION_CODE IN\n" +
                "       (SELECT A.OPERATION_CODE\n" +
                "          FROM PUB_OPERATIONS A\n" +
                "         WHERE A.FUNCTION_CODE IN\n" +
                "               (SELECT A.FUNCTION_CODE\n" +
                "                  FROM PUB_MENU_ITEM A\n" +
                "                 WHERE A.MENU_ID IN\n" +
                "                       (SELECT T.MENU_ID\n" +
                "                          FROM PUB_MENU_STRU T\n" +
                "                         WHERE MENU_TYPE_ID = '1'\n" +
                "                         START WITH MENU_ID = '?'\n" +
                "                        CONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID)))";
        private static String PUB_MENU_ITEM = "SELECT --DECODE(LENGTH(T.MENU_ID),30,'!'||SUBSTR(T.MENU_ID, 2),'!'||T.MENU_ID)MENU_ID,\n" +
                "\t   T.MENU_ID,\n" +
                "       T.MENU_NAME,\n" +
                "       T.REQUEST_ACTION,\n" +
                "       T.TARGET,\n" +
                "       T.FUNCTION_CODE,\n" +
                "       T.MODULE_CODE,\n" +
                "       T.APP_CODE,\n" +
                "       T.IS_LEAF,\n" +
                "       T.ICON\n" +
                "  FROM PUB_MENU_ITEM T\n" +
                "   WHERE T.MENU_ID IN\n" +
                "       (SELECT A.MENU_ID\n" +
                "          FROM PUB_MENU_STRU A\n" +
                "         WHERE MENU_TYPE_ID = '1'\n" +
                "         START WITH MENU_ID = '?'\n" +
                "        CONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID)";
        private static String PUB_MENU_STRU = "SELECT --DECODE(LENGTH(T.MENU_STRU_ID),30,'!'||SUBSTR(T.MENU_STRU_ID, 2),'!'||T.MENU_STRU_ID) MENU_STRU_ID,\n" +
                "       T.MENU_STRU_ID,\n" +
                "       T.MENU_TYPE_ID,\n" +
                "       --DECODE(LENGTH(T.MENU_ID),30,'!'||SUBSTR(T.MENU_ID, 2),'!'||T.MENU_ID)MENU_ID,\n" +
                "       T.MENU_ID,\n" +
                "       --DECODE(LENGTH(T.PARENT_MENU_ID),30,'!'||SUBSTR(T.PARENT_MENU_ID, 2),'!'||T.PARENT_MENU_ID) PARENT_MENU_ID,\n" +
                "       T.PARENT_MENU_ID,\n" +
                "       T.MENU_PATH,\n" +
                "       T.PATH_NAME,\n" +
                "       T.SEQ\n" +
                "  FROM PUB_MENU_STRU T\n" +
                "   WHERE MENU_TYPE_ID = '1'\n" +
                " START WITH MENU_ID = '?'\n" +
                "CONNECT BY NOCYCLE PRIOR MENU_ID = PARENT_MENU_ID";
        BaseDao baseDao = new BaseDao();
        public ExportMenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String localFile = DataSource.GetSettingString("localFile");
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择导出文件路径";
            if (localFile != null|| localFile != "") {
                dialog.SelectedPath = localFile;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;
                DataSource.UpdateSettingString("localFile", foldPath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入菜单名!");
            }
            else
            {
                listView1.Items.Clear();
                List<Object> list = new List<object>();
                list.Add(textBox1.Text.Trim());
                String sql = "SELECT T.MENU_ID,T.PATH_NAME FROM PUB_MENU_STRU T WHERE T.PATH_NAME LIKE '%?%' AND T.MENU_TYPE_ID='1'";
                List<Dictionary<String, Object>> backList = baseDao.executeQuery(sql, list);
                for (int i = 0; i < backList.Count; i++)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    Dictionary<String, Object> dic = backList[i];
                    listViewItem.Text = Convert.ToString(i + 1);
                    listViewItem.SubItems.Add(Convert.ToString(dic["MENU_ID"]));
                    listViewItem.SubItems.Add(Convert.ToString(dic["PATH_NAME"]));
                    listView1.Items.Add(listViewItem);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox3.Text += listView1.FocusedItem.SubItems[1].Text.ToString() + ";";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ("".Equals(textBox2.Text.Trim()))
            {
                MessageBox.Show("导出路径不能为空!");
                return;
            }
            if ("".Equals(textBox3.Text.Trim()))
            {
                MessageBox.Show("菜单ID不能为空!");
                return;
            }
            String[] menuIds = textBox3.Text.Trim().Split(';');
            export(menuIds);
        }
        public void export(String[] menuIds)
        {
 
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String filePath = textBox2.Text.Trim() +"\\"+ textBox1.Text.Trim() + "导出菜单.sql";             
                //创建并打开文件流
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                for (int i = 0; i < menuIds.Length; i++)
                {
                    //顺序不能调整
                    WRRITE2TXT(writer, menuIds[i], PUB_MODULES, "PUB_MODULES");
                    WRRITE2TXT(writer, menuIds[i], PUB_FUNCTIONS, "PUB_FUNCTIONS");
                    WRRITE2TXT(writer, menuIds[i], PUB_OPERATIONS, "PUB_OPERATIONS");
                    WRRITE2TXT(writer, menuIds[i], PUB_URLS, "PUB_URLS");
                    WRRITE2TXT(writer, menuIds[i], PUB_MENU_ITEM, "PUB_MENU_ITEM");
                    WRRITE2TXT(writer, menuIds[i], PUB_MENU_STRU, "PUB_MENU_STRU");
                    
                }
                
                writer.Flush();

                MessageBox.Show("导出成功!");

            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message);
            }
            finally
            {
                if (null != writer)
                {
                    writer.Flush();
                    writer.Close();
                }
                if (null != fs)
                {
                    fs.Close();
                }
            }
        }
        public void WRRITE2TXT(StreamWriter writer, Object obj, String sql, String tableName)
        {
            List<Dictionary<String, Object>> list = baseDao.executeQuery(sql,new List<Object> { Convert.ToString(obj) });
            foreach (Dictionary<String, Object> dict in list)
            {
                writer.Write("insert into " + tableName + " (");
                String[] keys = dict.Keys.ToArray();
                for (int i=0;i<keys.Length;i++) {
                    writer.Write(keys[i]);
                    if (i < keys.Length - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.Write(")\nvalues (");
                for (int i = 0; i < keys.Length; i++)
                {
                    if (dict[keys[i]].GetType() == typeof(System.Decimal))
                    {
                        writer.Write(dict[keys[i]]);

                    }
                    else if (dict[keys[i]].GetType() == typeof(System.DBNull))
                    {
                        writer.Write("null");

                    }
                    else if (dict[keys[i]].GetType() == typeof(System.String))
                    {
                        writer.Write("'" + dict[keys[i]] + "'");

                    }
                    else {
                        writer.Write("'" + dict[keys[i]] + "'");
                    }
                    if (i < keys.Length - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.Write(");\n\n");

            }
            
        }
    }
}
