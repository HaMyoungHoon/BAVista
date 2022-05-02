using BAVista.Base;
using BAVista.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace BAVista
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _loginUrl;
        private string _calendarUrl;
        private int _timeout;
        private string _loginID;
        private string _loginPW;
        private ObservableCollection<string> _months;
        private ObservableCollection<string> _days;
        private ObservableCollection<string> _hours;
        private string _selectedMonth;
        private string _selectedDay;
        private string _selectedHour;
        private int _threadDelay;
        private int _refreshDelay;
        private int _flowCountMax;
        private bool _newTab;
        private string _retText;
        private string _startText;
        private bool _settingEnable;
        private DispatcherTimer _timer;
        public MainWindowViewModel()
        {
            _loginUrl = FBaseFunc.Ins.Cfg.LoginUrl;
            _calendarUrl = FBaseFunc.Ins.Cfg.CalendarUrl;
            _timeout = FBaseFunc.Ins.Cfg.Timeout;
            _loginID = FBaseFunc.Ins.Cfg.LoginInfo.LoginID;
            _loginPW = "";
            _months = new ObservableCollection<string>();
            _days = new ObservableCollection<string>();
            _hours = new ObservableCollection<string>();
            _selectedMonth = "";
            _selectedDay = "";
            _selectedHour = "";
            _threadDelay = FBaseFunc.Ins.Cfg.ThreadDelay;
            _refreshDelay = FBaseFunc.Ins.Cfg.RefreshDelay;
            _flowCountMax = FBaseFunc.Ins.Cfg.FlowCountMax;
            _newTab = FBaseFunc.Ins.Cfg.NewTab;
            _retText = "";
            _startText = "시작";
            _settingEnable = false;
            _timer = new();
            _timer.Interval = TimeSpan.FromMilliseconds(300);
            _timer.Tick += Timer_Tick;
            _timer.IsEnabled = true;

            SaveUtilCommand = new CommandImpl(SaveUtilEvent);
            SaveLoginCommand = new CommandImpl(SaveLoginEvent);
            SaveOptionCommand = new CommandImpl(SaveOptionEvent);
            StartCommand = new CommandImpl(StartEvent);
            ResetCommand = new CommandImpl(ResetEvent);
            SkipSeqCommand = new CommandImpl(SkipSeqEvent);
            KillCommand = new CommandImpl(KillEvent);
            CloseCommand = new CommandImpl(CloseEvent);

            for (int i = DateTime.Now.Month; i < 13; i++)
            {
                Months.Add(i.ToString("D2"));
            }

            for (int i = 7; i < 18; i++)
            {
                Hours.Add(i.ToString("D2"));
            }

            SelectedMonth = DateTime.Now.AddMonths(1).Month.ToString("D2");
            SelectedDay = DateTime.Now.AddMonths(1).Day.ToString();
            SelectedHour = "08";
        }
        public string LoginUrl
        {
            get => _loginUrl;
            set
            {
                _loginUrl = value;
                OnPropertyChanged();
            }
        }
        public string CalendarUrl
        {
            get => _calendarUrl;
            set
            {
                _calendarUrl = value;
                OnPropertyChanged();
            }
        }
        public int Timeout
        {
            get => _timeout;
            set
            {
                if (value > 1000)
                {
                    _timeout = value;
                }
                else
                {
                    _timeout = 1000;
                }
                OnPropertyChanged();
            }
        }
        public string LoginID
        {
            get => _loginID;
            set
            {
                _loginID = value;
                OnPropertyChanged();
            }
        }
        public string LoginPW
        {
            get => _loginPW;
            set
            {
                _loginPW = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Days
        {
            get => _days;
            set
            {
                _days = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged();
            }
        }
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                FBaseFunc.Ins.SelectedMonth = value;
                _selectedMonth = value;
                OnPropertyChanged();
                if (int.TryParse(value, out int month))
                {
                    int dayCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
                    Days.Clear();
                    for (int i = 1; i < dayCount + 1; i++)
                    {
                        Days.Add(i.ToString());
                    }

                    SelectedDay = "1";
                }
            }
        }
        public string SelectedDay
        {
            get => _selectedDay;
            set
            {
                FBaseFunc.Ins.SelectedDay = value;
                _selectedDay = value;
                OnPropertyChanged();
            }
        }
        public string SelectedHour
        {
            get => _selectedHour;
            set
            {
                FBaseFunc.Ins.SelectedHour = value;
                _selectedHour = value;
                OnPropertyChanged();
            }
        }
        public int ThreadDelay
        {
            get => _threadDelay;
            set
            {
                if (value > 10)
                {
                    _threadDelay = value;
                }
                else
                {
                    _threadDelay = 10;
                }
                OnPropertyChanged();
            }
        }
        public int RefreshDelay
        {
            get => _refreshDelay;
            set
            {
                if (value > 100)
                {
                    _refreshDelay = value;
                }
                else
                {
                    _refreshDelay = 100;
                }
                OnPropertyChanged();
            }
        }
        public int FlowCountMax
        {
            get => _flowCountMax;
            set
            {
                _flowCountMax = value;
                OnPropertyChanged();
            }
        }
        public bool NewTab
        {
            get => _newTab;
            set
            {
                _newTab = value;
                OnPropertyChanged();
            }
        }
        public string RetText
        {
            get => _retText;
            set
            {
                _retText = value;
                OnPropertyChanged();
            }
        }
        public string StartText
        {
            get => _startText;
            set
            {
                _startText = value;
                OnPropertyChanged();
            }
        }
        public bool SettingEnable
        {
            get => _settingEnable;
            set
            {
                _settingEnable = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveUtilCommand { get; init; }
        public ICommand SaveLoginCommand { get; init; }
        public ICommand SaveOptionCommand { get; init; }
        public ICommand StartCommand { get; init; }
        public ICommand ResetCommand { get; init; }
        public ICommand SkipSeqCommand { get; init; }
        public ICommand KillCommand { get; init; }
        public ICommand CloseCommand { get; init; }

        private void SaveUtilEvent(object? obj)
        {
            FBaseFunc.Ins.Cfg.SaveUtil(LoginUrl, CalendarUrl, Timeout);
            FBaseFunc.Ins.Print($"Save {LoginUrl} {CalendarUrl}, {Timeout}");
        }
        private void SaveLoginEvent(object? obj)
        {
            FBaseFunc.Ins.Cfg.SaveLogin(LoginID, LoginPW);
            FBaseFunc.Ins.Print($"Save {LoginID}, {LoginPW}");
            LoginPW = "";
        }
        private void SaveOptionEvent(object? obj)
        {
            FBaseFunc.Ins.Cfg.SaveOption(ThreadDelay, RefreshDelay, FlowCountMax, NewTab);
            FBaseFunc.Ins.Print($"Save {ThreadDelay}, {RefreshDelay}, {FlowCountMax}, {NewTab}");
        }
        private void StartEvent(object? obj)
        {
//            FBaseFunc.Ins.Test();
            FBaseFunc.Ins.SeqStart();
        }
        private void ResetEvent(object? obj)
        {
            FBaseFunc.Ins.SeqReset();
        }
        private void SkipSeqEvent(object? obj)
        {
            FBaseFunc.Ins.Seq.SeqMoveNext();
        }
        private void KillEvent(object? obj)
        {
            if (SettingEnable)
            {
                FBaseFunc.EdgeDriverKill();
                FBaseFunc.Ins.Print("Kill");
            }
        }
        private void CloseEvent(object? obj)
        {
            FBaseFunc.Ins.TerminateSystem();

            if (obj is Window window)
            {
                App.WindowInstance.DestroyWPF();
                window.Close();
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            SettingEnable = FBaseFunc.Ins.Seq.Pause;
            if (SettingEnable)
            {
                StartText = "시작";
            }
            else
            {
                StartText = "중지";
            }

            if (FBaseFunc.Ins.ResultMessage.Count > 0)
            {
                RetText = $"{RetText}\n{FBaseFunc.Ins.ResultMessage.Dequeue()}";
            }
        }

        public void SetMessageHook(Window mother)
        {
            WindowInteropHelper helper = new(mother);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(HookingFunc);
        }
        public IntPtr HookingFunc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x10)
            {
                handled = true;
            }
            return IntPtr.Zero;
        }
        public void IHaveToCloseThis(object sender, MainWindow mother)
        {
            MainWindow? parent = FindParent<MainWindow>((Button)sender);
            if (parent == null)
            {
                parent = mother;
            }
            HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(parent as Visual);
            if (hwndSource != null)
            {
                hwndSource.RemoveHook(new HwndSourceHook(HookingFunc));
            }
            Window.GetWindow(mother).Close();
        }
        private static T? FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
            {
                return null;
            }

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
        public void DestroyWPF(Visual mother)
        {
            HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(mother);
            if (hwndSource != null)
            {
                hwndSource.RemoveHook(new HwndSourceHook(HookingFunc));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}