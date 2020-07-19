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
    public partial class CreateEntityForm : Form
    {
        public CreateEntityForm()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入表名!");
                return;
            }
            else {
                String sql = "SELECT UT.COLUMN_NAME COLUMNNAME, UT.DATA_TYPE DATATYPE, UT.NULLABLE, UC.COMMENTS\n" +
                    "  FROM USER_TAB_COLS UT\n" +
                    "  LEFT JOIN USER_COL_COMMENTS UC\n" +
                    "    ON UC.TABLE_NAME = UT.TABLE_NAME\n" +
                    "   AND UC.COLUMN_NAME = UT.COLUMN_NAME\n" +
                    " WHERE UT.TABLE_NAME = '?'\n";
                List<Dictionary<String, Object>> list = new BaseDao().executeQuery(sql, new List<Object> { textBox1.Text.Trim() });
                String[] columnName = new String[list.Count()];
                String[] dataType = new String[list.Count()];
                String[] nullAble = new String[list.Count()];
                String[] comments = new String[list.Count()];
                for (int i = 0; i < list.Count(); i++)
                {
                    columnName[i] = list[i]["COLUMNNAME"].ToString();
                    dataType[i] = list[i]["DATATYPE"].ToString();
                    nullAble[i] = list[i]["NULLABLE"].ToString();
                    comments[i] = list[i]["COMMENTS"].GetType() == typeof(System.DBNull) ? "默认字段" + i : list[i]["COMMENTS"].ToString().Replace("\"", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
                }
                if (list.Count == 0)
                {
                    MessageBox.Show("未找到对应的表字段信息!");
                    return;
                }

                StringBuilder s = new StringBuilder();
                s.Append("import org.loushang.next.dao.Column;\r\n" +
                        "import org.loushang.next.dao.Table;\r\n" +
                        "import org.loushang.next.dao.Transient;\r\n" +
                        "import org.loushang.next.data.Rule;\r\n" +
                        "import org.loushang.next.data.StatefulDatabean;\r\n" +
                        "import java.math.BigDecimal;\r\n" +
                        "\r\n" +
                        "/*\r\n" +
                        "作者: CODE MACHINE  创建日期:" + System.DateTime.Now + "\r\n" +
                        "*/\r\n" +
                        "@Table(tableName = \"" + strAllUpp(textBox1.Text.Trim()) + "\" , keyFields =\"" + columnName[0] + "\")\r\n" +
                        "public class " + strAllUpp(textBox1.Text.Trim()) + " extends StatefulDatabean {\r\n");
                for (int i = 0; i < columnName.Length; i++)
                {
                    //写注释
                    s.Append("\r\n" +
                            "\t/*\r\n\t\t" + comments[i] + "\r\n\t*/\r\n");
                    //非空规则
                    if (nullAble[i].Equals("N"))
                    {
                        s.Append("\t@Rule(value=\"require\")\r\n");
                    }
                    s.Append("\t@Column(name = \"" + columnName[i] + "\")\r\n");
                    //根据类型来定义变量
                    if (dataType[i].Contains("NUMBER") || dataType[i].Contains("FLOAT"))
                    {
                        //数值类型
                        s.Append("\tprivate BigDecimal " + strUpp(columnName[i]) + ";");
                    }
                    else if (dataType[i].Contains("INT"))
                    {
                        //整数类型
                        s.Append("\tprivate int " + strUpp(columnName[i]) + ";");
                    }
                    else
                    {
                        //其余皆为String
                        s.Append("\tprivate String " + strUpp(columnName[i]) + ";");
                    }
                }
                s.Append("\r\n\t/*---------------------以下为变量getset方法区域----------------------------*/\r\n");
                for (int i = 0; i < columnName.Length; i++)
                {
                    //写注释
                    s.Append("\r\n" +
                            "\t/*\r\n\t\t" + comments[i] + "\r\n\t*/\r\n");
                    if (dataType[i].Contains("NUMBER") || dataType[i].Contains("FLOAT"))
                    {
                        //set方法
                        s.Append("\tpublic void set" + strAllUpp(columnName[i]) + "( BigDecimal " + strUpp(columnName[i]) + "){\r\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                        //写注释
                        s.Append("\r\n" +
                                "\t/*\r\n\t\t" + comments[i] + "\r\n\t*/\r\n");
                        //get方法
                        s.Append("\tpublic BigDecimal get" + strAllUpp(columnName[i]) + "(){\r\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                    }
                    else if (dataType[i].Contains("INT"))
                    {
                        //set方法
                        s.Append("\tpublic void set" + strAllUpp(columnName[i]) + "( int " + strUpp(columnName[i]) + "){\r\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                        //写注释
                        s.Append("\r\n" +
                                "\t/*\r\n\t\t" + comments[i] + "\r\n\t*/\r\n");
                        //get方法
                        s.Append("\tpublic int get" + strAllUpp(columnName[i]) + "(){\r\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                    }
                    else
                    {
                        //set方法
                        s.Append("\tpublic void set" + strAllUpp(columnName[i]) + "( String " + strUpp(columnName[i]) + "){\r\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                        //写注释
                        s.Append("\r\n" +
                                "\t/*\r\n\t\t" + comments[i] + "\r\n\t*/\r\n");
                        //get方法
                        s.Append("\tpublic String get" + strAllUpp(columnName[i]) + "(){\r\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\r\n" +
                                "\t}");
                    }
                }
                s.Append("\r\n}");
                textBox2.Text = s.ToString();
            }
        }
        //更改首字母大写
        public static String strAllUpp(String s)
        {
            String[] str = s.ToLower().Split('_');
            String sb = "";
            for (int i = 0; i < str.Length; i++)
            {
                sb = sb + str[i].Substring(0, 1).ToString().ToUpper() + str[i].Substring(1);
            }
            return sb;
        }
        //更改为驼峰命名
        public static String strUpp(String s)
        {
            String[] str = s.ToLower().Split('_');
            String sb = str[0];
            for (int i = 1; i < str.Length; i++)
            {
                sb = sb + str[i].Substring(0, 1).ToString().ToUpper() + str[i].Substring(1);
            }
            return sb;
        }
    }
}
