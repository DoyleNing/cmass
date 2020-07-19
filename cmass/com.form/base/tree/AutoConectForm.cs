using CodeMachine;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CODING.com.form
{
    public partial class AutoConectForm : Form
    {
        private bool ie_Read = false;
        private bool isOpen = false;
        int NOMARL_INTERVAL;//正常时的中断
        int UNNOMARL_INTERVAL;//不正常是的中断
        public AutoConectForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//跨线程
            textBox1.Text = DataSource.GetSettingString("URL");
            textBox2.Text = DataSource.GetSettingString("PARAM");
            NOMARL_INTERVAL = Convert.ToInt32(DataSource.GetSettingString("NOMARL_INTERVAL").ToString());
            UNNOMARL_INTERVAL = Convert.ToInt32(DataSource.GetSettingString("UNNOMARL_INTERVAL").ToString());
        }

        private void AutoConectForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //线程没开
            if (!isOpen) {
                isOpen = true;

                Start();

                //如果textbox有内容将光标移到最后
                if (textBox3.Text.Length > 0)
                {
                    textBox3.Select(textBox3.Text.Length - 1, 0);
                    textBox3.ScrollToCaret();
                }
                button1.Text = "停止";
                button1.BackColor = Color.Red;
            }
            else {
                isOpen = false;

                Stop();

                //如果textbox有内容将光标移到最后
                if (textBox3.Text.Length > 0)
                {
                    textBox3.Select(textBox3.Text.Length - 1, 0);
                    textBox3.ScrollToCaret();
                }
                button1.Text = "启动";
                button1.BackColor = Color.Green;
            }
        }
        private void ie_DocumentComplete(object pDisp, ref object URL)
        {
            ie_Read = true;
        }
        private void compWait()
        {
            while (ie_Read != true)
            {
                Application.DoEvents();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataSource.UpdateSettingString("URL",textBox1.Text.Trim());
            DataSource.UpdateSettingString("PARAM",textBox2.Text.Trim());
            MessageBox.Show("更新配置文件成功,重启后生效!");
        }
        public void aotuConnect() {
            while (true) {
                //网络可用
                if (IsInternetAvailable())
                {
                    textBox3.Text += System.DateTime.Now + "\r\n  ----网络状态正常----\r\n\r\n";
                    textBox3.Text += System.DateTime.Now + "\r\n  ----休眠"+(NOMARL_INTERVAL/1000/60)+"分钟----\r\n\r\n";
                    textBox3.Select(textBox3.Text.Length - 1, 0);
                    textBox3.ScrollToCaret();
                    Thread.Sleep(NOMARL_INTERVAL);
                }
                else
                {
                    textBox3.Text += System.DateTime.Now + "\r\n  ----网络状态异常----\r\n\r\n";
                    textBox3.Text += System.DateTime.Now + "\r\n  ----系统将自动联网----\r\n\r\n";
                    String url = DataSource.GetSettingString("URL");
                    String param = DataSource.GetSettingString("PARAM");
                    textBox3.Text += "\r\n  ----url:" + url + "----\r\n\r\n";
                    textBox3.Text += "\r\n  ----参数:" + param + "----\r\n\r\n";

                    textBox3.Select(textBox3.Text.Length - 1, 0);
                    textBox3.ScrollToCaret();

                    SHDocVw.InternetExplorer ie = new SHDocVw.InternetExplorer();
                    ie.DocumentComplete += ie_DocumentComplete;//等待页面读取事件  
                    ie.Navigate(url);
                    ie.Visible = false;
                    compWait();
                    textBox3.Text += "\r\n  ----联网中请等待----\r\n\r\n";

                    textBox3.Select(textBox3.Text.Length - 1, 0);
                    textBox3.ScrollToCaret();
                    HTMLDocument doc = null;
                    try {
                        doc = ie.Document;
                        
                        String[] attr = param.Split(';');
                        for (int i = 0; i < param.Split(';').Length; i++)
                        {
                            //自定义规则，如果某一个值中包含#Click则为点击按钮
                            if (attr[i].Split(':')[1].Contains("#Click"))
                            {
                                if (doc.getElementById(attr[i].Split(':')[0]) == null)
                                {
                                    textBox3.Text += "\r\n  ----获取联网页面失败,请检查配置文件----\r\n\r\n";
                                    textBox3.Select(textBox3.Text.Length - 1, 0);
                                    textBox3.ScrollToCaret();
                                    break;
                                }
                                else
                                {
                                    doc.getElementById(attr[i].Split(':')[0]).click();
                                }
                            }
                            else
                            {
                                if (doc.getElementById(attr[i].Split(':')[0]) == null)
                                {
                                    textBox3.Text += "\r\n  ----获取联网页面失败,请检查配置文件----\r\n\r\n";
                                    textBox3.Select(textBox3.Text.Length - 1, 0);
                                    textBox3.ScrollToCaret();
                                    break;
                                }
                                else
                                {
                                    doc.getElementById(attr[i].Split(':')[0]).setAttribute("value", attr[i].Split(':')[1]);
                                }
                            }
                        }
                        doc.close();
                        ie.Quit();
                        ie_Read = false;
                    }
                    catch (Exception) {
                        doc.close();
                        ie.Quit();
                        ie_Read = false;
                    }
                    if (IsInternetAvailable())
                    {
                        textBox3.Text += System.DateTime.Now + "\r\n  ----自动联网成功----\r\n\r\n";
                        textBox3.Text += System.DateTime.Now + "\r\n  ----休眠" + (NOMARL_INTERVAL / 1000/60) + "分钟----\r\n\r\n";
                        textBox3.Select(textBox3.Text.Length - 1, 0);
                        textBox3.ScrollToCaret();
                        Thread.Sleep(NOMARL_INTERVAL);
                    }
                    else
                    {
                        textBox3.Text += System.DateTime.Now + "\r\n  ----自动联网失败----\r\n\r\n";
                        textBox3.Text += System.DateTime.Now + "\r\n  ----休眠" + (UNNOMARL_INTERVAL / 1000) + "秒后自动重连----\r\n\r\n";
                        textBox3.Select(textBox3.Text.Length - 1, 0);
                        textBox3.ScrollToCaret();
                        Thread.Sleep(UNNOMARL_INTERVAL);
                    }
                }
            }
            /*   webBrowser1.Navigate("http://www.baidu.com");
               while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
               {
                   Application.DoEvents();
               }
               HtmlDocument ht = webBrowser1.Document;
               ht.GetElementById("kw").SetAttribute("value", "hellow world");
               ht.GetElementById("su").InvokeMember("click");
               ht.Window.Close();
               */
        }
        Thread th = null;
        public void Start() {
            th = new Thread(new ThreadStart(aotuConnect)); //创建线程  
            th.IsBackground = true;
            th.Start(); //启动线程
            textBox3.Text += System.DateTime.Now + "\r\n------自动联网启动成功------\r\n\r\n";
        }
        public void Stop()
        {
            th.Abort();
            textBox3.Text += System.DateTime.Now+ "\r\n------自动联网停止成功------\r\n\r\n"; 
        }
        private bool IsInternetAvailable()
        {
            try
            {
                Dns.GetHostEntry("www.baidu.com"); //using System.Net;
                return true;
            }
            catch (SocketException )
            {
                return false;
            }
        }
    }
}
