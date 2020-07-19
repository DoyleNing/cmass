using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CODING.com.form
{
    public partial class InterfaceTestForm : Form
    {
        public InterfaceTestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* String src = "customNo=w23e3242e323234234w23e3242e323234234&custName=张阿斯顿发三";
            String key1 = "6y!jS4rPD71eDBGZcGzY8sS9lu9V%*J4";
            MessageBox.Show( HttpEncryUtil.encrypt(src,key1));
            MessageBox.Show(HttpEncryUtil.decryptThreeDESECB( HttpEncryUtil.encrypt(src, key1),key1));
            return;
            */
             if ("".Equals(textBox1.Text.Trim()))
             {
                 MessageBox.Show("测试地址不能为空!");
                 return;
             }
             else{
                String url = textBox1.Text.Trim();
                int end = url.IndexOf("?");
                String serviceUrl = url.Split('?')[0];
                //有？号后面的参数
                String[] paramS = null;
                if (url.Split('?').Length > 1)
                {
                    paramS = url.Split('?')[1].Split('&');
                }
                else {
                    paramS = new String[0];
                }                              
                Hashtable map = new Hashtable();
                for (int i = 0; i < paramS.Length; i++)
                {
                    String key = paramS[i].Split('=')[0];
                    if (!"paramsMap".Equals(key))
                    {
                        String value = paramS[i].Split('=')[1];
                        map.Add(key,value);
                    }
                    else
                    {
                        String value = paramS[i].Split('=')[1];
                        String mapKey = value.Split('{')[0];
                        String mapValue = value.Split('{')[1];
                        Hashtable tempMap = getParamsMap(mapValue);
                        map.Add(mapKey, tempMap);
                    }
                }
                String reslut = null;
                try
                {
                    reslut = HttpEncryUtil.postEncryJson(serviceUrl, map, "UTF-8", 300, 300);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("接口测试失败:\n" + ee.Message);
                }
                textBox2.Text = reslut;
            }
               
        }

        public Hashtable getParamsMap(String paramsString)
        {
            Hashtable returnMap = new Hashtable();
            String[] array = paramsString.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                int binary = array[i].IndexOf(":");
                String key = array[i].Substring(0, binary);
                String value = array[i].Substring(binary + 1, array[i].Length);
                returnMap.Add(key, value);
            }
            return returnMap;
        }
    }
}
