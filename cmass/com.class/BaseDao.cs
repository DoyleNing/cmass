using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace CodeMachine
{
    class BaseDao
    {
        public List<Dictionary<String, Object>> executeQuery(String sql,List<Object> list){
            return excution(getSQL(sql,list));
        }
        //自带数据源访问
        public List<Dictionary<String, Object>> executeQuery(String sql, List<Object> list,String dataSource)
        {
            return excution(getSQL(sql, list),dataSource);
        }
        //执行
        private List<Dictionary<String, Object>> excution(String sql) {
            //创建一个新连接
            OracleConnection conn = new OracleConnection(DataSource.GetSettingString("connString"));
            //创建list接受返回值
            List<Dictionary<String, Object>> list = new List<Dictionary<string, object>>();
            try
            {

                Console.WriteLine(sql);

                conn.Open();//打开指定的连接 

                OracleCommand com = conn.CreateCommand();//创建请求

                com.CommandText = sql;//装载Sql语句 

                OracleDataReader odr = com.ExecuteReader();
                while (odr.Read())//读取数据，如果返回为false的话，就说明到记录集的尾部了 
                {
                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                    for (int i = 0; i < odr.FieldCount; i++)
                    {
                        dictionary.Add(odr.GetName(i),odr.GetValue(i));
                    }
                    list.Add(dictionary);
                 }
                odr.Close();//关闭reader.这是一定要写的 
                
            }
            catch (Exception ee)
            {
                throw new Exception("数据库链接失败\n"+ee.Message);
            }
            finally
            {
                conn.Close(); //关闭连接
            }
            return list;
        }

        //带数据源的执行
        private List<Dictionary<String, Object>> excution(String sql,String dataSource)
        {
            //创建一个新连接
            OracleConnection conn = new OracleConnection(dataSource);
            //创建list接受返回值
            List<Dictionary<String, Object>> list = new List<Dictionary<string, object>>();
            try
            {

                Console.WriteLine(sql);

                conn.Open();//打开指定的连接 

                OracleCommand com = conn.CreateCommand();//创建请求

                com.CommandText = sql;//装载Sql语句 

                OracleDataReader odr = com.ExecuteReader();
                while (odr.Read())//读取数据，如果返回为false的话，就说明到记录集的尾部了 
                {
                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                    for (int i = 0; i < odr.FieldCount; i++)
                    {
                        dictionary.Add(odr.GetName(i), odr.GetValue(i));
                    }
                    list.Add(dictionary);
                }
                odr.Close();//关闭reader.这是一定要写的 

            }
            catch (Exception ee)
            {
                throw new Exception("数据库链接失败\n" + ee.Message);
            }
            finally
            {
                conn.Close(); //关闭连接
            }
            return list;
        }




        //组装sql
        private String getSQL(String sql,List<Object> list) {
            String returnSql = "";
            String[] sqlDep = sql.Split('?');
            for(int i=0;i<sqlDep.Length;i++){
                returnSql += sqlDep[i];
                if (i< list.Count){
                        returnSql += list[i];
                }
            }
            return returnSql;
        }
    }
}
