using BaseLib_Net6;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAVista.Base.Sequence
{
    internal partial class FMainSeq : FThread
    {
        public FMainSeq()
        {
            _printf = new FPrintf[(int)SEQ.SEQ_MAX];
            _printf[(int)SEQ.FLOW] = new(@"log\Seq\", "Flow.log");
            _write = WRITE.IDLE;
            _seq = new((int)SEQ.SEQ_MAX);
            _calendar = "rightCnt";
            _dayIndex = 0;
            _retryCount = 0;
        }

        public void InitSeq()
        {
            if (IsStart)
            {
                return;
            }
            IsStart = true;
            Pause = false;
            SetThreadInterval(eTHREAD.TH1, 10);
            CreateThread(eTHREAD.TH1);

        }
        public long GetSubSeq(SEQ data)
        {
            if (Enum.IsDefined(data.GetType(), data) == false)
            {
                return 0;
            }
            return _seq[data];
        }
        public WRITE GetWriteSeq()
        {
            return _write;
        }
        public long GetWriteSubSeq()
        {
            return GetSubSeq(SEQ.FLOW);
        }
        public void SeqMoveNext()
        {
            if (IsStart == false || Pause == false)
            {
                FBaseFunc.Ins.Print("시작한 뒤, 정지상태에서만 가능");
                return;
            }

            int thread = (int)SEQ.FLOW;
            _seq[SEQ.FLOW]++;

            switch (_write)
            {
                case WRITE.IDLE:
                    {
                        if (_seq[SEQ.FLOW] > 2)
                        {
                            _seq[SEQ.FLOW] = 0;
                            _write++;
                            _printf[thread].PRINT_F("[Seq : ??]\tSkip");
                            _printf[thread].PRINT_F("</SeqIdle>");
                        }
                    }
                    break;
                case WRITE.START:
                    {
                        if (_seq[SEQ.FLOW] > 3)
                        {
                            _seq[SEQ.FLOW] = 0;
                            _write++;
                            _printf[thread].PRINT_F("[Seq : ??]\tSkip");
                            _printf[thread].PRINT_F("</SeqStart>");
                        }
                    }
                    break;
                case WRITE.LOGIN:
                    {
                        if (_seq[SEQ.FLOW] > 3)
                        {
                            _seq[SEQ.FLOW] = 0;
                            _write++;
                            _printf[thread].PRINT_F("[Seq : ??]\tSkip");
                            _printf[thread].PRINT_F("</SeqLogin>");
                        }
                    }
                    break;
                case WRITE.WAIT_DAY:
                    {
                        if (_seq[SEQ.FLOW] > 6)
                        {
                            _seq[SEQ.FLOW] = 0;
                            _write++;
                            _printf[thread].PRINT_F("[Seq : ??]\tSkip");
                            _printf[thread].PRINT_F("</SeqWaitDay>");
                        }
                    }
                    break;
                case WRITE.OCR:
                    {
                        if (_seq[SEQ.FLOW] > 2)
                        {
                            _seq[SEQ.FLOW] = 0;
                            _write++;
                            _printf[thread].PRINT_F("[Seq : ??]\tSkip");
                            _printf[thread].PRINT_F("</SeqOCR>");
                        }
                    }
                    break;
            }

            if (_write == WRITE.SEQ_MAX)
            {
                _write = WRITE.IDLE;
            }

            FBaseFunc.Ins.Print($"Write Sequence : {_write} Step, sub : {_seq[SEQ.FLOW]}");
        }
        public void TerminateSeq()
        {
            TerminateOn = true;
            CloseThread();
            try
            {
                if (_driver != null)
                {
                    _driver.Close();
                    _driver = null;
                }
            }
            catch
            {

            }
            WaitThreadTerminate();
        }

        public override bool ProcThread1()
        {
            SEQ thread = SEQ.FLOW;

            if (TerminateOn)
            {
                return false;
            }

            WRITE write = _write;
            long subSeq = _seq[thread];

            RET ret;
            switch (write)
            {
                case WRITE.IDLE: ret = SeqIdle(ref subSeq, ref write); break;
                case WRITE.START: ret = SeqStart(ref subSeq, ref write); break;
                case WRITE.LOGIN: ret = SeqLogin(ref subSeq, ref write); break;
                case WRITE.WAIT_DAY: ret = SeqWaitDay(ref subSeq, ref write); break;
                case WRITE.OCR: ret = SeqOCR(ref subSeq, ref write); break;
                default: return false;
            }

            switch (ret)
            {
                case RET.OK:
                    {
                        _write++;
                        if (_write == WRITE.SEQ_MAX)
                        {
                            _write = WRITE.IDLE;
                        }
                        _seq[thread] = 0;
                    }
                    break;
                case RET.WAIT: _seq[thread] = subSeq; break;
                case RET.JUMP: _write = write; _seq[thread] = 0; break;
                case RET.ERROR: Pause = true; break;
                default: return false;
            }

            return true;
        }

        public void Test()
        {
            try
            {
                if (Driver == null)
                {
                    EdgeDriverService service = EdgeDriverService.CreateDefaultService(Cfg.EdgeDriverPath);
                    service.HideCommandPromptWindow = true;
                    EdgeOptions options = new()
                    {
                        BinaryLocation = Cfg.EdgePath,
                    };
                    Driver = new(service, options);
                    Driver.Manage().Window.Maximize();
                }
                else
                {
                    try
                    {
                        if (Driver.WindowHandles.Count > 0)
                        {
                            if (Cfg.NewTab)
                            {
                                Driver.ExecuteScript("window.open()");
                                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                            }
                        }
                        else
                        {
                            EdgeDriverService service = EdgeDriverService.CreateDefaultService(Cfg.EdgeDriverPath);
                            service.HideCommandPromptWindow = true;
                            EdgeOptions options = new()
                            {
                                BinaryLocation = Cfg.EdgePath,
                            };
                            Driver = new(service, options);
                            Driver.Manage().Window.Maximize();
                        }
                    }
                    catch (WebDriverException ex)
                    {
                        FBaseFunc.Ins.Print(ex.Message);
                        EdgeDriverService service = EdgeDriverService.CreateDefaultService(Cfg.EdgeDriverPath);
                        service.HideCommandPromptWindow = true;
                        EdgeOptions options = new()
                        {
                            BinaryLocation = Cfg.EdgePath,
                        };
                        Driver = new(service, options);
                        Driver.Manage().Window.Maximize();
                    }
                }

                Driver?.Navigate().GoToUrl("https://nid.naver.com/");

                var loginBtn = Driver?.FindElement(By.Id("log.login"));
                var loginText = loginBtn?.Text;
                loginText = "";
                Driver?.ExecuteScript("alert('asdf');");
                if (IsAlertOpen())
                {
                    loginText = loginBtn?.Text;
                }
                else
                {
                    loginText = loginBtn?.Text;
                }
            }
            catch (Exception ex)
            {
                FBaseFunc.Ins.Print(ex.Message);
            }
        }

        private EdgeDriver? Driver
        {
            get
            {
                if (_driver == null)
                {
                    return _driver;
                }

                try
                {
                    WebDriverWait wait = new(_driver, TimeSpan.FromMilliseconds(200));
                    if (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent()) != null)
                    {
                        _driver?.SwitchTo().Alert().Accept();
                    }
                }
                catch
                {

                }

                return _driver;
            }
            set
            {
                _driver = value;
            }
        }
        private bool IsAlertOpen()
        {
            if (_driver == null)
            {
                return false;
            }

            try
            {
                WebDriverWait wait = new(_driver, TimeSpan.FromMilliseconds(200));
                if (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent()) != null)
                {
                    _driver?.SwitchTo().Alert().Accept();
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
        private IWebElement? FindElement(EdgeDriver? driver, string data, FBaseFunc.ElementType type, int timeout = 0)
        {
            if (timeout == 0)
            {
                timeout = Cfg.Timeout;
            }

            WebDriverWait wait = new(driver, TimeSpan.FromMilliseconds(timeout));
            try
            {
                return type switch
                {
                    FBaseFunc.ElementType.XPATH => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(data))),
                    FBaseFunc.ElementType.ID => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(data))),
                    FBaseFunc.ElementType.CLASS => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName(data))),
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                FBaseFunc.Ins.Print(ex.Message);
                return null;
            }
        }
    }
}
