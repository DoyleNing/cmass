using CodeMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODING.com
{
    class PmassUtils
    {
        static Dictionary<string, string> map = new Dictionary<string, string>();
        static PmassUtils(){
            map.Clear();
            map.Add("登记","01");
            map.Add("已发测试环境","02");
            map.Add("验证完毕","03");
            map.Add("已投产","04");
            map.Add("待发版","05");
            map.Add("作废", "06");
        }
        public String getTar(String bh,String name) {
            bh = bh.ToUpper();
            String deskTop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\"+ name + "\\";
            String gm = deskTop + "gm\\";
            String gmsql = deskTop + "gmsql\\";
            String wy = deskTop + "wy\\";
            String wysql = deskTop + "wysql\\";
            String dd = deskTop + "调度\\";
            String bb = deskTop + "报表\\";

            String[] code = bh.Split(';');
            String findCode = "";
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i].Trim()!="") {
                    findCode += "'" + code[i] + "',";
                }
            }
            findCode = findCode.Substring(0, findCode.Length - 1);
            String dataSource = DataSource.GetSettingString("pmassConnString");
            String sql = "SELECT PR.PATCH_CODE, PR.PATCH_DISC, PM.FILEBODY,PM.FILE_NAME,PR.PATCH_DEVER  " +
                "FROM PM_ATTACH_FILES PM INNER JOIN PM_PATCH_REG PR ON PM.BUS_ID = PR.PATCH_ID " +
                "WHERE PR.PATCH_CODE IN(?)";
            List<Dictionary<String, Object>> list = new BaseDao().executeQuery(sql, new List<Object> { findCode }, dataSource);

            for (int i = 0; i < list.Count; i++)
            {
                String PATCH_CODE = list[i]["PATCH_CODE"].ToString();
                String PATCH_DISC = list[i]["PATCH_DISC"].ToString();
                String FILE_NAME = list[i]["FILE_NAME"].ToString();
                String PATCH_DEVER = list[i]["PATCH_DEVER"].ToString();
                byte[] buff = (byte[])list[i]["FILEBODY"];
                //第一个补丁创建文件夹时删除以前的
                if (i == 0)
                {
                    if (!Directory.Exists(@deskTop))//检查文件夹
                    {
                        Directory.CreateDirectory(@deskTop);
                    }
                    else
                    {
                        Directory.Delete(@deskTop, true);
                        Directory.CreateDirectory(@deskTop);
                    }
                }
                if (PATCH_DISC.Contains("【柜面】"))
                {
                    if (!Directory.Exists(@gm))//检查文件夹
                    {
                        Directory.CreateDirectory(@gm);
                    }                   
                    if (FILE_NAME.Contains(".tar"))
                    {
                        String savepath = gm + PATCH_CODE + "_" + FILE_NAME;
                        FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }
                    else
                    {
                        if (!Directory.Exists(@gmsql))//检查文件夹
                        {
                            Directory.CreateDirectory(@gmsql);
                        }
                        String savepath = gmsql + PATCH_CODE + "_" + FILE_NAME;
                        FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }
                }
                else if (PATCH_DISC.Contains("【网银】"))
                {
                    if (!Directory.Exists(@wy))//检查文件夹
                    {
                        Directory.CreateDirectory(@wy);
                    }
                    if (FILE_NAME.Contains(".tar"))
                    {
                        String savepath = wy + PATCH_CODE + "_" + FILE_NAME;
                        FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }
                    else
                    {
                        if (!Directory.Exists(@wysql))//检查文件夹
                        {
                            Directory.CreateDirectory(@wysql);
                        }
                        String savepath = wysql + PATCH_CODE + "_" + FILE_NAME;
                        FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }
                }
                else if (PATCH_DISC.Contains("【报表】"))
                {
                    if (!Directory.Exists(@bb))//检查文件夹
                    {
                        Directory.CreateDirectory(@bb);
                    }
                    String savepath = bb + PATCH_CODE + "_" + FILE_NAME;
                    FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(buff, 0, buff.Length);
                    bw.Close();
                    fs.Close();
                }
                else if (PATCH_DISC.Contains("【调度】"))
                {
                    if (!Directory.Exists(@dd))//检查文件夹
                    {
                        Directory.CreateDirectory(@dd);
                    }
                    String savepath = dd + PATCH_CODE + "_" + FILE_NAME;
                    FileStream fs = new FileStream(savepath, FileMode.CreateNew);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(buff, 0, buff.Length);
                    bw.Close();
                    fs.Close();
                }
                else
                {
                    throw new Exception(PATCH_DEVER+"的补丁：" + PATCH_CODE + "描述不规范，下载失败！");
                }
            }
            return deskTop;
        }

        public void updateStat(String bh,String stat) {
            bh = bh.ToUpper();
            String[] code = bh.Split(';');
            String findCode = "";
            for (int i = 0; i < code.Length; i++)
            {
                findCode += "'" + code[i] + "',";
            }
            findCode = findCode.Substring(0, findCode.Length - 1);
            String dataSource = DataSource.GetSettingString("pmassConnString");
            String sql = " UPDATE PM_PATCH_REG PR SET PR.STAT = '?' WHERE PR.PATCH_CODE IN (?)";
            new BaseDao().executeQuery(sql, new List<Object> { map[stat].ToString(), findCode }, dataSource);           
        }

        public void readyTc(String bh,Boolean isUpt) {
            bh = bh.ToUpper();
            //获取tar包
            String name = DateTime.Now.ToString("yyyyMMdd")+"投产补丁";
            String path = getTar(bh,name);
            //生成脚本
            if (Directory.Exists(@path+"wy"))//检查文件夹
            {
                createSh(path+"wy\\");
            }
            if (Directory.Exists(@path + "wysql"))//检查文件夹
            {
                sortSql(path + "wysql\\");
            }
            if (Directory.Exists(@path + "gm"))//检查文件夹
            {
                createSh(path+"gm\\");
            }
            if (Directory.Exists(@path + "gmsql"))//检查文件夹
            {
                sortSql(path + "gmsql\\");
            }
            if (Directory.Exists(@path + "报表"))//检查文件夹
            {
                createSh(path+"报表\\");
            }
            if (Directory.Exists(@path + "调度"))//检查文件夹
            {
                sortSql(path + "调度\\");
            }
            //导出excel
            //更新状态
            if (isUpt) {
                updateStat(bh, "待发版");
            }
        }

        public  void createSh(String path)
        {
            String newFileName = path+"tarxvf.sh";          
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                List<String> ls = new List<String>();

                for (int i = 0; i <files.Length; i++)
                {
                    String name = files[i].Name;
                    if (files[i].Length < 1)
                    {
                        throw new IOException(name + "是空的，请核对");
                    }
                    ls.Add(name);
                }
                String[] arr = new String[ls.Count()];
                arr = ls.ToArray();
                sort(arr);

                //创建并打开文件流
                fs = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                foreach (String s in arr)
                {
                    writer.Write("tar -xvf " + s + " -C ../;\r\n");
                }
                writer.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("整理补丁失败\n" + e.Message);
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

        public void sortSql(String path)
        {
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                List<String> ls = new List<String>();

                for (int i = 0; i < files.Length; i++)
                {
                    String name = files[i].Name;
                    if (files[i].Length < 1)
                    {
                        throw new IOException(name + "是空的，请核对");
                    }
                    ls.Add(name);
                }
                String[] arr = new String[ls.Count()];
                arr = ls.ToArray();
                sort(arr);
                for (int i=0;i<arr.Length;i++){
                    File.Move(path+arr[i], path+(i+1)+"_"+arr[i]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("整理补丁失败\n" + e.Message);
            }
        }


        public static void sort(String[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    String[] b1 = a[j].Split('_');
                    String[] b2 = a[j + 1].Split('_');

                    String c1 = b1[0].Substring(b1[0].IndexOf("2"));
                    String c2 = b2[0].Substring(b2[0].IndexOf("2"));
                    double d1 = Convert.ToDouble(c1.Trim());
                    double d2 = Convert.ToDouble(c2.Trim());
                    if (d1 > d2)
                    {
                        String tmp = null;
                        tmp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = tmp;
                    }
                }
            }
        }
    }
}
