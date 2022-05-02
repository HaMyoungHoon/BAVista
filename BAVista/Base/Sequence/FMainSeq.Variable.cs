using BaseLib_Net6;
using BAVista.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base.Sequence
{
    internal partial class FMainSeq
    {
        private FPrintf[] _printf;
        private WRITE _write;
        private readonly FWrappingObject<SEQ, long> _seq;
        private EdgeDriver? _driver;
        private string _calendar;
        private int _dayIndex;
        private int _retryCount;
        public bool TerminateOn { get; set; } = false;
        public bool IsStart { get; set; } = false;
        public bool Reset { get; set; } = false;
        public bool Pause { get; set; } = true;
        public int FlowCount { get; set; } = 0;
        public string LastArticle { get; set; } = "";
        public FBaseConfig Cfg { get => FBaseFunc.Ins.Cfg; }

    }
}
