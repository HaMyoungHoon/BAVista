using BaseLib_Net6;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base
{
    internal partial class FBaseConfig
    {
        public FBaseConfig()
        {
            LoginInfo = new FBaseFunc.LoginInfo();
            LoginSector = new FBaseFunc.LoginInfo();
        }
        private static bool CheckFolder(string path)
        {
            string temp = path;
            DirectoryInfo filePath = new(path);
            DirectoryInfo di = new(temp.Replace(filePath.Name, ""));

            if (di.Exists == false)
            {
                di.Create();
                return false;
            }

            return true;
        }
        private static bool CheckFile(string path)
        {
            if (File.Exists(path) == false)
            {
                using (File.Create(path))
                {

                }

                return false;
            }

            return true;
        }
        private void CreateSettingFile()
        {
            string filePath = $@".\Setting\{CfgPath}";
            CheckFolder(filePath);
            if (CheckFile(filePath))
            {
                return;
            }

            File.WriteAllText(filePath, "<?xml version='1.0' encoding='UTF-8'?>\n<BAVista>\n</BAVista>");
            FFileParser xmlData = new(filePath, FFileParser.FILE_TYPE.XML);
            CreateUtil(xmlData);
            CreateLogin(xmlData);
            CreateOption(xmlData);
        }
        private void CreateUtil(FFileParser data)
        {
            data.SetString("BAVista,UTIL,LOGIN_URL", LoginUrl);
            data.SetString("BAVista,UTIL,CALENDAR_URL", CalendarUrl);
            data.SetString("BAVista,UTIL,LOG_PATH", LogPath);
            data.SetString("BAVista,UTIL,LOG_FILE", LogFile);
            data.SetString("BAVista,UTIL,EDGE_DRIVER", $@"{Directory.GetCurrentDirectory()}\edgedriver_win64\");
            data.SetString("BAVista,UTIL,EDGE", @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            data.SetInt("BAVista,UTIL,TIMEOUT", Timeout);
        }
        private void CreateLogin(FFileParser data)
        {
            data.SetString("BAVista,LOGIN,VALUE,LOGIN_ID", "");
            data.SetString("BAVista,LOGIN,VALUE,LOGIN_PW", "");

            data.SetString("BAVista,LOGIN,SECTOR,LOGIN_ID", LoginSector.LoginID);
            data.SetString("BAVista,LOGIN,SECTOR,LOGIN_PW", LoginSector.LoginPW);
            data.SetString("BAVista,LOGIN,SECTOR,LOGIN_BTN", LoginBtn);
        }
        private void CreateOption(FFileParser data)
        {
            data.SetInt("BAVista,OPTION,THREAD_DELAY", ThreadDelay);
            data.SetInt("BAVista,OPTION,REFRESH_DELAY", RefreshDelay);
            data.SetInt("BAVista,OPTION,FLOW_COUNT_MAX", FlowCountMax);
            data.SetInt("BAVista,OPTION,NEW_TAB", NewTab ? 1 : 0);
        }

        public void LoadSettingFile()
        {
            CreateSettingFile();
            string filePath = $@"{Directory.GetCurrentDirectory()}\Setting\{CfgPath}";
            FFileParser xmlData = new(filePath, FFileParser.FILE_TYPE.XML);
            LoadUtil(xmlData);
            LoadLogin(xmlData);
            LoadOption(xmlData);
        }
        private void LoadUtil(FFileParser data)
        {
            LoginUrl = data.GetString("BAVista,UTIL,LOGIN_URL", LoginUrl);
            CalendarUrl = data.GetString("BAVista,UTIL,CALENDAR_URL", CalendarUrl);
            LogPath = data.GetString("BAVista,UTIL,LOG_PATH", LogPath);
            LogFile = data.GetString("BAVista,UTIL,LOG_FILE", LogFile);
            EdgeDriverPath = data.GetString("BAVista,UTIL,EDGE_DRIVER", $@"{Directory.GetCurrentDirectory()}\edgedriver_win64\");
            EdgePath = data.GetString("BAVista,UTIL,EDGE", @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            Timeout = data.GetInt("BAVista,UTIL,TIMEOUT", Timeout);
        }
        private void LoadLogin(FFileParser data)
        {
            LoginInfo.LoginID = data.GetString("BAVista,LOGIN,VALUE,LOGIN_ID", LoginInfo.LoginID);
            LoginInfo.LoginPW = data.GetString("BAVista,LOGIN,VALUE,LOGIN_PW", LoginInfo.LoginPW);

            LoginSector.LoginID = data.GetString("BAVista,LOGIN,SECTOR,LOGIN_ID", LoginSector.LoginID);
            LoginSector.LoginPW = data.GetString("BAVista,LOGIN,SECTOR,LOGIN_PW", LoginSector.LoginPW);
            LoginBtn = data.GetString("BAVista,LOGIN,SECTOR,LOGIN_BTN", LoginBtn);
        }
        private void LoadOption(FFileParser data)
        {
            ThreadDelay = data.GetInt("BAVista,OPTION,THREAD_DELAY", ThreadDelay);
            RefreshDelay = data.GetInt("BAVista,OPTION,REFRESH_DELAY", RefreshDelay);
            FlowCountMax = data.GetInt("BAVista,OPTION,FLOW_COUNT_MAX", FlowCountMax);
            NewTab = data.GetInt("BAVista,OPTION,NEW_TAB", NewTab ? 1 : 0) == 1;

            ThreadDelay = ThreadDelay >= 10 ? ThreadDelay : 10;
            RefreshDelay = RefreshDelay >= 100 ? RefreshDelay : 100;
        }

        public void SaveUtil(string loginUrl, string cafeAllViewUrl, int timeout)
        {
            LoginUrl = loginUrl;
            CalendarUrl = cafeAllViewUrl;
            Timeout = timeout;
            string filePath = $@"{Directory.GetCurrentDirectory()}\Setting\{CfgPath}";
            FFileParser xmlData = new(filePath, FFileParser.FILE_TYPE.XML);
            xmlData.SetString("BAVista,UTIL,LOGIN_URL", LoginUrl);
            xmlData.SetString("BAVista,UTIL,CALENDAR_URL", CalendarUrl);
            xmlData.SetString("BAVista,UTIL,LOG_PATH", LogPath);
            xmlData.SetString("BAVista,UTIL,LOG_FILE", LogFile);
            xmlData.SetString("BAVista,UTIL,EDGE_DRIVER", EdgeDriverPath);
            xmlData.SetString("BAVista,UTIL,EDGE", EdgePath);
            xmlData.SetInt("BAVista,UTIL,TIMEOUT", Timeout);
        }
        public void SaveLogin(string loginID, string loginPW)
        {
            LoginInfo.LoginID = loginID;
            LoginInfo.LoginPW = loginPW;
            string filePath = $@"{Directory.GetCurrentDirectory()}\Setting\{CfgPath}";
            FFileParser xmlData = new(filePath, FFileParser.FILE_TYPE.XML);
            xmlData.SetString("BAVista,LOGIN,VALUE,LOGIN_ID", LoginInfo.LoginID);
            xmlData.SetString("BAVista,LOGIN,VALUE,LOGIN_PW", LoginInfo.LoginPW);

            xmlData.SetString("BAVista,LOGIN,SECTOR,LOGIN_ID", LoginSector.LoginID);
            xmlData.SetString("BAVista,LOGIN,SECTOR,LOGIN_PW", LoginSector.LoginPW);
            xmlData.SetString("BAVista,LOGIN,SECTOR,LOGIN_BTN", LoginBtn);
        }
        public void SaveOption(int threadDelay, int refreshDelay, int flowCountMax, bool newTab)
        {
            ThreadDelay = threadDelay;
            RefreshDelay = refreshDelay;
            FlowCountMax = flowCountMax;
            NewTab = newTab;
            string filePath = $@"{Directory.GetCurrentDirectory()}\Setting\{CfgPath}";
            FFileParser xmlData = new(filePath, FFileParser.FILE_TYPE.XML);
            xmlData.SetInt("BAVista,OPTION,THREAD_DELAY", ThreadDelay);
            xmlData.SetInt("BAVista,OPTION,REFRESH_DELAY", RefreshDelay);
            xmlData.SetInt("BAVista,OPTION,FLOW_COUNT_MAX", FlowCountMax);
            xmlData.SetInt("BAVista,OPTION,NEW_TAB", NewTab ? 1 : 0);
        }
    }
}
