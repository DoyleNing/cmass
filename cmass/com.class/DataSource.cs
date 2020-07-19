using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMachine
{
    class DataSource
    {
        private static String address;//服务器地址 10.5.2.11
        private static String serviceName;//服务器名 qcdb
        private static String userId;//用户名 cfssitgm
        private static String password;//密码 password


        public static String cofAddress { get => address; set => address = value; }

        public static String cofServiceName { get => serviceName; set => serviceName = value; }

        public static String cofUserId { get => userId; set => userId = value; }

        public static String cofPassword { get => password; set => password = value; }

        public void testConnection() {
            String connString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + address + ")(PORT=1521))" +
                        "(CONNECT_DATA=(SERVICE_NAME = " + serviceName + ")));User Id=" + userId + ";Password=" + password + ";";

            OracleConnection conn = new OracleConnection(connString);
            //创建一个新连接
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    MessageBox.Show("链接成功!","系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    //更新配置文件记住本次链接
                    UpdateSettingString("userId", userId);
                    UpdateSettingString("password",password);
                    UpdateSettingString("address", address);
                    UpdateSettingString("serviceName", serviceName);
                    UpdateSettingString("connString", connString);

                }
                else {
                    MessageBox.Show("链接失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (Exception ee){
                throw new Exception("链接失败:" + ee.Message);
            }finally{
                conn.Close(); //关闭连接
            }
        }
        //更新客户配置
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
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
                throw new Exception("读取客户配置失败:"+e.Message);
            }
        }
    }
}
