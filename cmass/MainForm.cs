using MetroFramework.Forms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeMachine.com.form;
using MetroFramework.Controls;
using System.Reflection;
using CODING.com.form;
using CODING.com;
using System.IO;

namespace CodeMachine
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void TestButton_Click(object sender, EventArgs e)
        {
            DataSourceForm dataSourceForm = new DataSourceForm();
            dataSourceForm.ShowDialog();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

            treeView1.ExpandAll();
            Assembly asm = Assembly.GetExecutingAssembly();//如果是当前程序集
            //如果是其他文件

            AssemblyDescriptionAttribute asmdis = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyDescriptionAttribute));
            AssemblyCopyrightAttribute asmcpr = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCopyrightAttribute));
            AssemblyCompanyAttribute asmcpn = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCompanyAttribute));
         //   string s = string.Format("{0}  {1}  {2} ", asmdis.Description, asmcpr.Copyright, asmcpn.Company);

            label1.Text = "版权信息:"+ Application.ProductVersion.ToString()+" "+ asmcpr.Copyright;
        }

        private void 数据库链接测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSourceForm dataSourceForm = new DataSourceForm();
            dataSourceForm.ShowDialog();
        }

        private void ExportMenuButton_Click(object sender, EventArgs e)
        {
            ExportMenuForm exportMenuForm = new ExportMenuForm();
            Add_TabPage("菜单导出", exportMenuForm);
        }

        private void CreateEntityButton_Click(object sender, EventArgs e)
        {
            CreateEntityForm createEntityForm = new CreateEntityForm();
            Add_TabPage("生成实体类", createEntityForm);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CreateForm createForm = new CreateForm();
            Add_TabPage("框架生成", createForm);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            PmassForm pmassForm = new PmassForm();
            Add_TabPage("pmass", pmassForm);          
        }

        private void TreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode trNode = treeView1.SelectedNode;
            if (trNode.Name== "menuPath")
            {
                
                QueryPathForm queryForm = new QueryPathForm();
                Add_TabPage("菜单路径查询", queryForm);
            }
            if (trNode.Name == "createForm")
            {
                CreateForm createForm = new CreateForm();
                Add_TabPage("框架生成", createForm);
            }
            if (trNode.Name == "createEntity")
            {
                CreateEntityForm createEntityForm = new CreateEntityForm();
                Add_TabPage("生成实体类", createEntityForm);
            }
            if (trNode.Name == "exportSql")
            {
                ExportMenuForm exportMenuForm = new ExportMenuForm();
                Add_TabPage("菜单导出", exportMenuForm);
            }
            if (trNode.Name == "interfaceTest")
            {
                InterfaceTestForm interfaceTestForm = new InterfaceTestForm();
                Add_TabPage("接口测试", interfaceTestForm);
            }
            if (trNode.Name == "autoConect") {
                AutoConectForm autoConectForm = new AutoConectForm();
                Add_TabPage("自动联网", autoConectForm);
            }
            if (trNode.Name == "pmass")
            {
                PmassForm pmassForm = new PmassForm();
                Add_TabPage("pmass", pmassForm);
            }
            /*           if (trNode.Name == "tongji")
                       {
                           DealExcelForm dealExcelForm = new DealExcelForm();
                           Add_TabPage("统计", dealExcelForm);
                       }
           */
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed == true)
            {
                splitContainer1.Panel1Collapsed = false;
                button2.Text = "《";
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
                button2.Text = "》";
            }
        }


        public bool TabControlCheckHave(TabControl tab, string tabName)
        {
            for (int i = 0; i < tab.TabCount; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    tab.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }
        public void Add_TabPage(string str, Form myForm)
        {
            if (TabControlCheckHave(this.metroTabControl1, str))
            {
                return;
            }
            else
            {
                metroTabControl1.TabPages.Add(str);
                metroTabControl1.SelectTab(metroTabControl1.TabPages.Count - 1);
                metroTabControl1.TabPages[metroTabControl1.TabPages.Count - 1].AutoScroll = true;

                myForm.FormBorderStyle = FormBorderStyle.None;
                myForm.Dock = DockStyle.Fill;
               // myForm.StartPosition = FormStartPosition.Manual;
                myForm.TopLevel = false;
                myForm.Parent = metroTabControl1.SelectedTab;
                myForm.Show();
            }
        }       
        private void MetroTabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(metroTabControl1.SelectedIndex ==0) return;
            metroTabControl1.TabPages.RemoveAt(metroTabControl1.SelectedIndex);

        }
    }
}
