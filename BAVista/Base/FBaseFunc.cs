using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base
{
    internal partial class FBaseFunc
    {
        public FBaseFunc()
        {
            Cfg = new();
            Seq = new();
            ResultMessage = new();
            SelectedMonth = "";
            SelectedDay = "";
            SelectedHour = "";
        }
        public void InitSystem()
        {
            Cfg.LoadSettingFile();
            Seq.SetThreadInterval(BaseLib_Net6.FThread.eTHREAD.TH1, Cfg.ThreadDelay);
            _log = new(Cfg.LogPath, Cfg.LogFile);
        }
        public void TerminateSystem()
        {
            Seq.TerminateSeq();
            EdgeDriverKill();
        }
        public void SeqStart()
        {
            if (Seq.IsStart)
            {
                Seq.Pause = !Seq.Pause;
                Print(Seq.Pause ? "중지" : "재시작");
            }
            else
            {
                Seq.InitSeq();
                Print("시작");
            }
        }
        public void SeqReset()
        {
            if (Seq.Pause)
            {
                Seq.Reset = true;
                Print("리셋");
            }
        }

        public void Test()
        {
            Seq.Test();
        }

        public void Print(string data)
        {
            _log?.PRINT_F(data);
            ResultMessage.Enqueue(data);
        }
        public static void EdgeDriverKill()
        {
            Process[] process = Process.GetProcessesByName("msedgedriver");
            if (process.Length > 0)
            {
                foreach (Process p in process)
                {
                    p.Kill();
                }
            }
        }
    }
}
