using CodeMachine;
using Newtonsoft.Json;
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

namespace CODING.com.form
{
    public partial class PmassForm : Form
    {
        public PmassForm()
        {
            InitializeComponent();        
        }

        private void PmassForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PmassUtils pmass = new PmassUtils();
            if ("".Equals(textBox1.Text.Trim()))
            {
                MessageBox.Show("补丁编号不能为空！");
            }
            else {
                try
                {
                    pmass.getTar(textBox1.Text.Trim(),"发版");
                    MessageBox.Show("补丁下载成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PmassUtils pmass = new PmassUtils();
            if ("".Equals(textBox1.Text.Trim()))
            {
                MessageBox.Show("补丁编号不能为空！");
            }
            else if ("".Equals(comboBox1.Text.Trim()))
            {
                MessageBox.Show("更新状态不能为空！");
            }
            else {
                try
                {
                    MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("确定更新状态为:"+ comboBox1.Text.Trim() + "吗？", "提示", mess);
                    if (dr == DialogResult.OK)
                    {
                        pmass.updateStat(textBox1.Text.Trim(), comboBox1.Text.Trim());
                        MessageBox.Show("更新状态成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }           
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            //AK/SK
            var API_KEY = "m8f77ha5oIWOSGGywRT0IAc5";
            var SECRET_KEY = "8SbNCP6DVubvVxPfmMLc2GO4wMAXScWN";

            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间


            var image = File.ReadAllBytes(@path);
            //用户向服务请求识别某张图中的所有文字
            var result = client.GeneralBasic(image);        //本地图图片
            //var result = client.GeneralBasicUrl(url);     //网络图片
            //var result = client.Accurate(image);          //本地图片：相对于通用文字识别该产品精度更高，但是识别耗时会稍长。

            //var result = client.General(image);           //本地图片：通用文字识别（含位置信息版）
            //var result = client.GeneralUrl(url);          //网络图片：通用文字识别（含位置信息版）

            //var result = client.GeneralEnhanced(image);   //本地图片：调用通用文字识别（含生僻字版）
            //var result = client.GeneralEnhancedUrl(url);  //网络图片：调用通用文字识别（含生僻字版）

            //var result = client.WebImage(image);          //本地图片:用户向服务请求识别一些背景复杂，特殊字体的文字。
            //var result = client.WebImageUrl(url);         //网络图片:用户向服务请求识别一些背景复杂，特殊字体的文字。
            Word rb = JsonConvert.DeserializeObject<Word>(result.ToString());
            textBox1.Text = "";
            for (int i = 0; i < rb.words_result.Count; i++)
            {
                textBox1.Text = textBox1.Text + rb.words_result[i].words + ";";
            }
            textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Trim().Length - 1);
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            PmassUtils pmass = new PmassUtils();
            if ("".Equals(textBox1.Text.Trim()))
            {
                MessageBox.Show("补丁编号不能为空！");
            }
            else {
                try
                {
                    MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("整理后自动更新状态为待发版吗？", "提示", mess);
                    if (dr == DialogResult.OK)
                    {
                        pmass.readyTc(textBox1.Text.Trim(),true);
                        MessageBox.Show("投产补丁整理成功！");
                    }
                    else if(dr == DialogResult.Cancel)
                    {
                        pmass.readyTc(textBox1.Text.Trim(),false);
                        MessageBox.Show("投产补丁整理成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
