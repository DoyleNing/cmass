using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMachine
{
    static class BaseUtilsClass
    {
        public static String OpenAndGetFilePath()
        {
            string foldPath = "";
            String localFile = DataSource.GetSettingString("localFile");
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择导出文件路径";
            if (localFile != null || localFile != "")
            {
                dialog.SelectedPath = localFile;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foldPath = dialog.SelectedPath;
                DataSource.UpdateSettingString("localFile", foldPath);
            }
            return foldPath;
        }
    }
}
