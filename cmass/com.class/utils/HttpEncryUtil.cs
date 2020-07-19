using CodeMachine;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CODING.com
{
    static class  HttpEncryUtil
    {
        private  static String rest_message_encrypt_key = "REST_MESSAGE_ENCRYPT_KEY";
        private  static String rest_message_encrypt_switch = "REST_MESSAGE_ENCRYPT_SWITCH";
        public static String postEncryJson(String url, Hashtable param, String charset, int timeOut, int readTimeOut) 
        {
            return postEncryJson(url, JsonConvert.SerializeObject(param), charset,timeOut,readTimeOut);
        }


        public static String postEncryJson(String url, String content, String charset, int timeOut, int readTimeOut)
        {
            if ("".Equals(url) || url.GetType() == typeof(DBNull))
            {
                return null;
            }

            System.GC.Collect();//垃圾回收，回收没有正常关闭的http链接
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            string result = "";

            try
            {
                //设置最大链接数
                ServicePointManager.DefaultConnectionLimit = 200;
                request = (HttpWebRequest)WebRequest.Create(url);
                //设置请求头
                setHeader(request);

                //HttpWebRequest 相关属性
                request.Method = "POST";
                request.ContentType = "application/json;charset=" + charset;
                request.KeepAlive = true;

                request.Timeout= timeOut * 1000;
                request.ContinueTimeout = readTimeOut*1000;
                request.ReadWriteTimeout = readTimeOut*1000;
                

                // 得到请求的输出流对象
                if (!"".Equals(content)&&content.GetType()!=typeof(DBNull))
                {
                    //如果要加密，那么json字符串都需要加密
                    if (isEncrypt())
                    {
                        content = encrypt(content, getRestMessageEncryptKey());
                    }
                }
                
                byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                request.ContentLength = data.Length;

                //写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //返回数据
                response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();

                //如果需要加密，那么返回值也需要解密
                if (isEncrypt())
                {
                    result = decryptThreeDESECB(result, getRestMessageEncryptKey());
                }
                
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("http请求失败:\n" + e.Message);
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
         }
        private static void setHeader(HttpWebRequest request)
        {
           if (request != null){
                String sys = DataSource.GetSettingString("INTER_SYS_ID") == null ? "" : DataSource.GetSettingString("INTER_SYS_ID");
                String userId = DataSource.GetSettingString("INTER_USER_ID") == null ? "" : DataSource.GetSettingString("INTER_USER_ID");
                String password = DataSource.GetSettingString("INTER_USER_PASS") == null ? "" : DataSource.GetSettingString("INTER_USER_PASS");
            if (isEncrypt()){
                sys=encrypt(sys, getRestMessageEncryptKey());
                userId=encrypt(userId, getRestMessageEncryptKey());
                password=encrypt(password, getRestMessageEncryptKey());
            }
                request.Headers.Set("system",sys);
                request.Headers.Set("userId",userId);
                request.Headers.Set("password",password);
            }
        }
        public static Boolean isEncrypt()
        {
            String encrypt_switch = getGlobalParam("*", rest_message_encrypt_switch);
            if (!"".Equals(encrypt_switch)&& encrypt_switch.GetType() != typeof(DBNull)&&
                ("true".Equals(encrypt_switch.ToLower()) || "on".Equals(encrypt_switch.ToLower()) 
                || "yes".Equals(encrypt_switch.ToLower()) || "1".Equals(encrypt_switch.ToLower())))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static String getRestMessageEncryptKey()
        {
            String encrypt_key = getGlobalParam("*", rest_message_encrypt_key);
            return encrypt_key;
        }
        public static String getGlobalParam(String orgId, String key)
        {
            String sql = "SELECT T.F_VALUE FROM LSSYCS T WHERE T.F_VKEY = '?' AND T.F_JGBH = '?'";
            List<Dictionary<String,Object>> list = new BaseDao().executeQuery(sql,new List<object> { key,orgId});
            String value = "";
            if (list.Count == 0)
            {
                list = new BaseDao().executeQuery(sql, new List<object> { key, "*" });
            }
            if (list.Count != 0) {
                value = list[0]["F_VALUE"].ToString();
            }
            return value;
        }

        public static String encrypt(String src, String keys)
        {
            byte[] k = System.Text.Encoding.UTF8.GetBytes(keys);
            byte[] key = null;
            //默认24位超过截取
            if (k.Length > 24)
            {
                key = new byte[24];
                Array.Copy(k, key, 24);
            }
            else {
                key = k;
            }
            byte[] data = System.Text.Encoding.UTF8.GetBytes(src);
            byte[] iv = new byte[] { 1,2,3,4,5,6,7,8};
            try
            {
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                byte[] ret = mStream.ToArray();
                cStream.Close();
                mStream.Close();
                return encode(Convert.ToBase64String(ret).Replace("\r", "").Replace("\n", ""));
            }
            catch (CryptographicException e)
            {
                throw new Exception("DES3加密失败;" + e.Message);
            }
        }
        public static String decryptThreeDESECB(String src, String keys)
        {
            src = decode(src);

            byte[] k = System.Text.Encoding.UTF8.GetBytes(keys);
            byte[] key = null;
            //默认24位超过截取
            if (k.Length > 24)
            {
                key = new byte[24];
                Array.Copy(k, key, 24);
            }
            else
            {
                key = k;
            }
            byte[] data = Convert.FromBase64String(src);
            byte[] iv = new byte[] { 1, 2, 3 };
            try
            {
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                byte[] fromEncrypt = new byte[data.Length];
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                return System.Text.Encoding.UTF8.GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                throw new Exception("DES3解密失败:" + e.Message);
            }
        }
        public static String encode(String str)
        {
            try
            {
                String hexString = "0123456789ABCDEF";
                //根据默认编码获取字节数组
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
                StringBuilder sb = new StringBuilder(bytes.Length * 2);
                //将字节数组中每个字节拆解成2位16进制整数
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(hexString[(bytes[i] & 0xf0) >> 4]);
                    sb.Append(hexString[((bytes[i] & 0x0f) >> 0)]);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("16进制编码:" + e.Message);
            }
        }
        public static String decode(String bytes)
        {
            try
            {
                String hexString = "0123456789ABCDEF";
                byte[] outputByteArray = new byte[bytes.Length/2];
                for (int i = 0, j = 0; i < bytes.Length; i += 2, j++)
                    outputByteArray[j] = (byte)(hexString.IndexOf(bytes[i]) << 4 | hexString.IndexOf(bytes[i + 1]));
                MemoryStream Stream = new MemoryStream();
                Stream.Write(outputByteArray, 0, outputByteArray.Length);
                return Encoding.UTF8.GetString(Stream.ToArray());
            }
            catch (Exception e)
            {
                throw new Exception("16进制解码失败:"+e.Message);
            }
        }
    }
}
