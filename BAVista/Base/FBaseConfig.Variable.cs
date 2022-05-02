using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base
{
    internal partial class FBaseConfig
    {
        private const string CfgPath = "BAVista.xml";
        public string LoginUrl { get; set; } = "URL 지움";
        public string CalendarUrl { get; set; } = "URL 지움";
        public string LogPath { get; set; } = @"Log\Craw";
        public string LogFile { get; set; } = "CrawLog.log";
        public string EdgeDriverPath { get; set; } = "";
        public string EdgePath { get; set; } = "";
        public int Timeout { get; set; } = 2000;

        public FBaseFunc.LoginInfo LoginInfo { get; set; }
        public FBaseFunc.LoginInfo LoginSector { get; set; }

        /// <summary>
        /// id, only sector
        /// </summary>
        public string LoginBtn { get; set; } = "loginBtn";

        public string Month { get; set; } = "01";
        public string Day { get; set; } = "01";

        public int ThreadDelay { get; set; } = 10;
        public int RefreshDelay { get; set; } = 1000;
        public int FlowCountMax { get; set; } = 1;
        public bool NewTab { get; set; } = false;
    }
}
