using CODING.com;
using CODING.com.form;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMachine
{
    public partial class DataSourceForm : MetroForm
    {
        public DataSourceForm()
        {
            InitializeComponent();
            textBox1.Text = GetSettingString("userId");
            textBox2.Text = GetSettingString("password");
            textBox3.Text = GetSettingString("address");
            textBox4.Text = GetSettingString("serviceName");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSource.cofUserId = textBox1.Text;
            DataSource.cofPassword = textBox2.Text;
            DataSource.cofAddress = textBox3.Text;
            DataSource.cofServiceName = textBox4.Text;
            DataSource dataSource = new DataSource();

            dataSource.testConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataSourceForm_Load(object sender, EventArgs e)
        {

        }
        //读取客户配置
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception e)
            {
                throw new Exception("读取客户配置失败\n" + e.Message); ;

            }
        }

    }
}
