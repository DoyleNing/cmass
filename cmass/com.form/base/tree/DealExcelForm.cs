using CODING.com;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
    public partial class DealExcelForm : Form
    {
        public DealExcelForm()
        {
            InitializeComponent();
        }
        private void MetroButton1_Click(object sender, EventArgs e)
        {
            //打开一个文件选择框
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Excel文件",
                FileName = "",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),//为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录
                Filter = "Excel文件| *.xlsx;*.xls",
                ValidateNames = true,     //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
                CheckFileExists = true,  //验证路径有效性
                CheckPathExists = true //验证文件有效性
            };
            string strName = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strName = ofd.FileName;
            }
            metroTextBox1.Text = strName;
        }

        private void MetroButton2_Click(object sender, EventArgs e)
        {
            String filePath = metroTextBox1.Text.Trim();
            if ("".Equals(metroTextBox1.Text.Trim()))
            {
                MessageBox.Show("EXCEL路径不能为空");
            }
            else
            {
                ImportExcel(filePath);
            }
        }
        public void ImportExcel(string filePath) {
            DataSet ds = new DataSet();
            List<StudentInfo> list = new List<StudentInfo>();
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string extension = System.IO.Path.GetExtension(filePath);//获取扩展名
                IWorkbook workbook1 = null;
                if (extension.Equals(".xls")) //2003
                {
                    workbook1 = new HSSFWorkbook(fs);
                }
                else                         //2007以上
                {
                    workbook1 = new XSSFWorkbook(fs);
                }
                ISheet sheet = workbook1.GetSheetAt(0);

                //获取行数
                int rowCount = sheet.LastRowNum;

                //获取列名
                for (int i = 4; i <= sheet.LastRowNum; i++)
                {
                    StudentInfo student = new StudentInfo();
                    if (sheet.GetRow(i).GetCell(1).ToString() == "")
                        continue;
                    student.ClassNo = sheet.GetRow(i).GetCell(0).ToString();
                    student.Name = sheet.GetRow(i).GetCell(1).ToString();
                    student.Grade = Convert.ToInt32(sheet.GetRow(i).GetCell(2).ToString());
                    student.AGrade = sheet.GetRow(i).GetCell(2).ToString();
                    student.Uuid = System.Guid.NewGuid().ToString("N");
                    list.Add(student);
                 }
                list.Sort();
                sheet = null;
                workbook1 = null;

                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Excel导入失败:"+ex.Message);
            }
            //区分网班非网班
            string netClassNo = DataSource.GetSettingString("NET_CLASS");
            List<StudentInfo> netList = new List<StudentInfo>();
            List<StudentInfo> normalList = new List<StudentInfo>();
            string netNos = "";
            string normalNos = "";
            for (int i=0;i<list.Count;i++) {
                StudentInfo s = list[i];
                if (netClassNo.Contains(s.ClassNo.Substring(6,2))) {
                    netList.Add(s);
                    if (!netNos.Contains(s.ClassNo.Substring(6, 2))) {
                        netNos += s.ClassNo.Substring(6, 2)+";";
                    }
                }
                else {
                    normalList.Add(s);
                    if (!normalNos.Contains(s.ClassNo.Substring(6, 2)))
                    {
                        normalNos += s.ClassNo.Substring(6, 2) + ";";
                    }
                }
            }
            netList.Sort();
            normalList.Sort();

            //优胜榜        
            List<StudentInfo> netListA = new List<StudentInfo>();
            List<StudentInfo> normalListA = new List<StudentInfo>();

            for (int i = 0; i < 100; i++) {
                netListA.Add(netList[i]);
             }
            netListA.Sort();
            for (int i = 0; i < 200; i++)
            {
                normalListA.Add(normalList[i]);
            }
            normalListA.Sort();

            int[] net = ToIntArray(netNos.Substring(0,netNos.Length-1).Split(';'));
            int[] normal = ToIntArray(normalNos.Substring(0, normalNos.Length - 1).Split(';'));
            Array.Sort(net);
            Array.Sort(normal);

            //平均分
            List<string> keys = new List<string>();
            List<Dictionary<string, double>> average = new List<Dictionary<string, double>>();
            //成绩        
            List<StudentInfo> netGradeList = new List<StudentInfo>();
            List<StudentInfo> normalGradeList = new List<StudentInfo>();
            //网班填充数据后成绩
            for (int i=0;i<net.Length;i++) {
                List<StudentInfo> c = new List<StudentInfo>();
                for (int j=0;j<netList.Count;j++) {
                    if (Convert.ToInt32(netList[j].ClassNo.Substring(6,2))==net[i]) {
                        StudentInfo s = netList[j];
                        if (j<100) {
                            s.AGrade = "A";
                        }
                        c.Add(s);
                    }
                }
                //添加每个班的平均分
                Dictionary<string, double> d = new Dictionary<string, double>();
                d.Add(c[0].ClassNo.Substring(6, 2), c.Average(t => t.Grade));
                average.Add(d);
                keys.Add(c[0].ClassNo.Substring(6, 2));

                netGradeList = netGradeList.Union(c).ToList<StudentInfo>();
            }
            //非网班填充数据后成绩
            for (int i = 0; i < normal.Length; i++)
            {
                List<StudentInfo> c = new List<StudentInfo>();
                for (int j = 0; j < normalList.Count; j++)
                {
                    if (Convert.ToInt32(normalList[j].ClassNo.Substring(6, 2)) == normal[i])
                    {
                        StudentInfo s = normalList[j];
                        if (j < 200)
                        {
                            s.AGrade = "A";
                        }
                        c.Add(s);
                    }
                }
                //添加每个班的平均分
                Dictionary<string, double> d = new Dictionary<string, double>();
                d.Add(c[0].ClassNo.Substring(6, 2), c.Average(t => t.Grade));
                average.Add(d);
                keys.Add(c[0].ClassNo.Substring(6, 2));

                normalGradeList = normalGradeList.Union(c).ToList<StudentInfo>();
            }


            HSSFWorkbook workbook = new HSSFWorkbook();

            //创建工作表
            var normalAsheet = workbook.CreateSheet("非网班成绩");
            //创建标题行（重点） 从0行开始写入
            var normalArow = normalAsheet.CreateRow(0);
            //创建单元格
            normalArow.CreateCell(0).SetCellValue("班级");
            normalArow.CreateCell(1).SetCellValue("姓名");
            normalArow.CreateCell(2).SetCellValue("得分");
            //遍历集合，生成行
            for (int i = 0; i < normalGradeList.Count; i++)
            {
                var rowi = normalAsheet.CreateRow(i+1);           
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(normalGradeList[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(normalGradeList[i].Name);
                var grade = rowi.CreateCell(2);
                grade.SetCellValue(normalGradeList[i].AGrade);
            }

            //创建工作表
            var normalA = workbook.CreateSheet("非网班优胜榜");
            //创建标题行（重点） 从0行开始写入
            var rowNor = normalA.CreateRow(0);
            //创建单元格
            rowNor.CreateCell(0).SetCellValue("班级");
            rowNor.CreateCell(1).SetCellValue("姓名");
            //遍历集合，生成行
            for (int i = 0; i < normalListA.Count; i++)
            {
                var rowi = normalA.CreateRow(i + 1);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(normalListA[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(normalListA[i].Name);
            }

            //创建工作表
            var netAsheet = workbook.CreateSheet("网班成绩");
            //创建标题行（重点） 从0行开始写入
            var netArow = netAsheet.CreateRow(0);
            //创建单元格
            netArow.CreateCell(0).SetCellValue("班级");
            netArow.CreateCell(1).SetCellValue("姓名");
            netArow.CreateCell(2).SetCellValue("得分");
            //遍历集合，生成行
            for (int i = 0; i < netGradeList.Count; i++)
            {
                var rowi = netAsheet.CreateRow(i + 1);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(netGradeList[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(netGradeList[i].Name);
                var grade = rowi.CreateCell(2);
                grade.SetCellValue(netGradeList[i].AGrade);
            }

            //创建工作表
            var netA = workbook.CreateSheet("网班优胜榜");
            //创建标题行（重点） 从0行开始写入
            var rowNet = netA.CreateRow(0);
            //创建单元格
            rowNet.CreateCell(0).SetCellValue("班级");
            rowNet.CreateCell(1).SetCellValue("姓名");
            //遍历集合，生成行
            for (int i = 0; i < netListA.Count; i++)
            {
                var rowi = netA.CreateRow(i + 1);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(netListA[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(netListA[i].Name);
            }

            //创建工作表
            var avg = workbook.CreateSheet("平均分");
            //创建标题行（重点） 从0行开始写入
            var avg0 = avg.CreateRow(0);
            //创建单元格
            avg0.CreateCell(0).SetCellValue("全年级平均分");
            avg0.CreateCell(1).SetCellValue(list.Average(t => t.Grade));
            var avg1 = avg.CreateRow(1);
            //创建单元格
            avg1.CreateCell(0).SetCellValue("网班平均分");
            avg1.CreateCell(1).SetCellValue(netList.Average(t => t.Grade));
            var avg2 = avg.CreateRow(2);
            //创建单元格
            avg2.CreateCell(0).SetCellValue("非网班平均分");
            avg2.CreateCell(1).SetCellValue(normalList.Average(t => t.Grade));

            //遍历集合，生成行
            for (int i = 0; i < keys.Count; i++)
            {
                var rowi = avg.CreateRow(i + 3);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(keys[i]);
                var averg = rowi.CreateCell(1);
                averg.SetCellValue(average[i][keys[i]]);
            }

            //创建工作表
            var netsheet = workbook.CreateSheet("原始数据成绩单（网班）");
            //创建标题行（重点） 从0行开始写入
            var netrow = netsheet.CreateRow(0);
            //创建单元格
            netrow.CreateCell(0).SetCellValue("班级");
            netrow.CreateCell(1).SetCellValue("姓名");
            netrow.CreateCell(2).SetCellValue("得分");
            //遍历集合，生成行
            for (int i = 0; i < netGradeList.Count; i++)
            {
                var rowi = netsheet.CreateRow(i + 1);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(netGradeList[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(netGradeList[i].Name);
                var grade = rowi.CreateCell(2);
                grade.SetCellValue(netGradeList[i].Grade);
            }



            //创建工作表
            var normalsheet = workbook.CreateSheet("原始数据成绩单（非网班）");
            //创建标题行（重点） 从0行开始写入
            var normalrow = normalsheet.CreateRow(0);
            //创建单元格
            normalrow.CreateCell(0).SetCellValue("班级");
            normalrow.CreateCell(1).SetCellValue("姓名");
            normalrow.CreateCell(2).SetCellValue("得分");
            //遍历集合，生成行
            for (int i = 0; i < normalGradeList.Count; i++)
            {
                var rowi = normalsheet.CreateRow(i + 1);
                var classNo = rowi.CreateCell(0);
                classNo.SetCellValue(normalGradeList[i].ClassNo);
                var name = rowi.CreateCell(1);
                name.SetCellValue(normalGradeList[i].Name);
                var grade = rowi.CreateCell(2);
                grade.SetCellValue(normalGradeList[i].Grade);
            }


            String address = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileStream file = new FileStream(@address+"\\"+ System.Guid.NewGuid().ToString("N") + "统计表.xls", FileMode.CreateNew, FileAccess.Write);
            workbook.Write(file);
            file.Dispose();
            MessageBox.Show("数据已生成!");
        }
        public static int[] ToIntArray(string[] Content)
        {
            int[] c = new int[Content.Length];
            for (int i = 0; i < Content.Length; i++)
            {
                c[i] = Convert.ToInt32(Content[i].ToString());
            }
            return c;
        }
    }

}
