using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base
{
    internal partial class FBaseFunc
    {
        public enum ElementType
        {
            XPATH,
            ID,
            CLASS,
        }
        [FlagsAttribute]
        public enum ExecutionState : uint
        {
            /// <summary> 
            /// ES_SYSTEM_REQUIRED 
            /// </summary> 
            ES_SYSTEM_REQUIRED = 0x00000001,
            /// <summary> 
            /// ES_DISPLAY_REQUIRED 
            /// </summary> 
            ES_DISPLAY_REQUIRED = 0x00000002,
            /// <summary> 
            /// ES_AWAYMODE_REQUIRED 
            /// </summary> 
            ES_AWAYMODE_REQUIRED = 0x00000040,
            /// <summary> 
            /// ES_CONTINUOUS 
            /// </summary> 
            ES_CONTINUOUS = 0x80000000,

            ES_ALL = ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED | ES_AWAYMODE_REQUIRED | ES_CONTINUOUS
        }

        public class LoginInfo : INotifyPropertyChanged
        {
            /// <summary>
            /// id
            /// </summary>
            private string id;
            /// <summary>
            /// id
            /// </summary>
            private string pw;
            public LoginInfo()
            {
                id = "id";
                pw = "pwd";
            }
            public string LoginID { get => id; set { id = value; OnPropertyChanged(); } }
            public string LoginPW { get => pw; set { pw = value; OnPropertyChanged(); } }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
