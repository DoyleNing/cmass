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
    public partial class CreateForm : Form
    {
        String[] columnName, dataType, nullAble, comments;
        String entityPath, daoPath, domainPath, iDomainPath, qCmdPath, sCmdPath, jspPath, jsPath;
        BaseDao dao = new BaseDao();
        public CreateForm()
        {
            InitializeComponent();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            String localFile = DataSource.GetSettingString("localFile");
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择导出文件路径";
            if (localFile != null || localFile != "")
            {
                dialog.SelectedPath = localFile;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                fileTextBox.Text = foldPath;
                DataSource.UpdateSettingString("localFile", foldPath);
            }
        }

        private void javaTextBox_Click(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (tableTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入表名!");
                return;
            }
            if (adressTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入功能名称!");
                return;
            }
            if (jspTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入JSP路径!");
                return;
            }
            if (javaTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入JAVA路径!");
                return;
            }
            if (fileTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("请录入导出路径!");
                return;
            }
            String tableName = tableTextBox.Text.Trim();         
            String filePath = javaTextBox.Text.Trim();
            String jsFilePath = jspTextBox.Text.Trim();
            String addressName = adressTextBox.Text.Trim();


            this.CreateEntity(filePath,tableName);
            this.CreateDomain(filePath,addressName,tableName);
            this.CreateCommand(filePath,addressName);
            this.CreateConfig(filePath, addressName);
            this.CreateJs(jsFilePath,addressName);
            this.CreateJsp(jsFilePath,addressName);

        }

        public void CreateEntity(String filePath, String tableName){ 
            entityPath = filePath+".entity."+strAllUpp(tableName);
            String sql = "SELECT UT.COLUMN_NAME COLUMNNAME, UT.DATA_TYPE DATATYPE, UT.NULLABLE, UC.COMMENTS\n" +
                    "  FROM USER_TAB_COLS UT\n" +
                    "  LEFT JOIN USER_COL_COMMENTS UC\n" +
                    "    ON UC.TABLE_NAME = UT.TABLE_NAME\n" +
                    "   AND UC.COLUMN_NAME = UT.COLUMN_NAME\n" +
                    " WHERE UT.TABLE_NAME = '?'\n";
            List<Dictionary<String, Object>> list = dao.executeQuery(sql, new List<Object> { tableName });
            columnName = new String[list.Count()];
            dataType = new String[list.Count()];
            nullAble = new String[list.Count()];
            comments = new String[list.Count()];
            for(int i=0;i<list.Count();i++){
                columnName[i] = list[i]["COLUMNNAME"].ToString();
                dataType[i] = list[i]["DATATYPE"].ToString();
                nullAble[i] = list[i]["NULLABLE"].ToString();
                comments[i] = list[i]["COMMENTS"].GetType() == typeof(System.DBNull)? "默认字段"+i:list[i]["COMMENTS"].ToString().Replace("\"","").Replace("\n","").Replace(" ","").Replace("\t","");
             }
            if(list.Count==0){
                 MessageBox.Show("未找到对应的表字段信息!");
                return;
            }

            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\entity\\";
                String file = fileTextBox.Text.Trim() +"\\CodeMachine生成文件\\"+ filePath.Replace(".", "\\") + "\\entity\\" + strAllUpp(tableName) + ".java";
                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);               
                //创建并打开文件流
                fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("package " + filePath + ".entity;\n" +
                        "\n" +
                        "import org.loushang.next.dao.Column;\n" +
                        "import org.loushang.next.dao.Table;\n" +
                        "import org.loushang.next.dao.Transient;\n" +
                        "import org.loushang.next.data.Rule;\n" +
                        "import org.loushang.next.data.StatefulDatabean;\n" +
                        "import java.math.BigDecimal;\n" +
                        "\n" +
                        "/*\n" +
                        "作者: CODE MACHINE  创建日期:" + System.DateTime.Now + "\n" +
                        "*/\n" +
                        "@Table(tableName = \"" + tableName + "\" , keyFields =\"" + columnName[0] + "\")\n" +
                        "public class " + strAllUpp(tableName) + " extends StatefulDatabean {\n");
                for (int i = 0; i < columnName.Length; i++)
                {
                    //写注释
                    writer.Write("\n" +
                            "\t/*\n\t\t" + comments[i] + "\n\t*/\n");
                    //非空规则
                    if (nullAble[i].Equals("N"))
                    {
                        writer.Write("\t@Rule(value=\"require\")\n");
                    }
                    writer.Write("\t@Column(name = \"" + columnName[i] + "\")\n");
                    //根据类型来定义变量
                    if (dataType[i].Contains("NUMBER") || dataType[i].Contains("FLOAT"))
                    {
                        //数值类型
                        writer.Write("\tprivate BigDecimal " + strUpp(columnName[i]) + ";");
                    }
                    else if (dataType[i].Contains("INT"))
                    {
                        //整数类型
                        writer.Write("\tprivate int " + strUpp(columnName[i]) + ";");
                    }
                    else
                    {
                        //其余皆为String
                        writer.Write("\tprivate String " + strUpp(columnName[i]) + ";");
                    }
                }
                writer.Write("\n\t/*---------------------以下为变量getset方法区域----------------------------*/\n");
                for (int i = 0; i < columnName.Length; i++)
                {
                    //写注释
                    writer.Write("\n" +
                            "\t/*\n\t\t" + comments[i] + "\n\t*/\n");
                    if (dataType[i].Contains("NUMBER") || dataType[i].Contains("FLOAT"))
                    {
                        //set方法
                        writer.Write("\tpublic void set" + strAllUpp(columnName[i]) + "( BigDecimal " + strUpp(columnName[i]) + "){\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                        //写注释
                        writer.Write("\n" +
                                "\t/*\n\t\t" + comments[i] + "\n\t*/\n");
                        //get方法
                        writer.Write("\tpublic BigDecimal get" + strAllUpp(columnName[i]) + "(){\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                    }
                    else if (dataType[i].Contains("INT"))
                    {
                        //set方法
                        writer.Write("\tpublic void set" + strAllUpp(columnName[i]) + "( int " + strUpp(columnName[i]) + "){\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                        //写注释
                        writer.Write("\n" +
                                "\t/*\n\t\t" + comments[i] + "\n\t*/\n");
                        //get方法
                        writer.Write("\tpublic int get" + strAllUpp(columnName[i]) + "(){\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                    }
                    else
                    {
                        //set方法
                        writer.Write("\tpublic void set" + strAllUpp(columnName[i]) + "( String " + strUpp(columnName[i]) + "){\n" +
                                "\t\tthis." + strUpp(columnName[i]) + " = " + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                        //写注释
                        writer.Write("\n" +
                                "\t/*\n\t\t" + comments[i] + "\n\t*/\n");
                        //get方法
                        writer.Write("\tpublic String get" + strAllUpp(columnName[i]) + "(){\n" +
                                "\t\treturn this." + strUpp(columnName[i]) + ";\n" +
                                "\t}");
                    }
                }
                writer.Write("\n}");

                writer.Flush();

            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;
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
        public void CreateDomain(String filePath, String addressName, String tableName)
        {
            daoPath = filePath+".domain."+strAllUpp(addressName)+"Dao";
            domainPath = filePath+".domain."+strAllUpp(addressName)+"Domain";
            iDomainPath = filePath+".domain.I"+strAllUpp(addressName)+"Domain";
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\domain\\";
                String checkFileDao = fileDiction + strAllUpp(addressName) + "Dao.java";
                String checkFileIDomain = fileDiction + "I" + strAllUpp(addressName) + "Domain.java";
                String checkFileDomain = fileDiction + strAllUpp(addressName) + "Domain.java";
               
                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);

                //创建并打开文件流
                fs = new FileStream(checkFileDao, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("package " + filePath + ".domain;\n" +
                    "\n" +
                    "import com.genersoft.cfs.common.dao.TsBaseJdbcDao;\n" +
                    "import com.genersoft.cfs.common.exception.TsException;\n" +
                    "import " + entityPath + ";\n" +
                    "import org.loushang.next.data.DataSet;\n" +
                    "import org.loushang.next.data.ParameterSet;\n" +
                    "import org.loushang.next.data.Record;\n" +
                    "import java.util.ArrayList;\n" +
                    "import java.util.List;" +
                    "\n" +
                    "/**\n" +
                    "\t作者: CodeMachine 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n" +
                    "public class " + strAllUpp(addressName) + "Dao extends TsBaseJdbcDao {\n" +
                    "\tpublic " + strAllUpp(addressName) + "Dao() throws Exception {}\n" +
                    "\tpublic " + strAllUpp(addressName) + "Dao(Class entityClass) throws Exception {\n" +
                    "\t\tsuper(entityClass);\n" +
                    "\t}\n");
                //查询方法
                writer.Write("\tpublic DataSet query(ParameterSet pset){\n" +
                        "\t\tStringBuffer sql = new StringBuffer();\n" +
                        "\t\tList params = new ArrayList();\n" +
                        "\t\tsql.append(\"SELECT * FROM " + tableName + "\");\n" +
                        "\t\t//your coding....\n" +
                        "\t\treturn this.executeDatasetPageExt(sql.toString(),pset.getPageStart(),pset.getPageLimit(),true,params.toArray());\n" +
                        "\t}\n");
                //新增方法
                writer.Write("\tpublic String insert(ParameterSet pset){\n" +
                        "\t\t//your coding....\n" +
                        "\t\treturn \"\";\n\t}\n");
                //保存方法
                writer.Write("\tpublic String save(ParameterSet pset){\n" +
                        "\t\t//your coding....\n" +
                        "\t\treturn \"\";\n\t}\n");
                //删除方法
                writer.Write("\tpublic String delete(ParameterSet pset){\n" +
                        "\t\t//your coding....\n" +
                        "\t\treturn \"\";\n\t}\n");
                writer.Write("\n}");
                writer.Flush();
                writer.Close();
                fs.Close();
                //创建并打开文件流
                fs = new FileStream(checkFileIDomain, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.Write("package " + filePath + ".domain;\n" +
                    "\n" +
                    "import org.loushang.next.data.DataSet;\n" +
                    "import org.loushang.next.data.ParameterSet;\n" +
                    "import org.loushang.sca.transaction.Trans;\n" +
                    "\n" +
                    "/**\n" +
                    "\t作者: Code Machine 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n" +
                    "public interface I" + strAllUpp(addressName) + "Domain {\n" +
                    "\tpublic DataSet query(ParameterSet pset);\n" +
                    "\t@Trans\n" +
                    "\tpublic String insert(ParameterSet pset);\n" +
                    "\t@Trans\n" +
                    "\tpublic String delete(ParameterSet pset);\n" +
                    "\t@Trans\n" +
                    "\tpublic String save(ParameterSet pset);\n");
                writer.Write("}");
                writer.Flush();
                writer.Close();
                fs.Close();

                //创建并打开文件流
                fs = new FileStream(checkFileDomain, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("package " + filePath + ".domain;\n" +
                    "\n" +
                    "import com.genersoft.cfs.common.dao.TsDaoFactory;\n" +
                    "import " + entityPath + ";\n" +
                    "import org.loushang.next.data.DataSet;\n" +
                    "import org.loushang.next.data.ParameterSet;\n" +
                    "\n" +
                    "/**\n" +
                    "\t作者: DoyleNing 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n" +
                    "public class " + strAllUpp(addressName) + "Domain implements I" + strAllUpp(addressName) + "Domain {\n" +
                    "\tprivate " + strAllUpp(addressName) + "Dao dao =(" + strAllUpp(addressName) + "Dao) TsDaoFactory.getDao(\"" + daoPath + "\", " + strAllUpp(tableName) + ".class);\n" +
                    "\tpublic void setdao(" + strAllUpp(addressName) + "Dao dao) {\n" +
                    "\t\tthis.dao = dao;\n" +
                    "\t}\n" +
                    "\tpublic DataSet query(ParameterSet pset){\n" +
                    "\t\treturn dao.query(pset);\n\t}\n" +
                    "\tpublic String insert(ParameterSet pset){\n" +
                    "\t\treturn dao.insert(pset);\n\t}\n" +
                    "\tpublic String delete(ParameterSet pset){\n" +
                    "\t\treturn dao.delete(pset);\n\t}\n" +
                    "\tpublic String save(ParameterSet pset){\n" +
                    "\t\treturn dao.save(pset);\n\t}\n");
                writer.Write("}");
                writer.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;

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
        public void CreateCommand(String filePath, String addressName)
        {
            qCmdPath = filePath+".cmd."+strAllUpp(addressName)+"QueryCmd";
            sCmdPath = filePath+".cmd."+strAllUpp(addressName)+"SaveCmd";
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\cmd\\";
                String checkFileQuery = fileDiction + strAllUpp(addressName) + "QueryCmd.java";
                String checkFileSave = fileDiction + strAllUpp(addressName) + "SaveCmd.java";
                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);

               //创建并打开文件流
                fs = new FileStream(checkFileQuery, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("package " + filePath + ".cmd;\n" +
                    "\n" +
                    "import org.loushang.next.data.DataSet;\n" +
                    "import org.loushang.next.data.Record;\n" +
                    "import org.loushang.next.web.cmd.BaseQueryCommand;\n" +
                    "import org.loushang.sca.ScaComponentFactory;\n" +
                    "import " + iDomainPath + ";\n" +
                    "\n" +
                    "/**\n" +
                    "\t作者: DoyleNing 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n" +
                    "public class " + strAllUpp(addressName) + "QueryCmd extends BaseQueryCommand {\n" +
                    "\tI" + strAllUpp(addressName) + "Domain domain = ScaComponentFactory.getService(I" + strAllUpp(addressName) + "Domain.class, \"" + strAllUpp(addressName) + "Domain/" + strAllUpp(addressName) + "Domain\");\n");
                //查询方法
                writer.Write("\tpublic DataSet execute(){\n" +
                        "\t\t//your coding....\n" +
                        "\t\treturn domain.query(getParameterSet());\n\t}\n");
                writer.Write("\n}");
                writer.Flush();
                writer.Close();
                fs.Close();

                //创建并打开文件流
                fs = new FileStream(checkFileSave, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("package " + filePath + ".cmd;\n" +
                    "\n" +
                    "import org.loushang.next.data.DataSet;\n" +
                    "import org.loushang.next.data.Record;\n" +
                    "import org.loushang.next.data.ParameterSet;\n" +
                    "import org.loushang.next.web.cmd.BaseAjaxCommand;\n" +
                    "import org.loushang.sca.ScaComponentFactory;\n" +
                    "import " + iDomainPath + ";\n" +
                    "\n" +
                    "/**\n" +
                    "\t作者: DoyleNing 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n" +
                    "public class " + strAllUpp(addressName) + "SaveCmd extends BaseAjaxCommand {\n" +
                    "\tI" + strAllUpp(addressName) + "Domain domain = ScaComponentFactory.getService(I" + strAllUpp(addressName) + "Domain.class, \"" + strAllUpp(addressName) + "Domain/" + strAllUpp(addressName) + "Domain\");\n");

                writer.Write("\tpublic void insert(){\n" +
                        "\t\tString result = domain.insert(getParameterSet());\n" +
                        "\t\tthis.setReturn(\"result\", result);\n\t}\n" +

                        "\tpublic void delete(){\n" +
                        "\t\tString result = domain.delete(getParameterSet());\n" +
                        "\t\tthis.setReturn(\"result\", result);\n\t}\n" +

                        "\tpublic void save(){\n" +
                        "\t\tString result = domain.save(getParameterSet());\n" +
                        "\t\tthis.setReturn(\"result\", result);\n\t}\n");
                writer.Write("}");
                writer.Flush();                

            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;


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
        public void CreateConfig(String filePath, String addressName)
        {
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\scaconf\\";
                String checkFile = fileDiction + strAllUpp(addressName) + ".composite";

                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);
                //创建并打开文件流
                fs = new FileStream(checkFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                    "<composite name=\"DicountRateDomain\" xmlns=\"http://www.loushang.com/sca/1.0\">\n" +
                    "    <component name=\"" + strAllUpp(addressName) + "Domain\">\n" +
                    "        <service name=\"" + strAllUpp(addressName) + "Domain\">\n" +
                    "            <interface.java interface=\"" + iDomainPath + "\"/>\n" +
                    "            <binding.sca/>\n" +
                    "        </service>\n" +
                    "        <implementation.java impl=\"" + domainPath + "\"/>\n" +
                    "    </component>\n" +
                    "</composite>");
                writer.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;


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
        public void CreateJs(String filePath, String addressName)
        {
            jsPath = filePath.Replace(".","/")+"/"+strUpp(addressName)+".js";
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\";
                String checkFile = fileDiction + strUpp(addressName) + ".js";

                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);

                //创建并打开文件流
                fs = new FileStream(checkFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.Write("/**\n" +
                    "\t*作者: DoyleNing 创建日期:" + System.DateTime.Now + "\n" +
                    "*/\n");
                writer.Write("function init()\n" +
                        "{\n}\n");
                writer.Write("function doQuery()\n" +
                        "{\n" +
                        "\t//ds.setParameter(\"\",\"\");\n" +
                        "\tds.load();\n" +
                        "}\n");
                writer.Write("function doAdd()\n" +
                        "{\n//do something\n}\n");
                writer.Write("function doSave()\n" +
                        "{\n//do something\n}\n");
                writer.Write("function doEdit()\n" +
                        "{\n//do something\n}\n");
                writer.Write("function doCancel()\n" +
                        "{\n//do something\n}\n");
                writer.Write("function doSubmit()\n" +
                        "{\n//do something\n}\n");
                writer.Write("function doReset() {\n" +
                        "\tqueryForm.reset();\n}\n");
                writer.Write("function getParam(ElementId) {\n" +
                        "\tvar value = document.getElementById(ElementId).value;\n" +
                        "\tif (value == \"\") value = undefined;\n" +
                        "\treturn value;\n" +
                        "}\n");
                writer.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;


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
        public void CreateJsp(String filePath, String addressName) 
        {
            jspPath = filePath.Replace(".","/")+strUpp(addressName)+".jsp";
            StreamWriter writer = null;
            FileStream fs = null;
            try
            {
                String fileDiction = fileTextBox.Text.Trim() + "\\CodeMachine生成文件\\" + filePath.Replace(".", "\\") + "\\";
                String checkFile = fileDiction + strUpp(addressName) + ".jsp";


                if (!Directory.Exists(@fileDiction))//检查文件夹
                    Directory.CreateDirectory(@fileDiction);
                //创建并打开文件流
                fs = new FileStream(checkFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                //创建写入流
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

                writer.Write("<%--*作者: DoyleNing 创建日期:" +System.DateTime.Now + "--%>\n");
                writer.Write("<%@ page contentType=\"text/html; charset=UTF-8\" %>\n" +
                        "<%@ taglib uri=\"/tags/next-web\" prefix=\"next\" %>\n" +
                        "<%@ taglib uri=\"/tags/next-model\" prefix=\"model\" %>\n" +
                        "<%@ page import=\"org.loushang.next.skin.SkinUtils\" %>\n" +
                        "<%@ page import=\"org.loushang.bsp.security.context.GetBspInfo\" %>\n" +
                        "<%@ page import=\"com.genersoft.netbank.common.NetBankConst\" %>\n" +
                        "<%@ page import=\"com.genersoft.netbank.common.UserInfo\" %>\n" +
                        "<%@ page import=\"com.genersoft.cfs.common.dao.GetSysConf\" %>\n" +
                        "<%String userId = GetBspInfo.getBspInfo().getUserId();\n" +
                        "  UserInfo userInfo = (UserInfo) request.getSession().getAttribute(NetBankConst.LOGIN_USER_INFO);\n" +
                        "  String corpCode = userInfo.getCorpCode();  //经销商编号\n" +
                        "  String userType = userInfo.getUserType();  //用户类型   2代表网银\n" +
                        "  String corpName = userInfo.getCorpName(); //经销商名称\n" +
                        "%>\n");
                writer.Write("<html>\n" +
                        "<head>\n" +
                        "  <title>请修改为自己的title</title>\n" +
                        "  <next:ScriptManager/>\n" +
                        "  <script type=\"text/javascript\">\n" +
                        "    var userType = \"<%=userType%>\";\n" +
                        "    var accountDate = \"<%=GetSysConf.getActDate(userInfo.getUserJG())%>\";\n" +
                        "  </script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=request.getContextPath()%>/" + jsPath + "\"></script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=request.getContextPath()%>/jsp/cfs/pubHelps/pubHelps.js\"></script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=request.getContextPath()%>/jsp/cfs/public/js/pubValidate.js\"></script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=request.getContextPath()%>/jsp/cfs/public/js/tsPubWf.js\"></script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=SkinUtils.getJS(request, \"bpm/bpm.js\")%>\"></script>\n" +
                        "  <script type=\"text/javascript\" src=\"<%=request.getContextPath()%>/jsp/cfs/public/jsp/excel/exportOriginalExcel.js\"></script>\n" +
                        "</head>\n");
                writer.Write("<model:datasets>\n" +
                        "  <model:dataset id=\"ds\" cmd=\"" + qCmdPath + "\" global=\"true\">\n" +
                        "    <model:record fromBean=\"" + entityPath + "\"></model:record>\n" +
                        "  </model:dataset>\n" +
                        "</model:datasets>\n");
                writer.Write("<body>\n" +
                        "<next:Panel title=\"请修改title\" width=\"100%\">\n" +
                        "  <next:Html>\n" +
                        "    <fieldset style=\"overflow: visible;width:100%\" class=\"GroupBox\">\n" +
                        "        <legend\n" +
                        "            class=\"GroupBoxTitle\">查询条件 <img\n" +
                        "            class=\"GroupBoxExpandButton\"\n" +
                        "            src=\"<%=SkinUtils.getImage(request,\"groupbox_collapse.gif\")%>\"\n" +
                        "            onclick=\"collapse(this)\" /> </legend>\n" +
                        "      <div>\n" +
                        "        <form id=\"queryForm\" class=\"L5form\" style=\"width:100%\">\n" +
                        "          <table border=\"1\"  width=\"100%\">\n" +
                        "            <tr>\n" +
                        "                <td class=\"FieldLabel\" >查询条件一</td>\n" +
                        "                <td class=\"FieldInput\">\n" +
                        "                <input  class=\"TextEditor\" id=\"id1\"  title=\"查询条件一\" >\n" +
                        "                  <img src=\"<%=SkinUtils.getImage(request,\"l5/liulan.gif\")%>\" style=\"cursor:pointer\"\n" +
                        "                   onclick=\"func1()\"/>\n" +
                        "                </td>\n" +
                        "                <td class=\"FieldLabel\" >查询条件二</td>\n" +
                        "                <td class=\"FieldInput\">\n" +
                        "                <input  class=\"TextEditor\" id=\"id2\"  title=\"查询条件二\" >\n" +
                        "                </td>\n" +
                        "              <td class=\"FieldLabael\" align=\"right\" ><button  onclick=\"doQuery()\">查 询</button></td>\n" +
                        "              <td class=\"FieldLabael\" align=\"left\" ><button  onclick=\"doReset()\">重 置</button></td>\n" +
                        "            </tr>\n" +
                        "          </table>\n" +
                        "        </form>\n" +
                        "      </div>\n" +
                        "    </fieldset>\n" +
                        "  </next:Html>\n" +
                        "</next:Panel>\n");
                writer.Write("<next:GridPanel id=\"GridPanel\" width=\"100%\" stripeRows=\"true\" height=\"100%\" dataset=\"ds\" title=\"请修改\" notSelectFirstRow=\"true\" >\n" +
                        "  <next:TopBar>\n" +
                        "    <next:ToolBarItem text=\"新增\" id=\"add\" iconCls=\"add\" handler=\"doAdd\" />\n" +
                        "    <next:ToolBarItem text=\"保存\" id=\"save\" iconCls=\"save\" handler=\"doSave\" />\n" +
                        "    <next:ToolBarItem text=\"调整\" id=\"edit\" iconCls=\"edit\" handler=\"doEdit\" />\n" +
                        "    <next:ToolBarItem text=\"作废\" id=\"cancel\" iconCls=\"remove\" handler=\"doCancel\" />\n" +
                        "    <next:ToolBarItem text=\"提交\" id=\"submit\" iconCls=\"submit\" handler=\"doSubmit\" />\n" +
                        "  </next:TopBar>\n" +
                        "  <next:Columns>\n" +
                        "    <next:RowNumberColumn width=\"30\"/>\n" +
                        "    <next:CheckBoxColumn></next:CheckBoxColumn>\n");
                for (int i = 0; i < columnName.Length; i++)
                {
                    String comment = "默认字段";
                    if (columnName[i]!=null&& columnName[i].GetType()!= typeof(System.DBNull)&& columnName[i]!="")
                    {
                        comment = strUpp(columnName[i]);
                    }
                    writer.Write("    <next:Column id=\"" + comment + "\" header=\"" + comments[i] + "\" field=\"" + strUpp(columnName[i]) + "\" width=\"90\">\n" +
                            "    </next:Column>\n");
                }
                writer.Write("  </next:Columns>\n" +
                        "  <next:BottomBar>\n" +
                        "    <next:PagingToolBar dataset=\"ds\"/>\n" +
                        "  </next:BottomBar>\n" +
                        "</next:GridPanel>\n" +
                        "<iframe style=\"display: none\" id=\"export_iframe\"></iframe>\n" +
                        "</body>\n" +
                        "</html>");


                writer.Flush();

                MessageBox.Show("导出成功!");

            }
            catch (Exception e)
            {
                throw new Exception("导出文件失败\n" + e.Message); ;

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


        //更改为驼峰命名
        public static String strUpp(String s)
        {
            String[] str = s.ToLower().Split('_');
            String sb = str[0];
            for (int i = 1; i < str.Length; i++)
            {
                sb = sb + str[i].Substring(0,1).ToString().ToUpper() + str[i].Substring(1);
            }
            return sb;
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

    }
}
