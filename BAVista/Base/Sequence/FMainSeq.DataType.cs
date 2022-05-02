using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base.Sequence
{
    internal partial class FMainSeq
    {
        public enum SEQ
        {
            FLOW = 0,
            SEQ_MAX,
        }
        public enum WRITE
        {
            IDLE = 0,
            START,
            LOGIN,
            WAIT_DAY,
            OCR,
            SEQ_MAX,
        }
        public enum RET
        {
            ABORT = -2,
            ERROR = -1,
            OK = 0,
            WAIT = 1,
            JUMP = 2,
        }
    }
}
