using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeModel
{
    /// <summary>
    /// 设置webBrowser中的Ie版本及其兼容性问题，修改注册表
    /// </summary>
    static class WebBrowserIE
    {
        
        public static void setIEVersionEmulation(int IEVersion)
        {
            var reg = Registry.LocalMachine;
            var ie = reg.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (ie != null)
            {
                try
                {
                    var emulation = IEVersion;
                    if (emulation != 0)
                    {
                        ie.SetValue(Process.GetCurrentProcess().MainModule.FileName, emulation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("设置IE出错", ex.Message);
                }
            }

            reg.Close();
        }

        /// <summary>
        /// 获取WebBrowser中的IE内核
        /// </summary>
        /// <returns></returns>
        public static int getIEVersionEmulation()
        {
            int ieVersion = 9000;//IE版本号
                                 // String iePath = "SOFTWARE\\WOW6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
            RegistryKey reg = Registry.LocalMachine;
            
            reg = reg.CreateSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Internet Explorer");

            var svcVersion = reg.GetValue("svcVersion");
            var version = reg.GetValue("Version");
            if (svcVersion != null)
            {
                reg.Close();
                ieVersion = int.Parse(svcVersion.ToString().Split('.')[0]);
            }
            else if (version != null)
            {
                reg.Close();
                ieVersion = int.Parse(version.ToString().Split('.')[0]);
            }

            if (ieVersion < 8) return 0;
            if (ieVersion == 8) return 0x1F40;//8000 (0x1F40)、8888 (0x22B8)
            if (ieVersion == 9) return 0x2328;//9000 (0x2328)、9999 (0x270F)
            if (ieVersion == 10) return 0x02710;//10000 (0x02710)、10001 (0x2711)
            if (ieVersion == 11) return 0x2AF8;//11000 (0x2AF8)、11001 (0x2AF9

            return 0;
        }


    }
}
