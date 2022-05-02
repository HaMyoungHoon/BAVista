using BaseLib_Net6;
using BAVista.Base.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base
{
    internal partial class FBaseFunc
    {
        private static FBaseFunc? _ins;
        public static FBaseFunc Ins
        {
            get { return _ins ??= new(); }
        }

        public FBaseConfig Cfg;
        public FMainSeq Seq;

        private FPrintf? _log;

        public Queue<string> ResultMessage;

        public string SelectedMonth;
        public string SelectedDay;
        public string SelectedHour;
    }
}
