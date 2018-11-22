using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class TimePlay : MonoBehaviour
{

#if UNITY_EDITOR

    [System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
    public static extern bool SetLocalTime(ref Systemtime sysTime);

    /// <summary>
    /// 时间结构体
    /// </summary>
    public struct Systemtime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        /// <summary>
        /// 从System.DateTime转换。
        /// </summary>
        /// <param name="time">System.DateTime类型的时间。</param>
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
        /// <summary>
        /// 转换为System.DateTime类型。
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        /// <summary>
        /// 静态方法。转换为System.DateTime类型。
        /// </summary>
        /// <param name="time">SYSTEMTIME类型的时间。</param>
        /// <returns></returns>
        public static DateTime ToDateTime(Systemtime time)
        {
            return time.ToDateTime();
        }

        public override string ToString()
        {
            return wYear + " " + wMonth + " " + wDay + " " + wHour + " " + wMinute + " " + wSecond;
        }
    }

    private DateTime now;
    private DateTime record;
    private bool success = true;

    private const string format = "yyyy-MM-dd hh:mm:ss";

    private void Awake()
    {
        Debug.Log("lucky start play time");
        StreamReader sr = FileManager.GetStreamReader((FileManager.GetFilePath("Date.rec")));

        if (sr == null)
        {
            success = false;
            return;
        }
            

        string s = sr.ReadToEnd();
        sr.Close();

        Debug.Log("lucky record " + s);

        record = Convert.ToDateTime(s);
        now = DateTime.Now;

        Systemtime st = new Systemtime();
        st.FromDateTime(record);

        var result = SetLocalTime(ref st);
        Debug.Log("lucky set time " + result);
    }

    private void OnDestroy()
    {
        if (!success)
            return;

        DateTime wrongNow = DateTime.Now;
        TimeSpan ts = wrongNow - record;
        now = now.AddMilliseconds(ts.TotalMilliseconds);

        Systemtime st = new Systemtime();
        st.FromDateTime(now);

        var result = SetLocalTime(ref st);

    }

#endif

}
