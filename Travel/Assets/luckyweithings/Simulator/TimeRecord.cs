using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class TimeRecord : MonoBehaviour {

    // Use this for initialization
    private void Awake(){

        Debug.Log("luckyhigh Time Record start");
        StreamWriter sw = FileManager.GetStreamWriter((FileManager.GetFilePath("Date.rec")));
        if (sw == null)
            return;

        CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        string format = "yyyy-MM-dd hh:mm:ss";
        string stringValue = DateTime.Now.ToString(format, cultureInfo);
        sw.WriteLine(stringValue);
        sw.Close();
    }
}
