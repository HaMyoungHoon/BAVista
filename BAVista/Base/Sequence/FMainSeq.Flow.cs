using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BAVista.Base.Sequence
{
    internal partial class FMainSeq
    {
        private RET SeqIdle(ref long subSeq, ref WRITE jumpSeq)
        {
            int thread = (int)SEQ.FLOW;
            if (Pause)
            {
                if (Reset)
                {
                    Pause = false;
                    Reset = false;
                    FlowCount = 0;
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tReset On");
                    _printf[thread].PRINT_F("</SeqIdle>");
                    subSeq = 0;
                    jumpSeq = WRITE.IDLE;
                    return RET.JUMP;
                }
                return RET.WAIT;
            }

            if (subSeq == 0)
            {
                _printf[thread].PRINT_F("<SeqIdle>");
                subSeq = 1;
                return RET.WAIT;
            }

            if (subSeq == 1)
            {
                if (Cfg.FlowCountMax < 0 || (Cfg.FlowCountMax >= 0 && FlowCount < Cfg.FlowCountMax))
                {
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tFlow Count : {FlowCount}, Flow Count Max : {Cfg.FlowCountMax}");
                    FlowCount++;
                    subSeq = 2;
                }

                return RET.WAIT;
            }

            if (subSeq == 2)
            {
                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tSeqIdle End");
                _printf[thread].PRINT_F("</SeqIdle>");
                return RET.OK;
            }

            _printf[thread].PRINT_F("뭔가 잘못 짜서 쓰레드 사망.");
            return RET.ABORT;
        }
        private RET SeqStart(ref long subSeq, ref WRITE jumpSeq)
        {
            int thread = (int)SEQ.FLOW;
            if (Pause)
            {
                if (Reset)
                {
                    Pause = false;
                    Reset = false;
                    FlowCount = 0;
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tReset On");
                    _printf[thread].PRINT_F("</SeqStart>");
                    subSeq = 0;
                    jumpSeq = WRITE.IDLE;
                    return RET.JUMP;
                }
                return RET.WAIT;
            }

            if (subSeq == 0)
            {
                _printf[thread].PRINT_F("<SeqStart>");
                subSeq = 1;
            }

            if (subSeq == 1)
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
                        _printf[thread].PRINT_F($"[Seq : {subSeq}]\tDriver Set = {Cfg.EdgeDriverPath}");
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
                                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tDriver New Tab");
                                }

                                subSeq = 3;
                                return RET.WAIT;
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
                                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tDriver Set = {Cfg.EdgeDriverPath}");
                            }
                        }
                        catch (WebDriverException ex)
                        {
                            _printf[thread].PRINT_F($"[Seq : {subSeq}]\t{ex.Message}");
                            FBaseFunc.Ins.Print(ex.Message);
                            EdgeDriverService service = EdgeDriverService.CreateDefaultService(Cfg.EdgeDriverPath);
                            service.HideCommandPromptWindow = true;
                            EdgeOptions options = new()
                            {
                                BinaryLocation = Cfg.EdgePath,
                            };
                            Driver = new(service, options);
                            Driver.Manage().Window.Maximize();
                            _printf[thread].PRINT_F($"[Seq : {subSeq}]\tDriver Set = {Cfg.EdgeDriverPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\t{ex.Message}");
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                subSeq = 2;
                return RET.WAIT;
            }

            if (subSeq == 2)
            {
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/GOLFCALENDAR") == true)
                    {
                    }
                }
                catch
                {

                }

                Driver?.Navigate().GoToUrl(Cfg.CalendarUrl);
                subSeq = 3;
                return RET.WAIT;
            }

            if (subSeq == 3)
            {
                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tSeqStart End");
                _printf[thread].PRINT_F("</SeqStart>");
                return RET.OK;
            }

            _printf[thread].PRINT_F("뭔가 잘못 짜서 쓰레드 사망.");
            return RET.ABORT;
        }
        private RET SeqLogin(ref long subSeq, ref WRITE jumpSeq)
        {
            int thread = (int)SEQ.FLOW;
            if (Pause)
            {
                if (Reset)
                {
                    Pause = false;
                    Reset = false;
                    FlowCount = 0;
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tReset On");
                    _printf[thread].PRINT_F("</SeqLogin>");
                    subSeq = 0;
                    jumpSeq = WRITE.IDLE;
                    return RET.JUMP;
                }
                return RET.WAIT;
            }

            if (subSeq == 0)
            {
                _printf[thread].PRINT_F("<SeqLogin>");
                subSeq = 1;
            }

            if (subSeq == 1)
            {
                if (Driver == null)
                {
                    jumpSeq = WRITE.START;
                    FBaseFunc.Ins.Print("Driver Null Jump to Start");
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tDriver Null Jump");
                    _printf[thread].PRINT_F("</SeqLogin>");
                    return RET.JUMP;
                }
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/GOLFCALENDAR") == true)
                    {
                        subSeq = 3;
                        return RET.WAIT;
                    }
                    if (Driver?.Url.ToUpper().Contains("LOGIN") == false)
                    {
                        Driver?.Navigate().GoToUrl(FBaseFunc.Ins.Cfg.LoginUrl);
                    }
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                }
                subSeq = 2;
                return RET.WAIT;
            }

            if (subSeq == 2)
            {
                try
                {
                    IWebElement? id = FindElement(_driver, Cfg.LoginSector.LoginID, FBaseFunc.ElementType.ID);
                    IWebElement? pw = FindElement(_driver, Cfg.LoginSector.LoginPW, FBaseFunc.ElementType.ID);
                    IWebElement? login = FindElement(_driver, Cfg.LoginBtn, FBaseFunc.ElementType.CLASS);
                    id?.SendKeys(Cfg.LoginInfo.LoginID);
                    pw?.SendKeys(Cfg.LoginInfo.LoginPW);
                    login?.Click();

                    Thread.Sleep(1000);
                    if (Driver?.Url.ToUpper().Contains("LOGIN") != false)
                    {
                        _printf[thread].PRINT_F($"[Seq : {subSeq}]\tLogin Error");
                        FBaseFunc.Ins.Print("Login Error");
                        return RET.ERROR;
                    }

                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tLogin");
                }
                catch (Exception ex)
                {
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\t{ex.Message}");
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                subSeq = 3;
                return RET.WAIT;
            }

            if (subSeq == 3)
            {
                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tSeqLogin End");
                _printf[thread].PRINT_F("</SeqLogin>");
                return RET.OK;
            }

            _printf[thread].PRINT_F("뭔가 잘못 짜서 쓰레드 사망.");
            return RET.ABORT;
        }
        private RET SeqWaitDay(ref long subSeq, ref WRITE jumpSeq)
        {
            int thread = (int)SEQ.FLOW;
            if (Pause)
            {
                if (Reset)
                {
                    Pause = false;
                    Reset = false;
                    FlowCount = 0;
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tReset On");
                    _printf[thread].PRINT_F("</SeqWaitDay>");
                    subSeq = 0;
                    jumpSeq = WRITE.IDLE;
                    return RET.JUMP;
                }
                return RET.WAIT;
            }

            if (subSeq == 0)
            {
                _printf[thread].PRINT_F("<SeqWaitDay>");
                subSeq = 1;
                return RET.WAIT;
            }

            if (subSeq == 1)
            {
                if (Driver?.Url.ToUpper().Contains("BOOKING/GOLFCALENDAR") == true)
                {
                    subSeq = 2;
                    return RET.WAIT;
                }

                if (Cfg.CalendarUrl.Length <= 0)
                {
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tCalendar Url is Empty");
                    FBaseFunc.Ins.Print("CalendarUrl is Empty");
                    return RET.ERROR;
                }

                Driver?.Navigate().GoToUrl(Cfg.CalendarUrl);
                Thread.Sleep(Cfg.RefreshDelay);

                subSeq = 1;
                return RET.WAIT;
            }

            if (subSeq == 2)
            {
                if (FBaseFunc.Ins.SelectedMonth.Length <= 0)
                {
                    FBaseFunc.Ins.Print("날짜를 선택해주세요.");
                    return RET.ERROR;
                }

                try
                {
                    int loadingBoxCount = 0;
                    var loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    while (loadingBox != null)
                    {
                        if (loadingBoxCount > 30 * 1000)
                        {
                            return RET.WAIT;
                        }
                        if (Pause)
                        {
                            return RET.WAIT;
                        }

                        string display = loadingBox.GetCssValue("display");
                        if (display == "none")
                        {
                            break;
                        }
                        Thread.Sleep(50);
                        loadingBoxCount += 50;
                        loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    }

                    IWebElement? calendar = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS);
                    if (calendar != null)
                    {
                        string buff = calendar.FindElement(By.ClassName("month")).Text;
                        if (buff.Substring(buff.IndexOf(".") + 1, 2) == FBaseFunc.Ins.SelectedMonth)
                        {
                            subSeq = 3;
                            return RET.WAIT;
                        }
                    }

                    _calendar = "leftCnt";
                    calendar = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS);
                    if (calendar != null)
                    {
                        string buff = calendar.FindElement(By.ClassName("month")).Text;
                        if (buff.Substring(buff.IndexOf(".") + 1, 2) == FBaseFunc.Ins.SelectedMonth)
                        {
                            subSeq = 3;
                            return RET.WAIT;
                        }
                    }

                    if (calendar == null)
                    {
                        FBaseFunc.Ins.Print("예약 달력을 찾을 수 없습니다.");
                        return RET.ERROR;
                    }
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                }

                Driver?.Navigate().GoToUrl(Cfg.CalendarUrl);
                Thread.Sleep(Cfg.RefreshDelay);

                return RET.WAIT;
            }

            if (subSeq == 3)
            {
                if (FBaseFunc.Ins.SelectedDay.Length <= 0)
                {
                    FBaseFunc.Ins.Print("날짜를 선택해주세요.");
                    return RET.ERROR;
                }

                try
                {
                    int loadingBoxCount = 0;
                    var loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    while (loadingBox != null)
                    {
                        if (loadingBoxCount > 30 * 1000)
                        {
                            return RET.WAIT;
                        }
                        if (Pause)
                        {
                            return RET.WAIT;
                        }

                        string display = loadingBox.GetCssValue("display");
                        if (display == "none")
                        {
                            break;
                        }
                        Thread.Sleep(50);
                        loadingBoxCount += 50;
                        loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    }

                    ReadOnlyCollection<IWebElement>? day = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS)?.FindElement(By.TagName("tbody"))?.FindElements(By.TagName("a"));
                    if (day == null)
                    {
                        FBaseFunc.Ins.Print("Calendar Not Found Err");
                        return RET.ERROR;
                    }

                    if (int.Parse(FBaseFunc.Ins.SelectedDay) > 15)
                    {
                        for (int i = day.Count - 1; i >= 0; i--)
                        {
                            string buff = day[i].Text;
                            int enterIndex = buff.IndexOf("\r");
                            if (enterIndex > 0)
                            {
                                buff = buff[..enterIndex];
                            }

                            if (buff == FBaseFunc.Ins.SelectedDay)
                            {
                                _dayIndex = i;
                                subSeq = 4;
                                return RET.WAIT;
                            }

                            if (buff.Length > 0)
                            {
                                FBaseFunc.Ins.Print($"{buff}일 skip..");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < day.Count; i++)
                        {
                            string buff = day[i].Text;
                            int enterIndex = buff.IndexOf("\r");
                            if (enterIndex > 0)
                            {
                                buff = buff[..enterIndex];
                            }

                            if (buff == FBaseFunc.Ins.SelectedDay)
                            {
                                _dayIndex = i;
                                subSeq = 4;
                                return RET.WAIT;
                            }

                            if (buff.Length > 0)
                            {
                                FBaseFunc.Ins.Print($"{buff}일 skip..");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                return RET.ERROR;
            }

            if (subSeq == 4)
            {
                try
                {
                    try
                    {
                        int loadingBoxCount = 0;
                        var loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                        while (loadingBox != null)
                        {
                            if (loadingBoxCount > 30 * 1000)
                            {
                                return RET.WAIT;
                            }
                            if (Pause)
                            {
                                return RET.WAIT;
                            }

                            string display = loadingBox.GetCssValue("display");
                            if (display == "none")
                            {
                                break;
                            }
                            Thread.Sleep(50);
                            loadingBoxCount += 50;
                            loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                        }
                    }
                    catch
                    {
                        FBaseFunc.Ins.Print("로딩바를 못 찾음");
                        return RET.WAIT;
                    }

                    ReadOnlyCollection<IWebElement>? day = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS)?.FindElement(By.TagName("tbody"))?.FindElements(By.TagName("a"));
                    if (day == null)
                    {
                        FBaseFunc.Ins.Print("Calendar Not Found Err");
                        if (_retryCount > 2)
                        {
                            _retryCount = 0;
                            return RET.ERROR;
                        }
                        Driver?.Navigate().GoToUrl(Cfg.CalendarUrl);
                        _retryCount++;
                        return RET.WAIT;
                    }

                    if (day.Count <= _dayIndex)
                    {
                        FBaseFunc.Ins.Print("Something Wrong");
                        subSeq = 3;
                        return RET.ERROR;
                    }

                    string styleClass = day[_dayIndex].GetAttribute("class");
                    if (styleClass.ToUpper().Contains("RESERVE"))
                    {
                        day[_dayIndex].Click();
                        subSeq = 5;
                        return RET.WAIT;
                    }

                    if (styleClass.ToUpper().Contains("CLOSE"))
                    {
                        FBaseFunc.Ins.Print("End of Reserve");
                        _printf[thread].PRINT_F($"[Seq : {subSeq}]\tEnd of Reserve");
                        _printf[thread].PRINT_F("</SeqWaitDay>");
                        jumpSeq = WRITE.IDLE;
                        return RET.JUMP;
                    }

                    FBaseFunc.Ins.Print($"{day[_dayIndex].Text}day is Not Reserve State. Refresh...");
                    Driver?.Navigate().GoToUrl(Cfg.CalendarUrl);
                    Thread.Sleep(Cfg.RefreshDelay);
                    return RET.WAIT;
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }
            }

            if (subSeq == 5)
            {
                if (FBaseFunc.Ins.SelectedHour.Length <= 0)
                {
                    FBaseFunc.Ins.Print("시간을 선택해주세요.");
                    return RET.ERROR;
                }

                try
                {
                    int loadingBoxCount = 0;
                    var loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    while (loadingBox != null)
                    {
                        if (loadingBoxCount > 30 * 1000)
                        {
                            return RET.WAIT;
                        }
                        if (Pause)
                        {
                            return RET.WAIT;
                        }

                        string display = loadingBox.GetCssValue("display");
                        if (display == "none")
                        {
                            break;
                        }
                        Thread.Sleep(50);
                        loadingBoxCount += 50;
                        loadingBox = _driver?.FindElement(By.Id("ajaxLoadingBox"));
                    }

                    IWebElement? timeTable = FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS);
                    if (timeTable == null)
                    {
                        FBaseFunc.Ins.Print("Not Found TimeTable");
                        subSeq = 4;
                        return RET.WAIT;
                    }

                    bool firstSkip = false;
                    ReadOnlyCollection<IWebElement>? tr = timeTable.FindElements(By.TagName("tr"));
                    if (int.Parse(FBaseFunc.Ins.SelectedHour) > 11)
                    {
                        for (int i = tr.Count - 1; i >= 0; i--)
                        {
                            string buff = tr[i].Text;
                            if (int.Parse(buff.Substring(buff.IndexOf(":") - 2, 2)) <= int.Parse(FBaseFunc.Ins.SelectedHour))
                            {
                                IWebElement? reserve = tr[i].FindElement(By.TagName("a"));
                                if (reserve?.Text != "예약하기")
                                {
                                    continue;
                                }

                                if (firstSkip == false)
                                {
                                    firstSkip = true;
                                    continue;
                                }

                                reserve?.Click();
                                Thread.Sleep(500);
                                if (IsAlertOpen())
                                {
                                    FBaseFunc.Ins.Print($"{buff.Substring(buff.IndexOf(":") - 2, 5)}시 skip; 다른 회원 예약");
                                    if (FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS) == null)
                                    {
                                        ReadOnlyCollection<IWebElement>? day = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS)?.FindElement(By.TagName("tbody"))?.FindElements(By.TagName("a"));
                                        if (day == null)
                                        {
                                            subSeq = 4;
                                            return RET.WAIT;
                                        }

                                        day[_dayIndex].Click();
                                        IWebElement? timeTablebuff = FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS);
                                        if (timeTablebuff == null)
                                        {
                                            subSeq = 4;
                                            return RET.WAIT;
                                        }

                                        tr = timeTablebuff.FindElements(By.TagName("tr"));
                                    }
                                }
                                else
                                {
                                    subSeq = 6;
                                    return RET.WAIT;
                                }
                            }
                            FBaseFunc.Ins.Print($"{buff.Substring(buff.IndexOf(":") - 2, 5)}시 skip");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tr.Count; i++)
                        {
                            string buff = tr[i].Text;
                            if (int.Parse(buff.Substring(buff.IndexOf(":") - 2, 2)) >= int.Parse(FBaseFunc.Ins.SelectedHour))
                            {
                                IWebElement? reserve = tr[i].FindElement(By.TagName("a"));
                                if (reserve?.Text != "예약하기")
                                {
                                    continue;
                                }

                                if (firstSkip == false)
                                {
                                    firstSkip = true;
                                    continue;
                                }

                                reserve?.Click();
                                Thread.Sleep(500);
                                if (IsAlertOpen())
                                {
                                    FBaseFunc.Ins.Print($"{buff.Substring(buff.IndexOf(":") - 2, 5)}시 skip; 다른 회원 예약");
                                    if (FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS) == null)
                                    {
                                        ReadOnlyCollection<IWebElement>? day = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS)?.FindElement(By.TagName("tbody"))?.FindElements(By.TagName("a"));
                                        if (day == null)
                                        {
                                            subSeq = 4;
                                            return RET.WAIT;
                                        }

                                        day[_dayIndex].Click();
                                        IWebElement? timeTablebuff = FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS);
                                        if (timeTablebuff == null)
                                        {
                                            subSeq = 4;
                                            return RET.WAIT;
                                        }

                                        tr = timeTablebuff.FindElements(By.TagName("tr"));
                                    }
                                }
                                else
                                {
                                    subSeq = 6;
                                    return RET.WAIT;
                                }
                            }
                            FBaseFunc.Ins.Print($"{buff.Substring(buff.IndexOf(":") - 2, 5)}시 skip");
                        }
                    }

                    firstSkip = false;
                    FBaseFunc.Ins.Print($"다시 찾기");
                    for (int i = tr.Count - 1; i >= 0; i--)
                    {
                        string buff = tr[i].Text;
                        IWebElement? reserve = tr[i].FindElement(By.TagName("a"));
                        if (reserve?.Text != "예약하기")
                        {
                            continue;
                        }

                        if (firstSkip == false)
                        {
                            firstSkip = true;
                            continue;
                        }

                        reserve?.Click();
                        Thread.Sleep(1000);
                        if (IsAlertOpen())
                        {
                            FBaseFunc.Ins.Print($"{buff.Substring(buff.IndexOf(":") - 2, 5)}시 skip; 다른 회원 예약");
                            if (FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS) == null)
                            {
                                ReadOnlyCollection<IWebElement>? day = FindElement(_driver, _calendar, FBaseFunc.ElementType.CLASS)?.FindElement(By.TagName("tbody"))?.FindElements(By.TagName("a"));
                                if (day == null)
                                {
                                    subSeq = 4;
                                    return RET.WAIT;
                                }

                                day[_dayIndex].Click();
                                IWebElement? timeTablebuff = FindElement(_driver, "timeTbl", FBaseFunc.ElementType.CLASS);
                                if (timeTablebuff == null)
                                {
                                    subSeq = 4;
                                    return RET.WAIT;
                                }

                                tr = timeTablebuff.FindElements(By.TagName("tr"));
                            }
                        }
                        else
                        {
                            subSeq = 6;
                            return RET.WAIT;
                        }
                    }
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                subSeq = 4;
                return RET.WAIT;
            }

            if (subSeq == 6)
            {
                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tSeqWaitDay End");
                _printf[thread].PRINT_F("</SeqWaitDay>");
                return RET.OK;
            }

            _printf[thread].PRINT_F("뭔가 잘못 짜서 쓰레드 사망.");
            return RET.ABORT;
        }
        private RET SeqOCR(ref long subSeq, ref WRITE jumpSeq)
        {
            int thread = (int)SEQ.FLOW;
            if (Pause)
            {
                if (Reset)
                {
                    Pause = false;
                    Reset = false;
                    FlowCount = 0;
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tReset On");
                    _printf[thread].PRINT_F("</SeqOCR>");
                    subSeq = 0;
                    jumpSeq = WRITE.IDLE;
                    return RET.JUMP;
                }
                return RET.WAIT;
            }

            if (subSeq == 0)
            {
                _printf[thread].PRINT_F("<SeqOCR>");
                bool isJump = false;
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/GOLFCALENDAR") == true)
                    {
                        isJump = true;
                    }
                }
                catch
                {
                    isJump = true;
                }
                finally
                {
                    _printf[thread].PRINT_F($"[Seq : {subSeq}]\tJump to Wait");
                    _printf[thread].PRINT_F("</SeqOCR>");
                }
                if (isJump)
                {
                    jumpSeq = WRITE.WAIT_DAY;
                    return RET.JUMP;
                }
                else
                {
                    subSeq = 1;
                    return RET.WAIT;
                }
            }

            if (subSeq == 1)
            {
                try
                {
                    Driver?.ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
                    Driver?.GetScreenshot().SaveAsFile(@"./ocr/temp.jpeg", ScreenshotImageFormat.Jpeg);
                    using (Process ocr = Process.Start(@"./ocr/CsOCR.exe", "./ocr/temp.jpeg"))
                    {
                        ocr.Close();
                    }
                    Thread.Sleep(2000);
                    string result = File.ReadAllText("./ocr/ret.txt");
                    if (result.Length > 0 && result.ToUpper() != "ERR")
                    {
                        int startIndex = result.IndexOf("문자") + 2;
                        int endIndex = result.IndexOf("빨간색");
                        if (startIndex != -1 && endIndex != -1)
                        {
                            result = result[startIndex..endIndex].Trim();
                            result = Regex.Replace(result, "[^0-9]", "");
                        }
                        else
                        {
                            result = result.Trim();
                            startIndex = result.IndexOf("문자");
                            endIndex = result.IndexOf("빨간색");
                            if (startIndex != -1 && endIndex != -1)
                            {
                                result = result[startIndex..endIndex].Trim();
                            }
                        }

                        IWebElement? confirm = FindElement(_driver, "ConfirmNumber", FBaseFunc.ElementType.ID);
                        confirm?.SendKeys(result);
                        subSeq = 2;
                        return RET.WAIT;
                    }

                    FBaseFunc.Ins.Print("자동입력 방지 코드를 못 찾았습니다.");
                    return RET.ERROR;
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }
            }

            if (subSeq == 2)
            {
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/RESERVEDWITHINFO") == true)
                    {
                        subSeq = 4;
                        return RET.WAIT;
                    }

                    IWebElement? reserve = FindElement(_driver, "col", FBaseFunc.ElementType.CLASS);
                    reserve?.Click();
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                subSeq = 3;
                return RET.WAIT;
            }

            if (subSeq == 3)
            {
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/RESERVEDWITHINFO") == true)
                    {
                        subSeq = 4;
                        return RET.WAIT;
                    }

                    Thread.Sleep(Cfg.Timeout);
                    Driver?.SwitchTo().Alert().Accept();
                    Thread.Sleep(100);
                }
                catch
                {

                }
                try
                {
                    if (Driver?.Url.ToUpper().Contains("BOOKING/RESERVEDWITHINFO") == true)
                    {
                        subSeq = 4;
                        return RET.WAIT;
                    }

                    if (Driver?.Url.ToUpper().Contains("BOOKING/RESERVATIONFORM") == true)
                    {
                        Thread.Sleep(Cfg.RefreshDelay);
                        return RET.WAIT;
                    }
                }
                catch (Exception ex)
                {
                    FBaseFunc.Ins.Print(ex.Message);
                    return RET.ERROR;
                }

                subSeq = 4;
                return RET.WAIT;
            }

            if (subSeq == 4)
            {
                FBaseFunc.Ins.Print("한 바퀴 다 돌고 처음으로");
                FBaseFunc.Ins.SelectedMonth = $"{int.Parse(FBaseFunc.Ins.SelectedDay) + 1}";
                _printf[thread].PRINT_F($"[Seq : {subSeq}]\tSeqOCR End");
                _printf[thread].PRINT_F("</SeqOCR>");
                return RET.OK;
            }

            _printf[thread].PRINT_F("뭔가 잘못 짜서 쓰레드 사망.");
            return RET.ABORT;
        }
    }
}
